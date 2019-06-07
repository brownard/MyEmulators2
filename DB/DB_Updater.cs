using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Database;
using SQLite.NET;

namespace MyEmulators2
{
    class DB_Updater
    {
        public DB_Updater()
        {
            loadUpdates();
        }

        List<Update> updates = null;

        void loadUpdates()
        {
            updates = new List<Update>();

            #region Old Updates
            //updates.Add(new Update(0.2, new Update.UpdateDelegate(delegate() 
            //    { 
            //        DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN escapetoexit char(5)", Emulator.TABLE_NAME);
            //        DB.Instance.Execute("UPDATE {0} SET escapetoexit='False'", Emulator.TABLE_NAME);

            //        DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN escapetoexit char(5)", EmulatorProfile.TABLE_NAME);
            //        DB.Instance.Execute("UPDATE {0} SET escapetoexit='False'", EmulatorProfile.TABLE_NAME);

            //        DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN hash varchar(100)", Game.TABLE_NAME);
            //    }
            //    )));

            //updates.Add(new Update(0.3, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN platformtitle varchar(100)", Emulator.TABLE_NAME);
            //}
            //    )));

            //updates.Add(new Update(0.4, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN videopreview varchar(200)", Game.TABLE_NAME);
            //}
            //    )));

            //updates.Add(new Update(0.5, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN videopreview varchar(200)", Emulator.TABLE_NAME);
            //}
            //    )));
            //updates.Add(new Update(0.6, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN caseaspect int", Emulator.TABLE_NAME);
            //    DB.Instance.Execute("UPDATE {0} SET caseaspect=0", Emulator.TABLE_NAME);
            //}
            //    )));
            
            //updates.Add(new Update(0.7, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("BEGIN");
            //    DB.Instance.Execute("CREATE TABLE gamestemp ({0})", Game.TABLE_STRING);
            //    DB.Instance.Execute("INSERT INTO gamestemp SELECT * FROM {0}", Game.TABLE_NAME);
            //    DB.Instance.Execute("DROP TABLE {0}", Game.TABLE_NAME);
            //    DB.Instance.Execute("ALTER TABLE gamestemp RENAME TO {0}", Game.TABLE_NAME);
            //    DB.Instance.Execute("COMMIT");
            //}
            //    )));

            //updates.Add(new Update(0.8, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("UPDATE {0} SET latestplay='{1}'", Game.TABLE_NAME, DateTime.MinValue.ToString("s"));
            //}
            //    )));
            //updates.Add(new Update(0.9, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN checkcontroller char(5)", Emulator.TABLE_NAME);
            //    DB.Instance.Execute("UPDATE {0} SET checkcontroller='False'", Emulator.TABLE_NAME);

            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN checkcontroller char(5)", EmulatorProfile.TABLE_NAME);
            //    DB.Instance.Execute("UPDATE {0} SET checkcontroller='False'", EmulatorProfile.TABLE_NAME);
            //}
            //    )));

            //updates.Add(new Update(1.0, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN currentdisc int", Game.TABLE_NAME);
            //    DB.Instance.Execute("UPDATE {0} SET currentdisc=0", Game.TABLE_NAME);
            //}
            //    )));

            //updates.Add(new Update(1.1, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN arguments varchar(200)", Game.TABLE_NAME);
            //    Dictionary<string, Game> updateGames = new Dictionary<string, Game>();
            //    List<Game> games = DB.Instance.GetGames(new Emulator(EmulatorType.pcgame));
            //    foreach (Game game in games)
            //    {
            //        int index = game.Path.IndexOf(".exe ");
            //        if(index < 0)
            //            index = game.Path.IndexOf(".bat ");

            //        if (index > -1)
            //        {
            //            string args = game.Path.Substring(index + 5).Trim();
            //            string lPath = game.Path.Remove(index + 4);
            //            if (args != "" && !updateGames.ContainsKey(lPath))
            //            {
            //                if (!Game.Exists(lPath))
            //                {
            //                    game.Arguments = args;
            //                    updateGames.Add(lPath, game);
            //                }
            //                else
            //                    Logger.LogError("Unable to update game arguments for {0} - a game with path '{1}' already exists", game.Title, lPath);
            //            }
            //        }
            //    }

            //    DB.Instance.Execute("BEGIN");
            //    foreach (KeyValuePair<string, Game> keyVal in updateGames)
            //    {
            //        DB.Instance.Execute("UPDATE {0} SET path='{1}', arguments='{2}' WHERE gameid={3}", Game.TABLE_NAME, DB.Encode(keyVal.Key), DB.Encode(keyVal.Value.Arguments), keyVal.Value.GameID);
            //    }
            //    DB.Instance.Execute("COMMIT");
            //}
            //    )));

            //updates.Add(new Update(1.2, new Update.UpdateDelegate(delegate()
            //{
            //    DB.Instance.Execute("BEGIN");
            //    DB.Instance.Execute("ALTER TABLE {0} RENAME TO emuprofiletemp", EmulatorProfile.TABLE_NAME);
            //    DB.Instance.Execute(EmulatorProfile.DBTableString);
            //    DB.Instance.Execute("INSERT INTO {0} (uid, title, emulator_id, emulator_path, working_path, usequotes, args, suspend_mp, goodmerge_pref1, goodmerge_pref2, goodmerge_pref3, mountimages, escapetoexit, checkcontroller) SELECT * FROM emuprofiletemp", EmulatorProfile.TABLE_NAME);
            //    DB.Instance.Execute("DROP TABLE emuprofiletemp");
            //    DB.Instance.Execute("UPDATE {0} SET defaultprofile='False', prewaitforexit='False', preshowwindow='False', postwaitforexit='False', postshowwindow='False'", EmulatorProfile.TABLE_NAME);
            //    SQLiteResultSet emuResults = DB.Instance.Execute("SELECT uid, emulator_path, working_path, args, usequotes, suspend_mp, goodmerge_pref1, goodmerge_pref2, goodmerge_pref3, mountimages, escapetoexit, checkcontroller FROM {0}", Emulator.TABLE_NAME);
            //    foreach (SQLiteResultSet.Row emu in emuResults.Rows)
            //    {
            //        EmulatorProfile defaultProfile = new EmulatorProfile(true)
            //        {
            //            EmulatorID = int.Parse(emu.fields[0]),
            //            EmulatorPath = DB.Decode(emu.fields[1]),
            //            WorkingDirectory = DB.Decode(emu.fields[2]),
            //            Arguments = DB.Decode(emu.fields[3]),
            //            UseQuotes = bool.Parse(emu.fields[4]),
            //            SuspendMP = bool.Parse(emu.fields[5]),
            //            GoodMergePref = emu.fields[6],
            //            GoodMergePref2 = emu.fields[7],
            //            GoodMergePref3 = emu.fields[8],
            //            MountImages = bool.Parse(emu.fields[9]),
            //            EscapeToExit = bool.Parse(emu.fields[10]),
            //            CheckController = bool.Parse(emu.fields[11])
            //        };
            //        defaultProfile.Save();
            //    }
            //    DB.Instance.Execute("ALTER TABLE {0} RENAME TO emulatorstemp", Emulator.TABLE_NAME);
            //    DB.Instance.Execute(Emulator.DBTableString);
            //    DB.Instance.Execute("INSERT INTO {0} SELECT uid, title, rom_path, filter, enable_goodmerge, position, view, platformtitle, Company, Yearmade, Description, Grade, videopreview, caseaspect FROM emulatorstemp", Emulator.TABLE_NAME);
            //    DB.Instance.Execute("DROP TABLE emulatorstemp");
            //    DB.Instance.Execute("COMMIT");
            //}
            //    )));

            //updates.Add(new Update(1.3, new Update.UpdateDelegate(delegate()
            //{
            //    try
            //    {
            //        DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN game_id int", EmulatorProfile.TABLE_NAME);
            //    }
            //    catch { }
            //    try
            //    {
            //        DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN launchedexe varchar(200)", EmulatorProfile.TABLE_NAME);
            //    }
            //    catch { }
            //    DB.Instance.Execute("UPDATE {0} SET game_id=-1", EmulatorProfile.TABLE_NAME);
            //}
            //    )));
            #endregion

            //Added since BETA

            updates.Add(new Update(1.4, new Update.UpdateDelegate(delegate()
            {
                DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN stopemulation int", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("UPDATE {0} SET stopemulation=-1", EmulatorProfile.TABLE_NAME);
            }
                )));

            updates.Add(new Update(1.5, new Update.UpdateDelegate(delegate()
            {
                DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN delayresume char(5)", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN resumedelay int", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("UPDATE {0} SET delayresume='False', resumedelay=500", EmulatorProfile.TABLE_NAME);
            }
                )));
            updates.Add(new Update(1.6, new Update.UpdateDelegate(delegate()
            {
                string profileTableString =
                "uid int, title varchar(200), emulator_id int, emulator_path varchar(200), working_path varchar(200), usequotes char(5), args varchar(200), suspend_mp char(5), mountimages char(5), escapetoexit char(5), checkcontroller char(5), defaultprofile char(5), precommand varchar(200), prewaitforexit char(5), preshowwindow char(5), postcommand varchar(200), postwaitforexit char(5), postshowwindow char(5), game_id int, launchedexe varchar(200), stopemulation int, delayresume char(5), resumedelay int, goodmergepref varchar(200), PRIMARY KEY(uid)";

                DB.Instance.Execute("BEGIN");
                DB.Instance.Execute("ALTER TABLE {0} RENAME TO emuprofiletemp", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("CREATE TABLE {0}({1})", EmulatorProfile.TABLE_NAME, profileTableString);
                DB.Instance.Execute("INSERT INTO {0} SELECT uid, title, emulator_id, emulator_path, working_path, usequotes, args, suspend_mp, mountimages, escapetoexit, checkcontroller, defaultprofile, precommand, prewaitforexit, preshowwindow, postcommand, postwaitforexit, postshowwindow, game_id, launchedexe, stopemulation, delayresume, resumedelay, '' FROM emuprofiletemp", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("COMMIT");
                SQLiteResultSet currentPrefixes = DB.Instance.Execute("SELECT uid, goodmerge_pref1, goodmerge_pref2, goodmerge_pref3 FROM emuprofiletemp");
                DB.Instance.ExecuteTransaction(currentPrefixes.Rows, row =>
                {
                    string prefix = row.fields[1];
                    if (!string.IsNullOrEmpty(row.fields[2]))
                    {
                        if (!string.IsNullOrEmpty(prefix))
                            prefix += ";";
                        prefix += row.fields[2];
                    }
                    if (!string.IsNullOrEmpty(row.fields[3]))
                    {
                        if (!string.IsNullOrEmpty(prefix))
                            prefix += ";";
                        prefix += row.fields[3];
                    }
                    if (!string.IsNullOrEmpty(prefix))
                        DB.Instance.Execute("UPDATE {0} SET goodmergepref='{1}' WHERE uid={2}", EmulatorProfile.TABLE_NAME, prefix, row.fields[0]);
                });
                DB.Instance.Execute("DROP TABLE emuprofiletemp");

            }
                )));

            updates.Add(new Update(1.7, new Update.UpdateDelegate(delegate()
            {
                string emuTableString =
                "uid int, title varchar(50), rom_path varchar(200), filter varchar(100), position int, view int, platformtitle varchar(50), Company varchar(200), Yearmade int, Description varchar(2000), Grade int, videopreview varchar(200), caseaspect int, isarcade char(5), PRIMARY KEY(uid)";

                DB.Instance.Execute("BEGIN");
                DB.Instance.Execute("ALTER TABLE {0} RENAME TO emustemp", Emulator.TABLE_NAME);
                DB.Instance.Execute("CREATE TABLE {0}({1})", Emulator.TABLE_NAME, emuTableString);
                DB.Instance.Execute(
                    "INSERT INTO {0} SELECT uid, title, rom_path, filter, position, view, platformtitle, Company, Yearmade, Description, Grade, videopreview, caseaspect, 'False' FROM emustemp", 
                    Emulator.TABLE_NAME);

                DB.Instance.Execute("ALTER TABLE {0} ADD COLUMN enablegoodmerge char(5)", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("UPDATE {0} SET enablegoodmerge='False'", EmulatorProfile.TABLE_NAME);
                DB.Instance.Execute("COMMIT");

                SQLiteResultSet enableGoodmerge = DB.Instance.Execute("SELECT uid, enable_goodmerge FROM emustemp");
                DB.Instance.ExecuteTransaction(enableGoodmerge.Rows, row =>
                {
                    if (row.fields[1] == "True")
                        DB.Instance.Execute("UPDATE {0} SET enablegoodmerge='True' WHERE emulator_id={1}", EmulatorProfile.TABLE_NAME, row.fields[0]);
                });
                DB.Instance.Execute("DROP TABLE emustemp");
            }
                )));
            //add Update's here
        }

        public bool Update()
        {
            bool result = false;
            lock (DB.Instance.SyncRoot)
            {
                DB.Instance.SupressExceptions = false;
                try
                {
                    double currentDBVersion = DB.Instance.CurrentDBVersion;
                    foreach (Update update in updates)
                    {
                        if (currentDBVersion < update.MinRequiredVersion)
                            update.UpdateMethod();
                    }
                    result = true;
                }
                catch (Exception ex) 
                { 
                    Logger.LogError(ex); 
                }
                DB.Instance.SupressExceptions = true;
            }
            return result;
        }
    }

    class Update
    {
        public Update(double minRequiredVersion, UpdateDelegate updateMethod)
        {
            MinRequiredVersion = minRequiredVersion;
            UpdateMethod = updateMethod;
        }

        public delegate void UpdateDelegate();

        public double MinRequiredVersion
        {
            get;
            set;
        }

        public UpdateDelegate UpdateMethod
        {
            get;
            set;
        }
    }

}
