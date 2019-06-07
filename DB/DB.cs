using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.NET;
using MediaPortal.Configuration;
using MediaPortal.Database;
using MediaPortal.GUI.Library;
using System.IO;

namespace MyEmulators2
{
    public class DB : IDisposable
    {
        const string DB_FILE_NAME = "Emulators2_v1.db3";
        const double DB_VERSION = 1.7;
        const int MAX_TRANSACTION_STATEMENTS = 500;

        #region Singleton

        static object instanceSync = new object();
        static DB instance = null;
        public static DB Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceSync)
                        if (instance == null)
                            instance = new DB();
                return instance;
            }
        }

        #endregion

        public DB()
        {
            init();
        }
        
        SQLiteClient sqlClient = null;
        bool isInit = false;

        void init()
        {
            lock (syncRoot)
            {
                if (isInit)
                    return;
                                
                string dbFile = Config.GetFile(Config.Dir.Database, DB_FILE_NAME);
                bool exists = File.Exists(dbFile);
                sqlClient = new SQLiteClient(dbFile);
                DatabaseUtility.AddTable(sqlClient, Emulator.TABLE_NAME, Emulator.DBTableString);
                DatabaseUtility.AddTable(sqlClient, EmulatorProfile.TABLE_NAME, EmulatorProfile.DBTableString);
                DatabaseUtility.AddTable(sqlClient, Game.TABLE_NAME, Game.DBTableString);
                DatabaseUtility.AddTable(sqlClient, GameDisc.TABLE_NAME, GameDisc.DBTableString);

                DatabaseUtility.AddTable(sqlClient, "Info",
                    "CREATE TABLE Info(" +
                        "name varchar(50)," +
                        "value varchar(100)," +
                        "PRIMARY KEY(name)" +
                    ")"
                );

                isInit = true;
                
                SQLiteResultSet result = sqlClient.Execute("SELECT value FROM Info WHERE name='version'");
                if (result.Rows.Count == 0)
                {
                    if (exists)
                    {
                        sqlClient.Execute("INSERT INTO Info VALUES('version','1.7')");
                        isInit = false;
                        sqlClient.Dispose();
                        init();
                    }
                    else
                        UpdateDBVersion();
                    return;
                }

                double currentVersion = double.Parse(result.Rows[0].fields[0], System.Globalization.CultureInfo.InvariantCulture);
                if (currentVersion == DB_VERSION)
                    return;

                if (!backupDBFile())
                {
                    isInit = false;
                    return;
                }

                if (updateDatabase(currentVersion))
                {
                    UpdateDBVersion();
                    return;
                }

                isInit = false;
                sqlClient.Dispose();
                Logger.LogInfo("Deleting incompatible database (backup has been created)");
                try
                {
                    File.Delete(dbFile);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to delete database file, try deleting {0} manually - {1}", dbFile, ex.Message);
                    return;
                }
                init();
            }
        }

        bool updateDatabase(double currentVersion)
        {
            if (currentVersion > DB_VERSION)
            {
                Logger.LogError("Database is from a newer version of the plugin and cannot be used");
                return false;
            }

            Logger.LogInfo("Updating database from v{0} to v{1}", currentVersion, DB_VERSION);
            if (currentVersion < 1.3)
            {
                Logger.LogError("Database is from a pre-beta version and cannot be upgraded");
                return false;
            }
            
            if (new DB_Updater().Update())
            {
                return true;
            }
            else
            {
                Logger.LogError("Database update failed");
            }
            return false;
        }

        bool backupDBFile()
        {
            FileInfo dbFile = new FileInfo(Config.GetFile(Config.Dir.Database, DB_FILE_NAME));
            if (!dbFile.Exists)
                return true;

            Logger.LogInfo("Backing up current database");
            string backupFolder = Path.Combine(Config.GetFolder(Config.Dir.Database), string.Format("Emulators2_backup_{0}", DateTime.Now.ToString("yyyy_MM_dd_HHmm")));
            string backupPath = Path.Combine(backupFolder, DB_FILE_NAME);
            try
            {
                DirectoryInfo dir = new DirectoryInfo(backupFolder);
                if (!dir.Exists)
                    dir.Create();
                dbFile.CopyTo(backupPath, true);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to create backup of database at {0} - {1}, {2}\n{3}", backupPath, ex, ex.Message, ex.StackTrace);
                return false;
            }
            Logger.LogInfo("Successfully created backup of database at {0}", backupPath);
            return true;
        }

        readonly object syncRoot = new object();
        public object SyncRoot
        {
            get { return syncRoot; }
        }

        bool supressExceptions = true;
        public bool SupressExceptions
        { 
            get { return supressExceptions; } 
            set { supressExceptions = value; } 
        }

        public SQLiteResultSet Execute(string query, params object[] args)
        {
            lock (syncRoot)
                return ExecuteWithoutLock(query, args);
        }

        public SQLiteResultSet ExecuteWithoutLock(string query, params object[] args)
        {
            if (!isInit)
            {
                Logger.LogError("Database has not initialised correctly");
                return new SQLiteResultSet();
            }
            try
            {
                string exeQuery = string.Format(System.Globalization.CultureInfo.InvariantCulture, query, args);
                SQLiteResultSet result = sqlClient.Execute(exeQuery);
                return result;
            }
            catch (Exception ex)
            {
                if (supressExceptions)
                {
                    Logger.LogError(ex);
                    return new SQLiteResultSet();
                }
                else
                    throw (ex);
            }
        }

        public void ExecuteTransaction<T>(IEnumerable<T> items, Action<T> sqlHandler)
        {
            lock (syncRoot)
            {
                int itemNo = 0;
                ExecuteWithoutLock("BEGIN");
                foreach (T item in items)
                {
                    if (itemNo == MAX_TRANSACTION_STATEMENTS)
                    {
                        ExecuteWithoutLock("COMMIT");
                        ExecuteWithoutLock("BEGIN");
                        itemNo = 0;
                    }
                    else
                        itemNo++;

                    sqlHandler(item);
                }
                ExecuteWithoutLock("COMMIT");
            }
        }

        public double CurrentDBVersion
        {
            get
            {
                SQLiteResultSet result = Execute("SELECT value FROM Info WHERE name='version'");
                if (result.Rows.Count == 0)
                    return DB_VERSION; //Fresh install
                else
                    return double.Parse(result.Rows[0].fields[0], System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public void UpdateDBVersion()
        {
            lock (syncRoot)
            {
                SQLiteResultSet result = ExecuteWithoutLock("SELECT value FROM Info WHERE name='version'");
                string query;
                if (result.Rows.Count == 0)
                    query = "INSERT INTO Info VALUES('version','{0}')";
                else
                    query = "UPDATE Info SET value='{0}' WHERE name='version'";

                ExecuteWithoutLock(query, DB_VERSION);
            }
        }

        #region Emulators

        public Emulator GetEmulator(int uid)
        {
            if (uid == -1)
                return Emulator.GetPC();

            SQLiteResultSet result = Execute("SELECT * FROM {0} WHERE uid={1}", Emulator.TABLE_NAME, uid);
            if (result.Rows.Count < 1)
            {
                Logger.LogError("Unable to locate Emulator with uid {0}", uid);
                return null;
            }

            return Emulator.CreateEmulator(result.Rows[0]);
        }

        public Dictionary<int, Emulator> GetEmulators(List<int> ids)
        {
            Dictionary<int, Emulator> idDict = new Dictionary<int, Emulator>();
            if (ids.Count < 1)
                return idDict;

            foreach (Emulator emu in GetEmulators())
                idDict[emu.UID] = emu;

            return idDict;
        }

        public List<Emulator> GetEmulators(bool forcePC = false)
        {
            List<Emulator> emus = new List<Emulator>();
            SQLiteResultSet result = Execute("SELECT * FROM {0} ORDER BY position ASC", Emulator.TABLE_NAME);
            foreach (SQLiteResultSet.Row row in result.Rows)
                emus.Add(Emulator.CreateEmulator(row));

            Emulator pc = Emulator.GetPC();
            if (forcePC || GetGames(pc).Count > 0)
            {
                //sanitise position just in case
                if (pc.Position < 0)
                    pc.Position = 0;
                else if (pc.Position > emus.Count)
                    pc.Position = emus.Count;
                emus.Insert(pc.Position, pc); //Insert PC into correct position in list
            }

            return emus;
        }

        #endregion

        #region Games

        public List<string> GetAllGamePaths()
        {
            List<string> paths = new List<string>();

            SQLiteResultSet result = Execute("SELECT path FROM {0}", Game.TABLE_NAME);
            foreach (SQLiteResultSet.Row row in result.Rows)
                paths.Add(Decode(row.fields[0]));

            result = Execute("SELECT path FROM {0}", GameDisc.TABLE_NAME);
            foreach (SQLiteResultSet.Row row in result.Rows)
                paths.Add(Decode(row.fields[0]));

            return paths;
        }
        
        /// <summary>
        /// Returns the game with the specified id. If no database entry is found
        /// this returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Game GetGame(int id)
        {
            List<Game> games = GetGames("WHERE gameid=" + id);
            if (games.Count < 1)
            {
                Logger.LogError("Unable to locate Game with id {0}", id);
                return null;
            }

            return games[0];
        }

        /// <summary>
        /// Returns all games in the database.
        /// </summary>
        /// <returns></returns>
        public List<Game> GetGames()
        {
            return GetGames("");
        }

        /// <summary>
        /// Returns all games belonging to the specified emulator.
        /// </summary>
        /// <param name="emu"></param>
        /// <returns></returns>
        public List<Game> GetGames(Emulator emu)
        {
            if (emu == null)
                return new List<Game>();

            return GetGames("WHERE parentemu=" + emu.UID);
        }

        /// <summary>
        /// Returns all games selected by the specified SQL Query.
        /// The default 'ORDER BY' clause will be automatically appended. 
        /// </summary>
        /// <param name="sqlAppend">The 'WHERE' clause to append to the internal 'SELECT' statement.</param>
        /// <returns></returns>
        public List<Game> GetGames(string sqlAppend)
        {
            return GetGames(sqlAppend, true, false);
        }

        /// <summary>
        /// Returns all games selected by the specified SQL Query.
        /// If <paramref name="defaultSort"/> is false then <paramref name="sqlAppend"/>
        /// should contain an 'ORDER BY' clause.
        /// </summary>
        /// <param name="sqlAppend">The 'WHERE' clause to append to the internal 'SELECT' statement.</param>
        /// <param name="defaultSort">
        /// Whether the default 'ORDER BY' clause should be appended to the query.
        /// Default True.
        /// </param>
        /// <returns></returns>
        public List<Game> GetGames(string sqlAppend, bool defaultSort)
        {
            return GetGames(sqlAppend, defaultSort, false);
        }

        public List<Game> GetGames(string sqlAppend, bool defaultSort, bool showOrphans)
        {
            List<Game> games = new List<Game>();

            string query = "SELECT * FROM " + Game.TABLE_NAME;

            if (!string.IsNullOrEmpty(sqlAppend))
                query += " " + sqlAppend;

            if (defaultSort)
                query += " ORDER BY parentemu, title";

            SQLiteResultSet result = Execute(query);
            Dictionary<int, Emulator> emuDict = new Dictionary<int, Emulator>();

            foreach (SQLiteResultSet.Row row in result.Rows)
            {
                int emuId = int.Parse(row.fields[2]);
                Emulator parentEmu;
                if (!emuDict.TryGetValue(emuId, out parentEmu))
                {
                    parentEmu = GetEmulator(emuId);
                    emuDict.Add(emuId, parentEmu);
                }

                Game game = Game.CreateGame(row, parentEmu);
                if (game != null)
                    games.Add(game);
                else if (showOrphans)
                    games.Add(new Game(Decode(row.fields[1]), null) { GameID = int.Parse(row.fields[0], System.Globalization.CultureInfo.InvariantCulture) });
            }

            return games;
        }

        public Dictionary<int, Game> GetGames(List<int> ids)
        {
            Dictionary<int, Game> idDict = new Dictionary<int, Game>();
            if (ids.Count < 1)
                return idDict;

            List<int> lIds = new List<int>();
            string idString = "";
            for (int x = 0; x < ids.Count; x++)
            {
                int id = ids[x];
                if (x == 0)
                {
                    idString += id;
                    lIds.Add(id);
                }
                else if (!lIds.Contains(id))
                {
                    idString += ",";
                    idString += id;
                    lIds.Add(id);
                }
            }
            string sql = string.Format("WHERE gameid IN ({0})", idString);
            foreach (Game game in GetGames(sql, false))
            {
                idDict[game.GameID] = game;
            }

            return idDict;
        }

        public List<string> GetGenres()
        {
            List<string> genres = new List<string>();
            SQLiteResultSet results = DB.Instance.Execute("SELECT DISTINCT genre FROM {0}", Game.TABLE_NAME);
            foreach (SQLiteResultSet.Row row in results.Rows)
            {
                foreach (string s in row.fields[0].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!genres.Contains(s, StringComparer.CurrentCultureIgnoreCase))
                        genres.Add(s);
                }
            }
            if (genres.Count > 0)
                genres.Sort();
            return genres;
        }

        #endregion

        #region EmulatorProfiles

        public EmulatorProfile GetProfile(int id, Emulator parentEmu)
        {
            if (parentEmu.UID < 0)
                return new EmulatorProfile(true) { EmulatorID = -1 };
            SQLiteResultSet result;
            if (id > -1)
            {
                result = Execute("SELECT * FROM {0} WHERE uid={1} AND emulator_id={2}", EmulatorProfile.TABLE_NAME, id, parentEmu.UID);
                if (result.Rows.Count > 0)
                {
                    return EmulatorProfile.CreateProfile(result.Rows[0]);
                }
            }
            else
            {
                result = Execute("SELECT * FROM {0} WHERE emulator_id={1} AND defaultprofile='True'", EmulatorProfile.TABLE_NAME, parentEmu.UID);
                if (result.Rows.Count > 0)
                {
                    return EmulatorProfile.CreateProfile(result.Rows[0]);
                }
            }

            List<EmulatorProfile> profiles = GetProfiles(parentEmu);
            if (profiles.Count > 0)
                return profiles[0];

            Logger.LogError("No profiles found for {0}, database corrupt", parentEmu.Title);
            return null;
        }

        public EmulatorProfile GetProfile(Game game)
        {
            if (!game.ParentEmulator.IsPc())
                return GetProfile(game.SelectedProfileId, game.ParentEmulator);

            SQLiteResultSet result = Execute("SELECT * FROM {0} WHERE uid={1} AND game_id={2}", EmulatorProfile.TABLE_NAME, game.SelectedProfileId, game.GameID);
            if (result.Rows.Count > 0)
                return EmulatorProfile.CreateProfile(result.Rows[0]);

            List<EmulatorProfile> profiles = GetProfiles(game);
            if (profiles.Count > 0)
                return profiles[0];

            Logger.LogError("No profiles found for {0}, database corrupt", game.Title);
            return null;
        }

        public List<EmulatorProfile> GetProfiles(Emulator emu)
        {
            List<EmulatorProfile> profiles = new List<EmulatorProfile>();
            if (emu.UID == -1)
            {
                profiles.Add(new EmulatorProfile(true) { EmulatorID = -1 });
                return profiles;
            }

            SQLiteResultSet result = Execute("SELECT * FROM {0} WHERE emulator_id={1} ORDER BY defaultprofile DESC", EmulatorProfile.TABLE_NAME, emu.UID);
            foreach (SQLiteResultSet.Row row in result.Rows)
                profiles.Add(EmulatorProfile.CreateProfile(row));
            return profiles;
        }

        public List<EmulatorProfile> GetProfiles(Game game)
        {
            if (!game.ParentEmulator.IsPc())
                return GetProfiles(game.ParentEmulator);

            List<EmulatorProfile> profiles = new List<EmulatorProfile>();
            SQLiteResultSet result = Execute("SELECT * FROM {0} WHERE emulator_id=-1 AND game_id={1} ORDER BY defaultprofile DESC", EmulatorProfile.TABLE_NAME, game.GameID);
            if (result.Rows.Count > 0)
                foreach (SQLiteResultSet.Row row in result.Rows)
                    profiles.Add(EmulatorProfile.CreateProfile(row));
            else
                profiles.Add(new EmulatorProfile(true) { EmulatorID = -1, GameId = game.GameID, Arguments = game.Arguments, SuspendMP = true, UseQuotes = false, StopEmulationOnKey = false });

            return profiles;
        }

        #endregion

        #region Utils

        public static string Encode(String input)
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                String encoded = input;
                encoded = encoded.Replace("'", "&#39;");
                encoded = encoded.Replace("\"", "&#34;");
                encoded = encoded.Replace("{", "&#123;");
                encoded = encoded.Replace("}", "&#125;");
                return encoded;
            }
        }

        public static string Decode(String input)
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                String decoded = input;
                decoded = decoded.Replace("&#39;", "'");
                decoded = decoded.Replace("&#34;", "\"");
                decoded = decoded.Replace("&#123;", "{");
                decoded = decoded.Replace("&#125;", "}");
                return decoded;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (sqlClient != null)
            {
                sqlClient.Close();
                sqlClient.Dispose();
                sqlClient = null;
            }
        }

        #endregion
    }
}
