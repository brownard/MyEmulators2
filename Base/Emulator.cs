using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;

namespace MyEmulators2
{
    public enum EmulatorType
    {
        manyemulators,
        pcgame
    }

    public class Emulator : DBItem, DBInterface
    {
        public Emulator() {}

        public Emulator(EmulatorType specialRole)
        {
            switch (specialRole)
            {
                case EmulatorType.pcgame:
                {
                    UID = -1;
                    Title = Translator.Instance.pcgames;
                    PlatformTitle = "Windows";
                    break;
                }
                case EmulatorType.manyemulators:
                {
                    UID = -2; 
                    break;
                }
            }
        }

        EmulatorProfile defaultProfile = null;
        public EmulatorProfile DefaultProfile
        {
            get
            {
                if (defaultProfile == null)
                {
                    if (uid > -2)
                        defaultProfile = DB.Instance.GetProfile(-1, this);
                    if (defaultProfile == null)
                        defaultProfile = new EmulatorProfile(true);
                }
                return defaultProfile;
            }
        }

        private int uid = -3;
        public int UID
        {
            get { return uid; }
            set { uid = value; }
        }

        public int Grade { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public int Position { get; set; }
        public string PathToRoms { get; set; }
        public string Title { get; set; }
        public string Filter { get; set; }
        public int View { get; set; }
        public string PlatformTitle { get; set; }
        public bool IsArcade { get; set; }
        public double CaseAspect { get; set; }

        public void SetCaseAspect(string aspect)
        {
            int index = aspect.TrimStart().IndexOf(" ");
            if (index > -1)
                aspect = aspect.Substring(0, index);

            double caseAspect;
            if (double.TryParse(aspect, out caseAspect))
                CaseAspect = caseAspect;
        }

        public bool IsPc()
        {
            return UID == -1;
        }

        public bool IsManyEmulators()
        {
            return UID == -2;
        }

        public override string ToString()
        {
            return Title;
        }

        string videoPreview = "";
        public string VideoPreview 
        { 
            get { return videoPreview; } 
            set 
            {
                if (string.IsNullOrEmpty(value))
                    videoPreview = "";
                else
                    videoPreview = value;
            } 
        }
        
        public override ExtendedGUIListItem CreateGUIListItem()
        {
            ExtendedGUIListItem listItem = new ExtendedGUIListItem(Title);
            listItem.AssociatedEmulator = this;
            listItem.ThumbnailImage = listItem.ThumbGroup.FrontCoverDefaultPath;
            return listItem;
        }

        #region DBInterface Members

        public const string TABLE_NAME = "Emulators";

        const string TABLE_STRING =
            "uid int," +
            "title varchar(50)," +
            "rom_path varchar(200)," +
            "filter varchar(100)," +
            "position int," +
            "view int," +
            "platformtitle varchar(50)," +
            "Company varchar(200)," +
            "Yearmade int," +
            "Description varchar(2000)," +
            "Grade int," +
            "videopreview varchar(200)," +
            "caseaspect int," +
            "isarcade char(5)," +
            "PRIMARY KEY(uid)";

        const string INSERT_STRING =
            "{0}," + //uid
            "'{1}'," + //title
            "'{2}'," + //path to roms
            "'{3}'," + //filters
            "{4}," + //position
            "{5}," + //default view
            "'{6}'," + //platform title
            "'{7}'," + //company
            "{8}," + //year
            "'{9}'," + //description
            "{10}," + //grade
            "'{11}'," + //encode(videopreview)
            "{12}," + //case aspect
            "'{13}'"; //is arcade

        const string UPDATE_STRING =
            "title='{1}', " +
            "rom_path='{2}', " +
            "filter='{3}', " +
            "position={4}, " +
            "view={5}, " +
            "platformtitle='{6}', " +
            "Company='{7}', " +
            "Yearmade={8}, " +
            "Description='{9}', " +
            "Grade={10}, " +
            "videopreview='{11}', " +
            "caseaspect={12}, " +
            "isarcade='{13}' " +
            "WHERE uid={0}";

        object[] DBParams
        {
            get
            {
                return new object[] 
                { 
                    uid,
                    DB.Encode(Title), 
                    DB.Encode(PathToRoms), 
                    Filter,
                    Position, 
                    View, 
                    DB.Encode(PlatformTitle),
                    DB.Encode(Company),
                    Year,
                    DB.Encode(Description),
                    Grade,
                    DB.Encode(videoPreview),
                    Math.Round(CaseAspect * 100),
                    IsArcade
                };
            }
        }

        public static Emulator CreateEmulator(SQLite.NET.SQLiteResultSet.Row sqlRow)
        {
            if (sqlRow.fields.Count != 14)
            {
                Logger.LogError("Unable to create Emulator, invalid database row");
                return null;
            }

            Emulator emu = new Emulator();
            emu.uid = int.Parse(sqlRow.fields[0]);
            emu.Title = DB.Decode(sqlRow.fields[1]);
            emu.PathToRoms = DB.Decode(sqlRow.fields[2]);
            emu.Filter = sqlRow.fields[3];
            emu.Position = int.Parse(sqlRow.fields[4]);
            emu.View = int.Parse(sqlRow.fields[5]);
            emu.PlatformTitle = DB.Decode(sqlRow.fields[6]);
            emu.Company = DB.Decode(sqlRow.fields[7]);
            emu.Year = int.Parse(sqlRow.fields[8]);
            emu.Description = DB.Decode(sqlRow.fields[9]);
            emu.Grade = int.Parse(sqlRow.fields[10]);
            emu.videoPreview = DB.Decode(sqlRow.fields[11]);
            int lAspect = int.Parse(sqlRow.fields[12]);
            if (lAspect != 0)
                emu.CaseAspect = lAspect / 100.00;
            emu.IsArcade = bool.Parse(sqlRow.fields[13]);
            return emu;
        }

        public static Emulator GetPC()
        {
            Emulator emu = new Emulator(EmulatorType.pcgame);

            string title = Options.Instance.GetStringOption("pcitemtitle");
            if (title != "")
                emu.Title = title;

            emu.Company = Options.Instance.GetStringOption("pcitemcompany");
            emu.Description = Options.Instance.GetStringOption("pcitemdescription");
            emu.Year = Options.Instance.GetIntOption("pcitemyear");
            emu.Grade = Options.Instance.GetIntOption("pcitemgrade");
            int lAspect = Options.Instance.GetIntOption("pcitemcaseaspect");
            if (lAspect != 0)
                emu.CaseAspect = lAspect / 100.00;
            emu.Position = Options.Instance.GetIntOption("pcitemposition");
            emu.videoPreview = Options.Instance.GetStringOption("pcitemvideopreview");

            emu.View = Options.Instance.GetIntOption("viewpcgames");
            emu.PathToRoms = Options.Instance.GetStringOption("pcitemdirectory");
            emu.Filter = Options.Instance.GetStringOption("pcitemfilter");

            return emu;
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
            if (uid == -1)
            {
                savePC();
                return;
            }
            else if (uid == -2)
                return;

            lock (DB.Instance.SyncRoot)
            {
                if (Exists())
                {
                    DB.Instance.Execute(GetDBUpdateString());
                }
                else
                {
                    SQLite.NET.SQLiteResultSet result = DB.Instance.ExecuteWithoutLock("SELECT MAX(uid)+1 FROM {0}", TABLE_NAME);
                    if (result.Rows.Count > 0)
                        uid = MediaPortal.Database.DatabaseUtility.GetAsInt(result, 0, 0);
                    else
                        uid = 0;

                    DB.Instance.ExecuteWithoutLock(GetDBInsertString());
                    DefaultProfile.EmulatorID = uid;
                    DefaultProfile.Save();
                }
            }
        }

        public void SavePosition()
        {
            if (uid < -1)
                return;

            if (uid == -1)
                Options.Instance.UpdateOption("pcitemposition", Position);
            else
                DB.Instance.Execute("UPDATE {0} SET position={1} WHERE uid={2}", TABLE_NAME, Position, uid);
        }

        void savePC()
        {
            if (uid != -1)
                return;

            Options.Instance.UpdateOption("pcitemtitle", Title);
            Options.Instance.UpdateOption("pcitemcompany", Company);
            Options.Instance.UpdateOption("pcitemdescription", Description);
            Options.Instance.UpdateOption("pcitemyear", Year);
            Options.Instance.UpdateOption("pcitemgrade", Grade);
            Options.Instance.UpdateOption("pcitemcaseaspect", (int)Math.Round(CaseAspect * 100));
            Options.Instance.UpdateOption("pcitemposition", Position);
            Options.Instance.UpdateOption("pcitemvideopreview", videoPreview);
            Options.Instance.UpdateOption("viewpcgames", View);
            Options.Instance.UpdateOption("pcitemcheckcontroller", false);
            Options.Instance.UpdateOption("pcitemdirectory", PathToRoms);
            Options.Instance.UpdateOption("pcitemfilter", Filter);
        }

        public void Delete()
        {
            if (uid < 0)
                return;

            lock (DB.Instance.SyncRoot)
            {
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE uid={1}", TABLE_NAME, uid);
                //remove any associated profiles
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE emulator_id={1}", EmulatorProfile.TABLE_NAME, uid);
                //remove associated games
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE parentemu={1}", Game.TABLE_NAME, uid);
                //remove discs
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE emuid={1}", GameDisc.TABLE_NAME, uid);
                DeleteThumbs();
            }
        }

        public bool Exists()
        {
            if (uid < -1)
                return false;

            return DB.Instance.Execute("SELECT 1 FROM {0} WHERE uid={1}", TABLE_NAME, uid).Rows.Count > 0;
        }

        #endregion

    }
}
