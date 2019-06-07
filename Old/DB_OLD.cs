using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite.NET;
using MediaPortal.Configuration;
using MediaPortal.Database;
using MediaPortal.GUI.Library;

namespace myEmulators
{
    public static class DB
    {
        private static SQLiteClient sqlDB;
        static volatile bool isDBInit = false;

        static object dbLock = new object();

        static SQLiteResultSet dbExecute(string query)
        {
            if (!isDBInit)
            {
                Logger.LogDebug("Attempting to access database before initialisation complete");
                return new SQLiteResultSet();
            }
            SQLiteResultSet result;
            lock (dbLock)
            {
                if (sqlDB == null)
                    return new SQLiteResultSet();

                result = sqlDB.Execute(query);
            }
            return result;
        }

        public static void init()
        {
            if (isDBInit)
            {
               Logger.LogDebug("Database already initialised");
               return;
            }

            String dbFile = Config.GetFile(Config.Dir.Database, "myEmulators_v2.db3");

            lock (dbLock) //don't allow any DB calls until init completes
            {
                sqlDB = new SQLiteClient(dbFile);

                DatabaseUtility.AddTable(sqlDB, "Emulators",
                    "CREATE TABLE Emulators(" +
                        "uid int," +
                        "emulator_path varchar(200)," +
                        "position int," +
                        "rom_path varchar(200)," +
                        "title varchar(50)," +
                        "filter varchar(100)," +
                        "working_path varchar(200)," +
                        "usequotes char(5)," +
                        "view int," +
                        "args varchar(200)," +
                        "suspend_mp char(5)," +
                        "enable_goodmerge char(5)," +
                        "goodmerge_pref1 varchar(10)," +
                        "goodmerge_pref2 varchar(10)," +
                        "goodmerge_pref3 varchar(10)," +
                        "goodmerge_temp_path varchar(200)," +
                        "Grade int," +
                        "Company varchar(200)," +
                        "Yearmade int," +
                        "Description varchar(2000)," +
                        "mountimages char(5)," +
                        "PRIMARY KEY(uid)" +
                    ")"
                );

                DatabaseUtility.AddTable(sqlDB, "EmulatorProfiles",
                    "CREATE TABLE EmulatorProfiles(" +
                    "uid int," +
                    "title varchar(200)," +
                    "emulator_id int," +
                    "emulator_path varchar(200)," +
                    "working_path varchar(200)," +
                    "usequotes char(5)," +
                    "args varchar(200)," +
                    "suspend_mp char(5)," +
                    "goodmerge_pref1 varchar(10)," +
                    "goodmerge_pref2 varchar(10)," +
                    "goodmerge_pref3 varchar(10)," +
                    "mountimages char(5)," +
                    "PRIMARY KEY(uid)" +
                    ")"
                );

                DatabaseUtility.AddTable(sqlDB, "Games",
                    @"CREATE TABLE Games(
                    gameid INTEGER PRIMARY KEY AUTOINCREMENT, 
                    path varchar(200),  
                    parentemu int,
                    title varchar(100),
                    grade int,
                    playcount int,
                    yearmade int,
                    latestplay varchar(16),
                    description varchar(200),
                    genre varchar(50),
                    company varchar(50),
                    visible char(5),
                    favourite char(5),
                    LaunchFile varchar(200),
                    emuprofile int,
                    infochecked char(5)
                    )"
                );

                DatabaseUtility.AddTable(sqlDB, "Info",
                    "CREATE TABLE Info(" +
                        "name varchar(50)," +
                        "value varchar(100)," +
                        "PRIMARY KEY(name)" +
                    ")"
                );

                //Check version and upgrade if neccessary
                SQLiteResultSet result = sqlDB.Execute("SELECT value FROM Info WHERE name='version'");
                if (result.Rows.Count == 0)
                {
                    //Fresh install
                    sqlDB.Execute("INSERT INTO Info VALUES('version'," + Options.getVersionNumber() + ")");
                    //Update result to new version number
                    result = sqlDB.Execute("SELECT value FROM Info WHERE name='version'");
                }
                //Upgrade to 3.2

                if (result.GetRow(0).fields[0].CompareTo("3.2") < 0)
                {
                    //Add the suspend_mp column to the emulators table
                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN suspend_mp char(5)");
                    }
                    catch { }

                    //Add default value
                    sqlDB.Execute("UPDATE Emulators SET suspend_mp='False'");
                }

                // add goodmerge support
                if (result.GetRow(0).fields[0].CompareTo("3.4") < 0)
                {
                    //Add the suspend_mp column to the emulators table
                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN enable_goodmerge char(5)");
                    }
                    catch { }

                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN goodmerge_pref1 varchar(10)");
                    }
                    catch { }

                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN goodmerge_pref2 varchar(10)");
                    }
                    catch { }

                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN goodmerge_pref3 varchar(10)");
                    }
                    catch { }

                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN goodmerge_temp_path varchar(200)");
                    }
                    catch { }

                    //Add default value
                    sqlDB.Execute("UPDATE Emulators SET enable_goodmerge='False'");

                    try
                    {
                        sqlDB.Execute("ALTER TABLE Games ADD COLUMN LaunchFile varchar(200)");
                    }
                    catch { }

                }

                if (result.GetRow(0).fields[0].CompareTo("3.5") < 0)
                {
                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN Company varchar(200)");
                    }
                    catch { }

                    try
                    {

                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN Description varchar(2000)");
                    }
                    catch { }

                    try
                    {

                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN Grade int");
                    }
                    catch { }

                    try
                    {

                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN Yearmade int");
                    }
                    catch { }

                    sqlDB.Execute("UPDATE Emulators SET Grade=0");
                }

                if (result.GetRow(0).fields[0].CompareTo("4.0") < 0)
                {
                    try
                    {
                        sqlDB.Execute("ALTER TABLE Games ADD COLUMN emuprofile int");
                    }
                    catch { }

                    sqlDB.Execute("UPDATE Games SET emuprofile=-1");
                }

                if (result.GetRow(0).fields[0].CompareTo("4.1") < 0)
                {

                    try
                    {
                        sqlDB.Execute("ALTER TABLE Games ADD COLUMN infochecked char(5)");
                    }
                    catch { }
                    
                    sqlDB.Execute("UPDATE Games SET infochecked = 'False'");
                }

                if (result.GetRow(0).fields[0].CompareTo("4.2") < 0)
                {
                    try
                    {
                        sqlDB.Execute("ALTER TABLE Emulators ADD COLUMN mountimages char(5)");
                    }
                    catch { }

                    sqlDB.Execute("UPDATE Emulators SET mountimages = 'False'");

                    try
                    {
                        sqlDB.Execute("ALTER TABLE EmulatorProfiles ADD COLUMN mountimages char(5)");
                    }
                    catch { }

                    sqlDB.Execute("UPDATE EmulatorProfiles SET mountimages = 'False'");

                    //Recreate games table
                    sqlDB.Execute("CREATE TABLE Gamestemp(gameid int,path varchar(200),parentemu int,title varchar(100),grade int,playcount int,yearmade int,latestplay varchar(16),description varchar(200),genre varchar(50),company varchar(50),visible char(5),favourite char(5),LaunchFile varchar(200),emuprofile int, infochecked char(5))");
                    sqlDB.Execute("INSERT INTO Gamestemp SELECT * FROM games;");
                    sqlDB.Execute("DROP TABLE games;");
                    sqlDB.Execute("CREATE TABLE Games(gameid INTEGER PRIMARY KEY AUTOINCREMENT, path varchar(200),parentemu int,title varchar(100),grade int,playcount int,yearmade int,latestplay varchar(16),description varchar(200),genre varchar(50),company varchar(50),visible char(5),favourite char(5),LaunchFile varchar(200),emuprofile int, infochecked char(5))");
                    sqlDB.Execute("INSERT INTO games SELECT NULL,path,parentemu,title,grade,playcount,yearmade,latestplay,description,genre,company,visible,favourite,LaunchFile,emuprofile,infochecked FROM gamestemp;");
                    sqlDB.Execute("DROP Table Gamestemp");
                }

                //To always be in sync with the version number
                sqlDB.Execute("UPDATE Info SET value='" + Options.getVersionNumber() + "' WHERE name='version'");

                //remove pc emulator if left over from previous version
                sqlDB.Execute("DELETE FROM Emulators WHERE uid=-1");

                /*
                //Bundle installation
                //TODO: gör bundle-mpi
                result = sqlDB.Execute("SELECT uid FROM Emulators WHERE emulator_path LIKE '%--EMU_BASE_PATH--%'");
                if (result.Rows.Count > 0)
                {
                    for (int i = 0; i < result.Rows.Count; i++)
                    {
                        Emulator item = getEmulator(DatabaseUtility.GetAsInt(result, i, 0));
                        item.PathToEmulator = item.PathToEmulator.Replace("--EMU_BASE_PATH--", Config.GetFolder(Config.Dir.Base) + @"\Emulators");
                        item.PathToRoms = item.PathToRoms.Replace("--GAME_BASE_PATH--", item.PathToEmulator.Remove(item.PathToEmulator.LastIndexOf('\\')) + @"\Place your games here");
                        saveEmulator(item);
                    }
                }
                */
            }
            isDBInit = true;
        }

        //Does not include PC as this method is called when determining rom directories
        //and filters, which the PC emulator does not have.
        public static Emulator[] getEmulators()
        {
            List<Emulator> items = new List<Emulator>();
            SQLiteResultSet result = dbExecute("SELECT uid FROM Emulators ORDER BY position ASC");
            for (int i = 0; i < result.Rows.Count; i++)
            {
                items.Add(getEmulator(DatabaseUtility.GetAsInt(result, i, 0)));
            }
            return items.ToArray();
        }

        //Method used to display all emulators including PC.
        public static Emulator[] getEmulatorsAndPC()
        {
            List<Emulator> items = new List<Emulator>();
            items.AddRange(getEmulators());

            if (getPCGames().Length > 0) //only include PC if we have PC games
            {
                Emulator pc = getPCDetails();
                //sanitise position just in case
                if (pc.Position < 0)
                    pc.Position = 0;
                else if (pc.Position > items.Count)
                    pc.Position = items.Count;
                items.Insert(pc.Position, pc); //Insert PC into correct position in list
            }

            return items.ToArray();
        }

        public static Emulator getEmulator(int uid)
        {
            if(uid == -1)
                return getPCDetails();

            SQLiteResultSet result = dbExecute("SELECT * FROM Emulators WHERE uid=" + uid);
            if (result.Rows.Count > 0)
            {
                Emulator item = new Emulator();
                item.UID = DatabaseUtility.GetAsInt(result, 0, 0);
                item.PathToEmulator = decode(DatabaseUtility.Get(result, 0, 1));
                item.Position = DatabaseUtility.GetAsInt(result, 0, 2);
                item.PathToRoms = decode(DatabaseUtility.Get(result, 0, 3));
                item.Title = decode(DatabaseUtility.Get(result, 0, 4));
                item.Filter = DatabaseUtility.Get(result, 0, 5);
                item.WorkingFolder = decode(DatabaseUtility.Get(result, 0, 6));
                item.UseQuotes = Boolean.Parse(DatabaseUtility.Get(result, 0, 7));
                item.View = DatabaseUtility.GetAsInt(result, 0, 8);
                item.Arguments = decode(DatabaseUtility.Get(result, 0, 9));
                item.SuspendRendering = Boolean.Parse(DatabaseUtility.Get(result, 0, 10));
                item.EnableGoodmerge = Boolean.Parse(DatabaseUtility.Get(result, 0, 11));
                item.GoodmergePref1 = decode(DatabaseUtility.Get(result, 0, 12));
                item.GoodmergePref2 = decode(DatabaseUtility.Get(result, 0, 13));
                item.GoodmergePref3 = decode(DatabaseUtility.Get(result, 0, 14));
                item.GoodmergeTempPath = decode(DatabaseUtility.Get(result, 0, 15));
                item.Grade = DatabaseUtility.GetAsInt(result, 0, "Grade");
                item.Company = decode(DatabaseUtility.Get(result, 0, "Company"));
                item.Yearmade = DatabaseUtility.GetAsInt(result, 0, "Yearmade");
                item.Description = decode(DatabaseUtility.Get(result, 0, "Description"));
                item.MountImages = Boolean.Parse(DatabaseUtility.Get(result, 0, "mountimages"));
                return item;
            }
            else
                return null;
        }

        public static int saveEmulator(Emulator item)
        {
            if (item.isPc())
            {
                savePCDetails(item);
                return -1;
            }
            //SQLiteResultSet result = sqlDB.Execute("SELECT COUNT(*) FROM Emulators WHERE uid=" + item.UID);
            //if (DatabaseUtility.GetAsInt(result, 0, 0) > 0)
            SQLiteResultSet result = dbExecute("SELECT Title FROM Emulators WHERE uid=" + item.UID);
            if(result.Rows.Count > 0)
            {
                bool updateThumbs = false;
                string oldTitle = "";
                if (ThumbsHandler.Instance.NeedThumbUpdate)
                {
                    oldTitle = DatabaseUtility.Get(result, 0, 0);
                    if (oldTitle != item.Title)
                        updateThumbs = true;
                }

                //Update existing emulator
                dbExecute(
                    "UPDATE Emulators SET " +
                        "emulator_path='" + encode(item.PathToEmulator) + "', " +
                        "position=" + item.Position + ", " +
                        "rom_path='" + encode(item.PathToRoms) + "', " +
                        "title='" + encode(item.Title) + "', " +
                        "filter='" + item.Filter + "', " +
                        "working_path='" + encode(item.WorkingFolder) + "', " +
                        "usequotes='" + item.UseQuotes + "', " +
                        "view=" + item.View + ", " +
                        "args='" + encode(item.Arguments) + "', " +
                        "suspend_mp='" + item.SuspendRendering + "', " +
                        "enable_goodmerge='" + item.EnableGoodmerge + "', " +
                        "goodmerge_pref1='" + item.GoodmergePref1 + "', " +
                        "goodmerge_pref2='" + item.GoodmergePref2 + "', " +
                        "goodmerge_pref3='" + item.GoodmergePref3 + "', " +
                        "goodmerge_temp_path='" + encode(item.GoodmergeTempPath) + "'," +
                        "Grade=" + item.Grade + ", " +
                        "Company='" + encode(item.Company) + "'," +
                        "Yearmade=" + item.Yearmade + ", " +
                        "Description='" + encode(item.Description) + "', " +
                        "mountimages='" + item.MountImages + "'" +

                    " WHERE uid=" + item.UID
                );
                if (updateThumbs)
                    ThumbsHandler.Instance.updateThumbs(item, oldTitle);
                return item.UID;
            }
            else
            {
                //New emulator

                int uid = getNewEmuUIDandInsert(item);

                //Create thumbnail folder if doesn't exist
                String folderPath = ThumbsHandler.Instance.thumb_games + @"\" + item.Title;
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                return uid;
            }
        }

        static int getNewEmuUIDandInsert(Emulator item)
        {
            if (!isDBInit)
                return item.UID;

            //get uid and insert within 1 lock to ensure thread safe 
            //(otherwise new Emulator could be added between checking
            //and inserting)
            lock (dbLock)  
            {
                if (item.UID < 1)
                {
                    SQLiteResultSet result = sqlDB.Execute("SELECT MAX(uid)+1 FROM Emulators");
                    if (result.Rows.Count > 0)
                    {
                        item.UID = DatabaseUtility.GetAsInt(result, 0, 0);
                    }
                    else
                    {
                        item.UID = 0;
                    }
                }

                sqlDB.Execute(
                    "INSERT INTO Emulators VALUES(" +
                        item.UID + "," +
                        "'" + encode(item.PathToEmulator) + "'," +
                        +item.Position + "," +
                        "'" + encode(item.PathToRoms) + "'," +
                        "'" + encode(item.Title) + "'," +
                        "'" + item.Filter + "'," +
                        "'" + encode(item.WorkingFolder) + "'," +
                        "'" + item.UseQuotes + "'," +
                        item.View + "," +
                        "'" + encode(item.Arguments) + "'," +
                        "'" + item.SuspendRendering + "'," +
                        "'" + item.EnableGoodmerge + "'," +
                        "'" + item.GoodmergePref1 + "'," +
                        "'" + item.GoodmergePref2 + "'," +
                        "'" + item.GoodmergePref3 + "'," +
                        "'" + encode(item.GoodmergeTempPath) + "'," +
                        item.Grade + "," +
                        "'" + encode(item.Company) + "'," +
                        item.Yearmade + "," +
                        "'" + encode(item.Description) + "'," +
                        "'" + item.MountImages + "'" +
                    ")"
                );

            }

            return item.UID;
        }

        /*
        private static int getNewEmulatorUID()
        {
            SQLiteResultSet result = dbExecute("SELECT MAX(uid)+1 FROM Emulators");
            if (result.Rows.Count > 0)
            {
                return DatabaseUtility.GetAsInt(result, 0, 0);
            }
            else
            {
                return 0;
            }
        }
        */
         
        public static void deleteEmulators()
        {
            dbExecute("DELETE FROM Emulators");
        }

        public static void deleteEmulator(Emulator item)
        {
            if (item.isPc())
                return;

            if (!isDBInit)
                return;

            lock (dbLock)
            {
                sqlDB.Execute("DELETE FROM Emulators WHERE uid=" + item.UID);
                //remove any associated profiles
                sqlDB.Execute("DELETE FROM EmulatorProfiles WHERE emulator_id=" + item.UID);
            }
        }

        public static void deleteGames()
        {
            dbExecute("DELETE FROM Games");
        }

        public static void deleteGame(Game item)
        {
            dbExecute("DELETE FROM Games WHERE path='" + encode(item.Path) + "'");
        }

        public static Game[] getGames(String path, Emulator emulator)
        {
            return getGames(path, emulator, SearchOption.TopDirectoryOnly, false);
        }

        public static Game[] getGames(String path, Emulator emulator, SearchOption recursive, bool showHidden)
        {
            List<Game> items = new List<Game>();

            SQLiteResultSet result = dbExecute("SELECT * FROM Games WHERE parentemu = " + emulator.UID.ToString());
            List<string> dbPaths = new List<string>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                dbPaths.Add(DatabaseUtility.Get(result, i, 1)); //store list of paths already in DB
            }

            if (!Directory.Exists(path)) //Rom directory doesn't exist, skip
            {
                Logger.LogError("Error locating Rom directory for '{0}'", emulator.Title);
                return items.ToArray();
            }

            foreach (String filter in emulator.Filter.Split(';')) //search directory against each filter
            {

                string[] gamePaths;
                try
                {
                    gamePaths = Directory.GetFiles(path, filter, recursive); //get list of matches
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error getting files from '{0}' rom directory - {1}", emulator, ex.Message);
                    continue;
                }
                foreach (String gamePath in gamePaths)
                {
                    Game item;
                    if (dbPaths.Contains(gamePath)) //already in database
                        item = createGame(result, dbPaths.IndexOf(gamePath), emulator);
                    else
                        item = getGame(gamePath, emulator); //add new item to database

                    if (item.Visible || showHidden)
                    {
                        items.Add(item);
                    }
                }
            }

            String[] keys = new String[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                keys[i] = items[i].Title; //Sort items by title
            }
            Game[] listToReturn = items.ToArray();
            Array.Sort(keys, listToReturn);
            return listToReturn;
        }

        public static Game[] getGamesFromDB(Emulator emulator)
        {
            return getGamesFromDB(emulator, false);
        }

        public static Game[] getGamesFromDB(Emulator emulator, bool showHidden)
        {
            List<Game> items = new List<Game>();

            SQLiteResultSet result = dbExecute("SELECT * FROM Games WHERE parentemu = " + emulator.UID.ToString() + @" ORDER BY parentemu, title");

            for (int i = 0; i < result.Rows.Count; i++)
            {
                //Game item = getGame(DatabaseUtility.Get(result, i, 0), emulator);
                Game item = createGame(result, i, emulator);
                if (item.Visible || showHidden)
                {
                    items.Add(item);
                }
            }

            String[] keys = new String[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                keys[i] = items[i].Title;
            }
            Game[] listToReturn = items.ToArray();
            Array.Sort(keys, listToReturn);
            return listToReturn;
        }

        public static Game[] getAllGames()
        {
            List<Game> items = new List<Game>();

            foreach (Emulator emu in getEmulators())
            {
                SQLiteResultSet result = dbExecute("SELECT * FROM Games WHERE parentemu = " + emu.UID.ToString() + @" ORDER BY parentemu, title");

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    //Game item = getGame(DatabaseUtility.Get(result, i, 0), emu);
                    Game item = createGame(result, i, emu);
                    items.Add(item);
                }
            }
            items.AddRange(getPCGames());

            return items.ToArray();
        }

        public static void refreshGamesDatabase()
        {
            //Find all game files on the file system and load it into the database

            foreach (Emulator emu in getEmulators())
            {
                getGames(emu.PathToRoms, emu, SearchOption.AllDirectories, true);
            }
        }

        delegate void RefreshDelegate();

        public static List<string> GetAllPaths()
        {
            SQLiteResultSet result = dbExecute("SELECT path FROM Games");
            List<string> dbPaths = new List<string>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                dbPaths.Add(decode(DatabaseUtility.Get(result, i, 0))); //store list of paths already in DB
            }
            return dbPaths;
        }

        public static void cleanGameDatabase()
        {
            foreach (Game game in getAllGames())
            {
                if (!File.Exists(decode(game.Path))) //rom doesn't exist
                    deleteGame(game);
                else if (game.ParentEmulator.UID > -1)
                {
                    bool remove = true;
                    if (game.Path.StartsWith(game.ParentEmulator.PathToRoms)) //is rom in rom directory?
                        foreach (string extension in game.ParentEmulator.Filter.Split(';')) 
                            if (extension.EndsWith(Path.GetExtension(game.Path)) || extension == "*.*") //does rom end with valid extension?
                            {
                                remove = false;
                                break;
                            }
                    if (remove)
                        deleteGame(game); //rom isn't in rom directory or doesn't have a valid extension
                }
            }
        }

        public static Game[] getSomeGames(String sqlTail)
        {

            //Do DB lookup
            List<Game> items = new List<Game>();
            //SQLiteResultSet result = sqlDB.Execute("SELECT path, parentemu FROM Games " + sqlTail);
            SQLiteResultSet result = dbExecute("SELECT * FROM Games " + sqlTail);
            if (result.Rows.Count > 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    //String path = DatabaseUtility.Get(result, i, 0);
                    //Emulator parentEmu = getEmulator(DatabaseUtility.GetAsInt(result, i, 1));
                    //Game item = getGame(path, parentEmu);
                    Game item = createGame(result, i, null);
                    if (item.Visible)
                    {
                        items.Add(item);
                    }
                }
            }
            return items.ToArray();
        }

        public static Game[] getPCGames()
        {
            return getSomeGames("WHERE parentemu = -1 ORDER BY title ASC");
        }

        static Game createGame(SQLiteResultSet result, int rowIndex, Emulator parentEmu)
        {
            Game item = new Game(decode(DatabaseUtility.Get(result, rowIndex, 1)), parentEmu);
            if(parentEmu == null)
                item.ParentEmulator = getEmulator(DatabaseUtility.GetAsInt(result, rowIndex, 2));
            item.GameID = DatabaseUtility.GetAsInt(result, rowIndex, 0);
            item.Title = decode(DatabaseUtility.Get(result, rowIndex, 3));
            item.Grade = DatabaseUtility.GetAsInt(result, rowIndex, 4);
            item.Playcount = DatabaseUtility.GetAsInt(result, rowIndex, 5);
            item.Yearmade = DatabaseUtility.GetAsInt(result, rowIndex, 6);
            item.Latestplay = DateTime.Parse(DatabaseUtility.Get(result, rowIndex, 7));
            item.Description = decode(DatabaseUtility.Get(result, rowIndex, 8));
            item.Genre = decode(DatabaseUtility.Get(result, rowIndex, 9));
            item.Company = decode(DatabaseUtility.Get(result, rowIndex, 10));
            item.Visible = Boolean.Parse(DatabaseUtility.Get(result, rowIndex, 11));
            item.Favourite = Boolean.Parse(DatabaseUtility.Get(result, rowIndex, 12));
            item.LaunchFile = decode(DatabaseUtility.Get(result, rowIndex, 13));
            item.SelectedProfile = DatabaseUtility.GetAsInt(result, rowIndex, 14);
            item.IsInfoChecked = Boolean.Parse(DatabaseUtility.Get(result, rowIndex, 15));
            return item;
        }

        public static Game getGame(int gameID)
        {
            Game[] games = getSomeGames("WHERE gameid=" + gameID.ToString());
            if (games.Length > 0)
                return games[0];
            else
                return null;
        }

        public static Game getGame(String path, Emulator parentEmulator)
        {
            Game item = new Game(decode(path), parentEmulator);

            SQLiteResultSet result;
            lock (dbLock)
            {
                result = sqlDB.Execute("SELECT * FROM Games WHERE path='" + encode(path) + "'");
                if (result.Rows.Count == 0)
                {
                    SQLiteResultSet idresult = sqlDB.Execute("SELECT MAX(gameid)+1 FROM Games");
                    if (idresult.Rows.Count > 0)
                    {
                        item.GameID = DatabaseUtility.GetAsInt(idresult, 0, 0);
                    }
                    else
                    {
                        item.GameID = 0;
                    }
                    //Insert new game into DB
                    sqlDB.Execute(
                        "INSERT INTO Games VALUES(" +
                            "NULL, " +
                            "'" + encode(item.Path) + "', " +
                            parentEmulator.UID + ", " +
                            "'" + encode(item.Title) + "', " +
                            "0, " +
                            "0, " +
                            "0, " +
                            "'" + DateTime.MinValue + "', " +
                            "'', " +
                            "'', " +
                            "'', " +
                            "'True'," +
                            "'False'," +
                            "'' ," +
                            item.SelectedProfile + ", " +
                            "'False'" +
                        ")"
                    );

                    return item;
                }
            }

            if (result.Rows.Count > 0)
            {
                //Lookup from DB
                item.GameID = DatabaseUtility.GetAsInt(result, 0, 0);
                item.Title = decode(DatabaseUtility.Get(result, 0, 3));
                item.Grade = DatabaseUtility.GetAsInt(result, 0, 4);
                item.Playcount = DatabaseUtility.GetAsInt(result, 0, 5);
                item.Yearmade = DatabaseUtility.GetAsInt(result, 0, 6);
                item.Latestplay = DateTime.Parse(DatabaseUtility.Get(result, 0, 7));
                item.Description = decode(DatabaseUtility.Get(result, 0, 8));
                item.Genre = decode(DatabaseUtility.Get(result, 0, 9));
                item.Company = decode(DatabaseUtility.Get(result, 0, 10));
                item.Visible = Boolean.Parse(DatabaseUtility.Get(result, 0, 11));
                item.Favourite = Boolean.Parse(DatabaseUtility.Get(result, 0, 12));
                item.LaunchFile = decode(DatabaseUtility.Get(result, 0, 13));
                item.SelectedProfile = DatabaseUtility.GetAsInt(result, 0, 14);
                item.IsInfoChecked = Boolean.Parse(DatabaseUtility.Get(result, 0, 15));
            }

            return item;
        }

        public static void saveGame(Game item)
        {
            dbExecute("UPDATE Games SET parentemu=" + item.ParentEmulator.UID + ", title='" + encode(item.Title) + "', grade=" + item.Grade + ", playcount=" + item.Playcount + ", yearmade=" + item.Yearmade + ", latestplay='" + item.Latestplay + "', description='" + encode(item.Description) + "', genre='" + encode(item.Genre) + "', company='" + encode(item.Company) + "', visible='" + item.Visible + "', favourite='" + item.Favourite + "', LaunchFile='" + encode(item.LaunchFile) + "', emuprofile=" + item.SelectedProfile + ", infochecked='" + item.IsInfoChecked + "' WHERE path='" + encode(item.Path) + "'");
        }

        //Additional save method added to avoid game title being changed when saving goodmerge games
        public static void saveGamePlayInfo(Game item)
        {
            dbExecute("UPDATE Games SET grade=" + item.Grade + ", playcount=" + item.Playcount + ", latestplay='" + item.Latestplay + "', favourite='" + item.Favourite + "', LaunchFile='" + encode(item.LaunchFile) + "' WHERE path='" + encode(item.Path) + "'");
        }

        public static void savePCGame(Game item)
        {
            SQLiteResultSet result = dbExecute("SELECT * FROM Games WHERE path='" + encode(item.Path) + "'");
            if (result.Rows.Count == 0)
            {
                //No possibility to edit, so only insert if is a new game
                Game insertedGame = getGame(item.Path, new Emulator(EmulatorType.pcgame));
                insertedGame.Title = item.Title;
                saveGame(insertedGame);
            }
        }

        private static String encode(String input)
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
                return encoded;
            }
        }

        private static String decode(String input)
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
                return decoded;
            }
        }

        //Special methods used for storing PC details.
        //PC details are stored in myEmulators.xml not
        //the DB.
        delegate Emulator getPCDel();

        public static Emulator getPCDetails()
        {
            if (GUIGraphicsContext.form.InvokeRequired)
                return GUIGraphicsContext.form.Invoke(new getPCDel(getPCDetails) ) as Emulator;

            Emulator emu = new Emulator(EmulatorType.pcgame);

            string title = Options.Instance.GetStringOption("pcitemtitle");
            if (title != "")
                emu.Title = title;

            emu.Description = Options.Instance.GetStringOption("pcitemdescription");
            emu.Position = Options.Instance.GetIntOption("pcitemposition");

            return emu;
        }

        public static void savePCDetails(Emulator emu)
        {
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                GUIGraphicsContext.form.Invoke(new System.Windows.Forms.MethodInvoker(delegate { savePCDetails(emu); }));
                return;
            }

            if (emu != null)
            {
                bool updateThumbs = false;
                string oldTitle = Options.Instance.GetStringOption("pcitemtitle");
                if (ThumbsHandler.Instance.NeedThumbUpdate)
                {
                    if (oldTitle != "" && oldTitle != emu.Title)
                        updateThumbs = true;
                }
                Options.Instance.UpdateOption("pcitemtitle", emu.Title);
                Options.Instance.UpdateOption("pcitemdescription", emu.Description);
                Options.Instance.UpdateOption("pcitemposition", emu.Position);

                if (updateThumbs)
                    ThumbsHandler.Instance.updateThumbs(emu, oldTitle);
            }
        }

        public static EmulatorProfile[] GetProfiles(Emulator emu)
        {
            List<EmulatorProfile> profiles = new List<EmulatorProfile>();

            emu = getEmulator(emu.UID); //get latest settings from DB when creating default profile
            //add default profile
            profiles.Add(new EmulatorProfile(emu, false));

            SQLiteResultSet result = dbExecute("SELECT * FROM EmulatorProfiles WHERE emulator_id=" + emu.UID);
            if (result.Rows.Count > 0)
            {
                for (int x = 0; x < result.Rows.Count; x++)
                    profiles.Add(createProfile(result, x));
            }

            return profiles.ToArray();
        }

        public static EmulatorProfile GetSelectedProfile(Game game)
        {
            Emulator emu = getEmulator(game.ParentEmulator.UID); //get latest settings from DB when creating default profile
            if (game.SelectedProfile == -1)
                return new EmulatorProfile(emu, false);

            SQLiteResultSet result = dbExecute(string.Format("SELECT * FROM EmulatorProfiles WHERE uid={0} AND emulator_id={1}", game.SelectedProfile, emu.UID));
            if (result.Rows.Count == 0)
                return new EmulatorProfile(emu, false);

            return createProfile(result, 0);
        }

        static EmulatorProfile createProfile(SQLiteResultSet result, int rowNum)
        {
            EmulatorProfile profile = new EmulatorProfile(null, false);
            profile.ID = DatabaseUtility.GetAsInt(result, rowNum, 0);
            profile.Title = decode(DatabaseUtility.Get(result, rowNum, 1));
            profile.EmulatorID = DatabaseUtility.GetAsInt(result, rowNum, 2);
            profile.EmulatorPath = decode(DatabaseUtility.Get(result, rowNum, 3));
            profile.WorkingDirectory = decode(DatabaseUtility.Get(result, rowNum, 4));
            profile.UseQuotes = Boolean.Parse(DatabaseUtility.Get(result, rowNum, 5));
            profile.Arguments = decode(DatabaseUtility.Get(result, rowNum, 6));
            profile.SuspendMP = Boolean.Parse(DatabaseUtility.Get(result, rowNum, 7));
            profile.GoodMergePref1 = decode(DatabaseUtility.Get(result, rowNum, 8));
            profile.GoodMergePref2 = decode(DatabaseUtility.Get(result, rowNum, 9));
            profile.GoodMergePref3 = decode(DatabaseUtility.Get(result, rowNum, 10));
            profile.MountImages = Boolean.Parse(DatabaseUtility.Get(result, rowNum, 11));
            return profile;
        }

        public static void DeleteProfile(EmulatorProfile emulatorProfile)
        {
            dbExecute("DELETE FROM EmulatorProfiles WHERE uid=" + emulatorProfile.ID);
        }

        public static int SaveProfile(EmulatorProfile emulatorProfile)
        {
            if (emulatorProfile.ID == -1) //Defult profile, update Emulator
            {
                Emulator emu = getEmulator(emulatorProfile.EmulatorID);
                if (emu == null)
                {
                    Logger.LogError("Unable to save profile - Parent Emulator does not exist");
                    return -1;
                }

                emu.PathToEmulator = emulatorProfile.EmulatorPath;
                emu.Arguments = emulatorProfile.Arguments;
                emu.WorkingFolder = emulatorProfile.WorkingDirectory;
                emu.UseQuotes = emulatorProfile.UseQuotes == true;
                emu.SuspendRendering = emulatorProfile.SuspendMP == true;
                emu.GoodmergePref1 = emulatorProfile.GoodMergePref1;
                emu.GoodmergePref2 = emulatorProfile.GoodMergePref2;
                emu.GoodmergePref3 = emulatorProfile.GoodMergePref3;
                emu.MountImages = emulatorProfile.MountImages;

                DB.saveEmulator(emu);
            }
            else if (emulatorProfile.ID > -1)
            {
                dbExecute(
                    "UPDATE EmulatorProfiles SET " +
                        "emulator_path='" + encode(emulatorProfile.EmulatorPath) + "', " +
                        "title='" + encode(emulatorProfile.Title) + "', " +
                        "working_path='" + encode(emulatorProfile.WorkingDirectory) + "', " +
                        "usequotes='" + emulatorProfile.UseQuotes + "', " +
                        "args='" + encode(emulatorProfile.Arguments) + "', " +
                        "suspend_mp='" + emulatorProfile.SuspendMP + "', " +
                        "goodmerge_pref1='" + emulatorProfile.GoodMergePref1 + "', " +
                        "goodmerge_pref2='" + emulatorProfile.GoodMergePref2 + "', " +
                        "goodmerge_pref3='" + emulatorProfile.GoodMergePref3 + "', " +
                        "mountimages='" + emulatorProfile.MountImages + "' " +

                    " WHERE uid=" + emulatorProfile.ID

                    );
            }
            else //new Profile
            {
                emulatorProfile.ID = getNewProfileIDandInsert(emulatorProfile);
            }

            return emulatorProfile.ID;
        }

        static int getNewProfileIDandInsert(EmulatorProfile emulatorProfile)
        {
            if (!isDBInit)
                return -2;

            //get next id and insert profile within 1 lock to avoid race condition
            lock (dbLock)
            {
                if (emulatorProfile.ID < 0)
                {
                    SQLiteResultSet result = sqlDB.Execute("SELECT MAX(uid)+1 FROM EmulatorProfiles");
                    if (result.Rows.Count > 0)
                    {
                        emulatorProfile.ID = DatabaseUtility.GetAsInt(result, 0, 0);
                    }
                    else
                    {
                        emulatorProfile.ID = 0;
                    }
                }

                sqlDB.Execute(
                    "INSERT INTO EmulatorProfiles VALUES(" +
                        emulatorProfile.ID + "," +
                        "'" + encode(emulatorProfile.Title) + "'," +
                        emulatorProfile.EmulatorID + "," +
                        "'" + encode(emulatorProfile.EmulatorPath) + "'," +
                        "'" + encode(emulatorProfile.WorkingDirectory) + "'," +
                        "'" + emulatorProfile.UseQuotes + "'," +
                        "'" + encode(emulatorProfile.Arguments) + "'," +
                        "'" + emulatorProfile.SuspendMP + "'," +
                        "'" + emulatorProfile.GoodMergePref1 + "'," +
                        "'" + emulatorProfile.GoodMergePref2 + "'," +
                        "'" + emulatorProfile.GoodMergePref3 + "'," +
                        "'" + emulatorProfile.MountImages + "'" +
                    ")"
                    );
            }

            return emulatorProfile.ID;
        }

    }
}
