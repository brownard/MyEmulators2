using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public class GameDisc : DBInterface, IComparable<GameDisc>
    {
        protected GameDisc() { }

        public GameDisc(Game game)
        {
            gameId = game.GameID;
            emuId = game.ParentEmulator.UID;
        }

        int uid = -1;
        public int Uid
        {
            get { return uid; }
        }

        int gameId = -1;
        public int GameId
        {
            get { return gameId; }
        }

        int emuId = -2;
        public int EmuId
        {
            get { return emuId; }
        }

        int number = 1;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        string path = null;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        string launchFile = null;
        public string LaunchFile
        {
            get { return launchFile; }
            set { launchFile = value; }
        }

        public string Name
        {
            get { return string.Format("Disc {0}", number); }
        }

        bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        #region DBInterface Members

        public const string TABLE_NAME = "GameDiscs";

        const string TABLE_STRING =
            "uid int," +
            "gameid int," +
            "emuid int," +
            "path varchar(200)," +
            "discnumber int," +
            "launchfile varchar(200)," +
            "PRIMARY KEY(uid)";

        const string INSERT_STRING =
            "{0}," + //uid
            "{1}," + //gameid
            "{2}," + //emuid
            "'{3}'," + //path
            "{4}," + //number
            "'{5}'"; //launchfile

        const string UPDATE_STRING =
            "gameid={1}, " +
            "emuid={2}, " +
            "path='{3}', " +
            "discnumber={4}, " +
            "launchfile='{5}' " +
            "WHERE uid={0}";

        object[] DBParams
        {
            get
            {
                return new object[] 
                { 
                    uid, 
                    gameId,
                    emuId,
                    DB.Encode(path), 
                    number,
                    DB.Encode(launchFile)
                };
            }
        }

        public static GameDisc CreateGameDisc(SQLite.NET.SQLiteResultSet.Row sqlRow)
        {
            if (sqlRow.fields.Count != 6)
            {
                Logger.LogError("Unable to create Game Disc, invalid database row");
                return null;
            }

            GameDisc disc = new GameDisc();
            disc.uid = int.Parse(sqlRow.fields[0]);
            disc.gameId = int.Parse(sqlRow.fields[1]);
            disc.emuId = int.Parse(sqlRow.fields[2]);
            disc.path = DB.Decode(sqlRow.fields[3]);
            disc.number = int.Parse(sqlRow.fields[4]);
            disc.launchFile = DB.Decode(sqlRow.fields[5]);
            return disc;
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
                }
            }
        }

        public void Delete()
        {
            if (uid < 0)
                return;

            lock (DB.Instance.SyncRoot)
            {
                DB.Instance.ExecuteWithoutLock("DELETE FROM {0} WHERE uid={1}", TABLE_NAME, uid);
            }
        }

        public bool Exists()
        {
            if (uid < 0)
                return false;

            return DB.Instance.Execute("SELECT 1 FROM {0} WHERE uid={1}", TABLE_NAME, uid).Rows.Count > 0;
        }

        #endregion

        #region IComparable<GameDisc> Members

        public int CompareTo(GameDisc other)
        {
            if (other == null)
                return 1;
            return this.number.CompareTo(other.number);
        }

        #endregion
    }
}
