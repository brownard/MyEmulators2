using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public class EmulatorProfile : DBInterface
    {
        public EmulatorProfile(bool isDefault)
        {
            if (isDefault)
            {
                this.IsDefault = true;
                this.Title = "Default";
            }
            else
                this.Title = "New Profile";
            this.ID = -2;
            this.EmulatorID = -3;
            this.EmulatorPath = "";
            this.WorkingDirectory = "";
            this.Arguments = "";
            this.UseQuotes = null;
            this.SuspendMP = null;
            this.GoodmergeTags = "";
            this.MountImages = false;
            this.EscapeToExit = false;
            this.CheckController = false;
            this.GameId = -1;
        }

        #region Properties

        public bool IsDefault
        {
            get;
            protected set;
        }

        public int ID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public int EmulatorID
        {
            get;
            set;
        }

        public string EmulatorPath
        {
            get;
            set;
        }

        public string WorkingDirectory
        {
            get;
            set;
        }

        public string Arguments
        {
            get;
            set;
        }

        public bool? UseQuotes
        {
            get;
            set;
        }

        public bool? SuspendMP
        {
            get;
            set;
        }

        public bool EnableGoodmerge { get; set; }

        public string GoodmergeTags
        {
            get;
            set;
        }

        public List<string> GetGoodmergeTags()
        {
            List<string> tags = new List<string>();
            if (!string.IsNullOrEmpty(GoodmergeTags))
            {
                string[] sTags = GoodmergeTags.Split(';');
                for(int x = 0; x < sTags.Length; x++)
                {
                    string tag = sTags[x].Trim();
                    if (tag.Length > 0)
                        tags.Add(tag);
                }
            }
            return tags;
        }

        public bool MountImages
        {
            get;
            set;
        }

        public bool EscapeToExit
        {
            get;
            set;
        }

        public bool CheckController
        {
            get;
            set;
        }


        public string PreCommand
        {
            get;
            set;
        }

        public bool PreCommandWaitForExit
        {
            get;
            set;
        }

        public bool PreCommandShowWindow
        {
            get;
            set;
        }

        public string PostCommand
        {
            get;
            set;
        }

        public bool PostCommandWaitForExit
        {
            get;
            set;
        }

        public bool PostCommandShowWindow
        {
            get;
            set;
        }

        public string LaunchedExe
        {
            get;
            set;
        }

        public int GameId
        {
            get;
            set;
        }

        public bool? StopEmulationOnKey
        {
            get;
            set;
        }

        public bool DelayResume
        {
            get;
            set;
        }

        public int ResumeDelay
        {
            get;
            set;
        }

        #endregion

        #region AutoConfig

        //These are only used when creating auto-configuration settings
        public string Filters { get; set; }
        public string Platform { get; set; }
        public bool HasSettings { get; set; }

        #endregion

        #region DBInterface Members

        public const string TABLE_NAME = "EmulatorProfiles";

        public const string TABLE_STRING =
            "uid int," +
            "title varchar(200)," +
            "emulator_id int," +
            "emulator_path varchar(200)," +
            "working_path varchar(200)," +
            "usequotes char(5)," +
            "args varchar(200)," +
            "suspend_mp char(5)," +
            "mountimages char(5)," +
            "escapetoexit char(5)," +
            "checkcontroller char(5)," +
            "defaultprofile char(5)," +
            "precommand varchar(200)," +
            "prewaitforexit char(5)," +
            "preshowwindow char(5)," +
            "postcommand varchar(200)," +
            "postwaitforexit char(5)," +
            "postshowwindow char(5)," +
            "game_id int," +
            "launchedexe varchar(200)," +
            "stopemulation int," +
            "delayresume char(5)," +
            "resumedelay int," +
            "goodmergepref varchar(200)," +
            "enablegoodmerge char(5)," +
            "PRIMARY KEY(uid)";

        const string INSERT_STRING =
            "{0}, " + //id
            "'{1}', " + //encode(title)
            "{2}, " + //emuId
            "'{3}', " + //encode(emuPath)
            "'{4}', " + //encode(workingDir)
            "'{5}', " + //useQuotes
            "'{6}', " + //encode(args)
            "'{7}', " + //suspendMP
            "'{8}'," + //mountImages
            "'{9}'," + //escape to exit
            "'{10}'," + //checkcontroller
            "'{11}'," + //isdefault
            "'{12}'," + //precommand
            "'{13}'," + //prewaitforexit
            "'{14}'," + //preshowwindow
            "'{15}'," + //postcommand
            "'{16}'," + //postwaitforexit
            "'{17}'," + //postshowwindow
            "{18}," + //game_id
            "'{19}', " + //launched exe
            "{20}, " + //stopemulation
            "'{21}', " + //delay resume
            "{22}, " + //resume delay
            "'{23}', " + //encode(goodPref)
            "'{24}'"; //enable goodmerge

        const string UPDATE_STRING =
            "title='{1}', " +
            "emulator_path='{3}', " +
            "working_path='{4}', " +
            "usequotes='{5}', " +
            "args='{6}', " +
            "suspend_mp='{7}', " +
            "mountimages='{8}', " +
            "escapetoexit='{9}', " +
            "checkcontroller='{10}', " +
            "defaultprofile='{11}', " +
            "precommand='{12}', " +
            "prewaitforexit='{13}', " +
            "preshowwindow='{14}', " +
            "postcommand='{15}', " +
            "postwaitforexit='{16}', " +
            "postshowwindow='{17}', " +
            "game_id={18}, " +
            "launchedexe='{19}', " +
            "stopemulation={20}, " +
            "delayresume='{21}', " +
            "resumedelay={22}, " +
            "goodmergepref='{23}', " +
            "enablegoodmerge='{24}' " +
            "WHERE uid={0}";

        object[] DBParams
        {
            get
            {
                return new object[] 
                { 
                    ID,
                    DB.Encode(Title),
                    EmulatorID,
                    DB.Encode(EmulatorPath),
                    DB.Encode(WorkingDirectory),
                    UseQuotes == true,
                    DB.Encode(Arguments),
                    SuspendMP == true,
                    MountImages,
                    EscapeToExit,
                    CheckController,
                    IsDefault,
                    DB.Encode(PreCommand),
                    PreCommandWaitForExit,
                    PreCommandShowWindow,
                    DB.Encode(PostCommand),
                    PostCommandWaitForExit,
                    PostCommandShowWindow,
                    GameId,
                    DB.Encode(LaunchedExe),
                    intFromBool(StopEmulationOnKey),
                    DelayResume,
                    ResumeDelay,
                    DB.Encode(GoodmergeTags),
                    EnableGoodmerge
                };
            }
        }

        public static EmulatorProfile CreateProfile(SQLite.NET.SQLiteResultSet.Row sqlRow)
        {
            if (sqlRow.fields.Count != 25)
            {
                Logger.LogError("Unable to create Profile, invalid database row");
                return null;
            }

            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

            EmulatorProfile profile = new EmulatorProfile(false);
            profile.ID = int.Parse(sqlRow.fields[0], culture);
            profile.Title = DB.Decode(sqlRow.fields[1]);
            profile.EmulatorID = int.Parse(sqlRow.fields[2], culture);
            profile.EmulatorPath = DB.Decode(sqlRow.fields[3]);
            profile.WorkingDirectory = DB.Decode(sqlRow.fields[4]);
            profile.UseQuotes = bool.Parse(sqlRow.fields[5]);
            profile.Arguments = DB.Decode(sqlRow.fields[6]);
            profile.SuspendMP = bool.Parse(sqlRow.fields[7]);
            profile.MountImages = bool.Parse(sqlRow.fields[8]);
            profile.EscapeToExit = bool.Parse(sqlRow.fields[9]);
            profile.CheckController = bool.Parse(sqlRow.fields[10]);
            profile.IsDefault = bool.Parse(sqlRow.fields[11]);
            profile.PreCommand = DB.Decode(sqlRow.fields[12]);
            profile.PreCommandWaitForExit = bool.Parse(sqlRow.fields[13]);
            profile.PreCommandShowWindow = bool.Parse(sqlRow.fields[14]);
            profile.PostCommand = DB.Decode(sqlRow.fields[15]);
            profile.PostCommandWaitForExit = bool.Parse(sqlRow.fields[16]);
            profile.PostCommandShowWindow = bool.Parse(sqlRow.fields[17]);
            profile.GameId = int.Parse(sqlRow.fields[18], culture);
            profile.LaunchedExe = DB.Decode(sqlRow.fields[19]);
            profile.StopEmulationOnKey = boolFromInt(int.Parse(sqlRow.fields[20], culture));
            profile.DelayResume = bool.Parse(sqlRow.fields[21]);
            profile.ResumeDelay = int.Parse(sqlRow.fields[22], culture);
            profile.GoodmergeTags = DB.Decode(sqlRow.fields[23]);
            profile.EnableGoodmerge = bool.Parse(sqlRow.fields[24]);
            return profile;
        }

        private static bool? boolFromInt(int i)
        {
            if (i < 0)
                return null;
            return i > 0;
        }

        private static int intFromBool(bool? b)
        {
            if (!b.HasValue)
                return -1;
            return b.Value ? 1 : 0;
        }

        public static string DBTableString
        {
            get
            {
                return "CREATE TABLE " + TABLE_NAME + "(" + TABLE_STRING + ")";
            }
        }

        public string GetDBInsertString()
        {
            return "INSERT INTO " + TABLE_NAME + " VALUES(" + string.Format(System.Globalization.CultureInfo.InvariantCulture, INSERT_STRING, DBParams) + ")";
        }

        public string GetDBUpdateString()
        {
            return "UPDATE " + TABLE_NAME + " SET " + string.Format(System.Globalization.CultureInfo.InvariantCulture, UPDATE_STRING, DBParams);
        }

        public void Save()
        {
            lock (DB.Instance.SyncRoot)
            {
                if (Exists())
                {
                    DB.Instance.ExecuteWithoutLock(GetDBUpdateString());
                }
                else
                {
                    SQLite.NET.SQLiteResultSet result = DB.Instance.ExecuteWithoutLock("SELECT MAX(uid)+1 FROM {0}", TABLE_NAME);
                    if (result.Rows.Count > 0)
                        ID = MediaPortal.Database.DatabaseUtility.GetAsInt(result, 0, 0);
                    else
                        ID = 0;

                    DB.Instance.ExecuteWithoutLock(GetDBInsertString());
                }
            }
        }

        public void Delete()
        {
            if (ID < 0)
                return;

            DB.Instance.Execute("DELETE FROM {0} WHERE uid={1}", TABLE_NAME, ID);
        }

        public bool Exists()
        {
            return DB.Instance.Execute("SELECT 1 FROM {0} WHERE uid={1}", TABLE_NAME, ID).Rows.Count
                != 0;
        }

        #endregion
    }
}
