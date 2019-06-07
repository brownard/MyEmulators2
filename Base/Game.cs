using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;

namespace MyEmulators2
{
    public class Game : DBItem, DBInterface
    {
        public Game()
        {

        }

        public Game(string path, Emulator parentEmu)
        {
            Path = path == null ? "" : path;
            Title = titleFromPath(Path);

            parentEmulator = parentEmu;
            Visible = true;
            LaunchFile = "";
        }

        private string titleFromPath(string path)
        {
            string s = "";
            int index = path.LastIndexOf(".");
            if (index > -1)
                s = path.Remove(index);

            if (s.Length > 0)
                s = s.Substring(s.LastIndexOf("\\") + 1);

            return s;
        }

        private int gameid = -2;
        public int GameID
        {
            get { return gameid; }
            set { gameid = value; }
        }

        private String title = "";
        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        private String path = "";
        public String Path
        {
            get { return path; }
            set 
            { 
                path = value;
                try
                {
                    Filename = System.IO.Path.GetFileNameWithoutExtension(value);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error getting filename from path {0} - {1}", value, ex.Message);
                    Filename = value;
                }
                goodmergeChecked = false;
            }
        }

        string arguments = "";
        public string Arguments
        {
            get { return arguments; }
            set { arguments = value; }
        }

        private String filename = "";
        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private Emulator parentEmulator = null;
        public Emulator ParentEmulator
        {
            get { return parentEmulator; }
            set 
            { 
                parentEmulator = value;
                goodmergeChecked = false;
            }
        }

        private int grade = 0;
        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        private int playcount = 0;
        public int Playcount
        {
            get { return playcount; }
            set { playcount = value; }
        }

        private int yearmade = 0;
        public int Yearmade
        {
            get { return yearmade; }
            set { yearmade = value; }
        }

        private DateTime latestplay = DateTime.MinValue;
        public DateTime Latestplay
        {
            get { return latestplay; }
            set { latestplay = value; }
        }

        private String description = "";
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        private String genre = "";
        public String Genre
        {
            get { return genre; }
            set 
            { 
                genre = value;
                //genres = null;
            }
        }

        //List<string> genres = null;
        //public List<string> Genres
        //{
        //    get
        //    {
        //        if (genres != null)
        //            return genres;

        //        genres = new List<string>();
        //        if (string.IsNullOrEmpty(genre))
        //            return genres;

        //        foreach (string s in genre.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
        //            genres.Add(s);

        //        return genres;
        //    }
        //}

        int currentDiscNum = 0;
        public int CurrentDiscNum
        {
            get { return currentDiscNum; }
            set 
            {
                if (currentDiscNum == value)
                    return;

                currentDiscNum = value;
                currentDisc = null;
                goodmergeChecked = false;
            }
        }

        GameDisc currentDisc = null;
        public GameDisc CurrentDisc
        {
            get
            {
                if (currentDisc == null)
                    currentDisc = GetDisc(currentDiscNum);
                return currentDisc;
            }
        }

        public GameDisc GetDisc(int discNum)
        {
            List<GameDisc> discs = GetDiscs();
            if (discs.Count < 1)
                return new GameDisc(this) { Path = path, LaunchFile = launchFile };

            int index = currentDiscNum - 1;
            if (index < 0)
                index = 0;
            else if (index > discs.Count - 1)
                index = discs.Count - 1;
            return discs[index];
        }

        public List<GameDisc> GetDiscs()
        {
            List<GameDisc> discs = new List<GameDisc>();
            foreach (SQLite.NET.SQLiteResultSet.Row row in DB.Instance.Execute("SELECT * FROM {0} WHERE gameid={1} ORDER BY discnumber", GameDisc.TABLE_NAME, gameid).Rows)
            {
                GameDisc disc = GameDisc.CreateGameDisc(row);
                if (disc != null)
                    discs.Add(disc);
            }
            if (discs.Count < 1)
                discs.Add(new GameDisc(this) { Path = path, LaunchFile = launchFile });
            return discs;
        }

        List<GameDisc> discs = null;
        public List<GameDisc> Discs
        {
            get
            {
                if (discs == null)
                    discs = GetDiscs();
                return discs;
            }
        }

        private String company = "";
        public String Company
        {
            get { return company; }
            set { company = value; }
        }

        private bool visible = true;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool favourite = false;
        public bool Favourite
        {
            get { return favourite; }
            set { favourite = value; }
        }
 
        public override string ToString()
        {
            return Title;
        }

        private String launchFile = "";
        public String LaunchFile
        {
            get { return launchFile; }
            set { launchFile = value; }
        }

        bool isInfoChecked = false;
        public bool IsInfoChecked
        {
            get { return isInfoChecked; }
            set { isInfoChecked = value; }
        }

        private String hash = "";
        public String Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        public override ExtendedGUIListItem CreateGUIListItem()
        {
            ExtendedGUIListItem listItem = new ExtendedGUIListItem(title);
            listItem.AssociatedGame = this;
            listItem.ThumbnailImage = listItem.ThumbGroup.FrontCoverDefaultPath;
            return listItem;
        }

        /// <summary>
        /// Reset the game object to a blank state. Only the filepath and filename remains.
        /// </summary>
        public void Reset()
        {
            this.Title = titleFromPath(Path);
            this.Grade = 0;
            this.Yearmade = 0;
            this.Description = "";
            this.Genre = "";
            this.Company = "";
            this.Hash = "";
            using (ThumbGroup thumbs = new ThumbGroup(this))
            {
                thumbs.FrontCover.Image = null;
                thumbs.BackCover.Image = null;
                thumbs.InGame.Image = null;
                thumbs.TitleScreen.Image = null;
                thumbs.Fanart.Image = null;
                thumbs.SaveAllThumbs();
            }
        }

        int selectedProfileId = -1;
        public int SelectedProfileId 
        {
            get { return selectedProfileId; }
            set 
            { 
                selectedProfileId = value;
                currentProfile = null;
            } 
        }

        EmulatorProfile currentProfile = null;
        public EmulatorProfile CurrentProfile
        {
            get
            {
                if (currentProfile == null)
                    currentProfile = GetSelectedProfile();
                return currentProfile;
            }            
        }

        public EmulatorProfile GetSelectedProfile()
        {
            return DB.Instance.GetProfile(this);
        }

        public List<EmulatorProfile> GetProfiles()
        {
            return DB.Instance.GetProfiles(this);
        }

        public bool IsMissingInfo()
        {
            if (this.GameID == -2) return true;
            if (String.IsNullOrEmpty(this.Title) ) return true;
            if (this.Grade == 0 ) return true;
            if (this.Yearmade == 0) return true;
            if (String.IsNullOrEmpty(this.Description)) return true;
            if (String.IsNullOrEmpty(this.Genre)) return true;
            if (String.IsNullOrEmpty(this.Company)) return true;
            //don't technically need to dispose of ThumbGroup as we don't load any images
            //but just in case
            using (ThumbGroup thumbs = new ThumbGroup(this))
            {
                if (string.IsNullOrEmpty(thumbs.FrontCover.Path))
                    return true;
                if (string.IsNullOrEmpty(thumbs.BackCover.Path))
                    return true;
                if (string.IsNullOrEmpty(thumbs.InGame.Path))
                    return true;
                if (string.IsNullOrEmpty(thumbs.TitleScreen.Path))
                    return true;
                if (string.IsNullOrEmpty(thumbs.Fanart.Path))
                    return true;
            }
            
            return false;
        }

        bool isGoodmerge = false;
        bool goodmergeChecked = false;
        public bool IsGoodmerge
        {
            get
            {                
                if (goodmergeChecked)
                    return isGoodmerge;
                goodmergeChecked = true;
                isGoodmerge = false;

                if (!CurrentProfile.EnableGoodmerge)
                    return false;

                string lPath = CurrentDisc.Path;
                if (string.IsNullOrEmpty(lPath))
                    return false;

                int index = lPath.LastIndexOf('.');
                if (index < 0)
                    return false; //file doesn't have an extension

                //get file extension
                string extension = lPath.Substring(index, lPath.Length - index).ToLower();
                //get configured Goodmerge extensions
                string[] goodmergeExts = Options.Instance.GetStringOption("goodmergefilters").Split(';');
                for (int x = 0; x < goodmergeExts.Length; x++)
                {
                    //see if extension matches filter
                    if (goodmergeExts[x].EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                        return isGoodmerge = true;
                }
                //no match was found
                return false;
            }
        }

        public List<string> GoodmergeFiles
        {
            get;
            set;
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

        #region DBInterface Members
        
        public const string TABLE_NAME = "Games";

        public const string TABLE_STRING =
            "gameid INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "path varchar(200) UNIQUE, " +
            "parentemu int, " +
            "title varchar(100), " +
            "grade int, " +
            "playcount int, " +
            "yearmade int, " +
            "latestplay varchar(16), " +
            "description varchar(200), " +
            "genre varchar(50), " +
            "company varchar(50), " +
            "visible char(5), " +
            "favourite char(5), " +
            "LaunchFile varchar(200), " +
            "emuprofile int, " +
            "infochecked char(5), " +
            "hash varchar(100), " +
            "videopreview varchar(200), " + 
            "currentdisc int, " +
            "arguments varchar(200)";

        const string INSERT_STRING =
            "NULL, " +
            "'{0}', " + //encode(path)
            "{1}, " + //parentEmu UID
            "'{2}', " + //encode(title)
            "{3}, " + //grade
            "{4}, " + //playcount
            "{5}, " + //yearmade
            "'{6}', " + //latestplay
            "'{7}', " + //encode(description)
            "'{8}', " + //encode(genre)
            "'{9}', " + //encode(company)
            "'{10}'," + //visible
            "'{11}'," + //favourite
            "'{12}' ," + //encode(launchfile)
            "{13}, " + //emuprofile
            "'{14}'," + //infochecked 
            "'{15}'," + //hash
            "'{16}'," + //encode(videoPreview)
            "{17}," + //currentDisc
            "'{18}'"; //encode(arguments)

        const string UPDATE_STRING =
            "parentemu={1}, " +
            "title='{2}', " +
            "grade={3}, " +
            "playcount={4}, " +
            "yearmade={5}, " +
            "latestplay='{6}', " +
            "description='{7}', " +
            "genre='{8}', " +
            "company='{9}', " +
            "visible='{10}', " +
            "favourite='{11}', " +
            "LaunchFile='{12}', " +
            "emuprofile={13}, " +
            "infochecked='{14}', " +
            "hash='{15}', " +
            "videopreview='{16}', " +
            "currentdisc={17}, " +
            "arguments='{18}' " +
            "WHERE path='{0}'";

        object[] DBParams
        {
            get
            {
                return new object[] 
                { 
                    DB.Encode(path),
                    parentEmulator.UID,
                    DB.Encode(title),
                    grade,
                    playcount,
                    yearmade,
                    latestplay.ToString("s"),
                    DB.Encode(description),
                    DB.Encode(genre),
                    DB.Encode(company),
                    visible,
                    favourite,
                    DB.Encode(launchFile),
                    selectedProfileId,
                    isInfoChecked,
                    hash,
                    DB.Encode(videoPreview),
                    currentDiscNum,
                    DB.Encode(arguments)
                };
            }
        }

        public static Game CreateGame(SQLite.NET.SQLiteResultSet.Row sqlRow)
        {
            return CreateGame(sqlRow, null);
        }

        public static Game CreateGame(SQLite.NET.SQLiteResultSet.Row sqlRow, Emulator parentEmulator)
        {
            if (sqlRow.fields.Count != 20)
            {
                Logger.LogError("Unable to create Game, invalid database row");
                return null;
            }

            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

            if (parentEmulator == null)
            {
                parentEmulator = DB.Instance.GetEmulator(int.Parse(sqlRow.fields[2], culture));
                if (parentEmulator == null)
                    return null;
            }

            Game game = new Game(DB.Decode(sqlRow.fields[1]), parentEmulator);
            game.GameID = int.Parse(sqlRow.fields[0], culture);
            game.Title = DB.Decode(sqlRow.fields[3]);
            game.Grade = int.Parse(sqlRow.fields[4], culture);
            game.Playcount = int.Parse(sqlRow.fields[5], culture);
            game.Yearmade = int.Parse(sqlRow.fields[6], culture);
            game.Latestplay = DateTime.Parse(sqlRow.fields[7], culture);
            game.Description = DB.Decode(sqlRow.fields[8]);
            game.Genre = DB.Decode(sqlRow.fields[9]);
            game.Company = DB.Decode(sqlRow.fields[10]);
            game.Visible = Boolean.Parse(sqlRow.fields[11]);
            game.Favourite = Boolean.Parse(sqlRow.fields[12]);
            game.LaunchFile = DB.Decode(sqlRow.fields[13]);
            game.SelectedProfileId = int.Parse(sqlRow.fields[14], culture);
            game.IsInfoChecked = Boolean.Parse(sqlRow.fields[15]);
            game.Hash = DB.Decode(sqlRow.fields[16]);
            game.VideoPreview = DB.Decode(sqlRow.fields[17]);
            game.CurrentDiscNum = int.Parse(sqlRow.fields[18], culture);
            if (game.CurrentDiscNum < 1)
                game.currentDiscNum = 1;
            game.Arguments = DB.Decode(sqlRow.fields[19]);
            return game;
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

        public string GetDBInsertOrIgnoreString()
        {
            return "INSERT OR IGNORE INTO " + TABLE_NAME + " VALUES(" + string.Format(System.Globalization.CultureInfo.InvariantCulture, INSERT_STRING, DBParams) + ")";
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
                    DB.Instance.Execute(GetDBUpdateString());
                }
                else
                {
                    if (MediaPortal.Database.DatabaseUtility.GetAsInt(DB.Instance.ExecuteWithoutLock("SELECT COUNT(gameid) FROM {0}", TABLE_NAME), 0, 0) == 0)
                    {
                        DB.Instance.ExecuteWithoutLock("DELETE FROM sqlite_sequence WHERE name='{0}'", TABLE_NAME);
                        gameid = 1;
                    }
                    else
                    {
                        SQLite.NET.SQLiteResultSet result = DB.Instance.ExecuteWithoutLock("SELECT seq + 1 FROM sqlite_sequence WHERE name='{0}'", TABLE_NAME);
                        if (result.Rows.Count > 0)
                            gameid = MediaPortal.Database.DatabaseUtility.GetAsInt(result, 0, 0);
                        else
                            gameid = 1;
                    }
                    DB.Instance.ExecuteWithoutLock(GetDBInsertString());
                    DeleteThumbs();
                }
            }
        }

        public void SaveGameDetails()
        {
            DB.Instance.Execute("UPDATE {0} SET title='{1}', company='{2}', description='{3}', genre='{4}', yearmade={5}, grade={6}, infochecked='{7}' WHERE path='{8}'",
                TABLE_NAME,
                DB.Encode(title), DB.Encode(company), DB.Encode(description), DB.Encode(genre), yearmade, grade, isInfoChecked, DB.Encode(path)
                );
        }

        public void UpdateAndSaveGamePlayInfo()
        {
            this.latestplay = DateTime.Now;
            this.playcount++;
            SaveGamePlayInfo();
        }

        public void SaveGamePlayInfo()
        {
            DB.Instance.Execute("UPDATE {0} SET grade={1}, playcount={2}, latestplay='{3}', favourite='{4}', LaunchFile='{5}', currentdisc={6}, emuprofile={7} WHERE path='{8}'",
                TABLE_NAME,
                grade, playcount, latestplay.ToString("s"), favourite, DB.Encode(launchFile), currentDiscNum, selectedProfileId, DB.Encode(path)
                );
        }

        public void SaveInfoCheckedStatus()
        {
            DB.Instance.Execute("UPDATE {0} SET infochecked='{1}' WHERE path='{2}'", TABLE_NAME, isInfoChecked, DB.Encode(path));
        }

        public void Delete()
        {
            if (gameid < 0)
                return;

            lock (DB.Instance.SyncRoot)
            {
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE gameid={1}", TABLE_NAME, gameid); 
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE game_id={1}", EmulatorProfile.TABLE_NAME, gameid);
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE gameid={1}", GameDisc.TABLE_NAME, gameid); //Delete associated discs
                DeleteThumbs();
            }
        }

        public bool Exists()
        {
            return Exists(path);
        }

        public static bool Exists(string gamePath)
        {
            return DB.Instance.Execute("SELECT 1 FROM {0} WHERE path='{1}'", TABLE_NAME, DB.Encode(gamePath)).Rows.Count > 0;
        }
        #endregion
        
        public string SearchTitle { get; set; }
    }
}
