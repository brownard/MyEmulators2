using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    class GroupHandler
    {
        static object instanceSync = new object();
        object syncRoot = new object();

        static GroupHandler instance = null;
        public static GroupHandler Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceSync)
                        if (instance == null)
                            instance = new GroupHandler();
                return instance;
            }
        }

        List<RomGroup> groups = null;
        Dictionary<string, RomGroup> groupNameDict = null;

        public void Init()
        {
            lock (syncRoot)
                init();
        }

        public List<RomGroup> Groups
        {
            get
            {
                lock (syncRoot)
                {
                    if (groups == null)
                        init();
                    return groups;
                }
            }
        }

        public Dictionary<string, RomGroup> GroupNames
        {
            get
            {
                lock (syncRoot)
                {
                    if (groupNameDict == null)
                        init();
                    return groupNameDict;
                }
            }
        }

        void init()
        {
            groups = LoadGroups();
            groupNameDict = new Dictionary<string, RomGroup>();

            foreach (RomGroup group in groups)
            {
                if (!groupNameDict.ContainsKey(group.Title))
                    groupNameDict.Add(group.Title.ToLower(), group);
            }
        }

        public List<RomGroup> LoadGroups()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            string xmlPath = System.IO.Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config), "Emulators2Groups.xml");

            bool loaded = false;
            bool error = false;

            if (System.IO.File.Exists(xmlPath))
            {
                try
                {
                    doc.Load(xmlPath);
                    loaded = true;
                }
                catch (Exception ex)
                {
                    loaded = false;
                    error = true;
                    Logger.LogError("Error loading Groups xml from location '{0}' - {1}", xmlPath, ex.Message);
                }
            }

            if (!loaded)
            {
                using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MyEmulators2.Data.Emulators2Groups.xml"))
                {
                    doc.Load(stream);
                }
                if (!error)
                {
                    try
                    {
                        doc.Save(xmlPath);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Unable to save Group xml to location '{0}' - {1}", xmlPath, ex.Message);
                    }
                }
            }

            List<RomGroup> groups = new List<RomGroup>();
            foreach (System.Xml.XmlNode node in doc.GetElementsByTagName("Group"))
            {
                RomGroup group = new RomGroup(node);
                if (group.IsReady) 
                    groups.Add(group);
            }
            return groups;
        }

        public void SaveGroups(List<RomGroup> groups)
        {
            if (groups == null)
                return;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlElement groupsEl = doc.CreateElement("Groups");
            doc.AppendChild(groupsEl);
            foreach (RomGroup group in groups)
                groupsEl.AppendChild(group.GetXML(doc));

            string xmlPath = System.IO.Path.Combine(MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Config), "Emulators2Groups.xml");

            try
            {
                doc.Save(xmlPath);
            }
            catch (Exception ex)
            {
                Logger.LogError("Unable to save Group xml to location '{0}' - {1}", xmlPath, ex.Message);
            }
        }

        #region Random Artwork

        object artworkCacheSync = new object();
        Dictionary<string, DBItem> sqlRandomArtwork = new Dictionary<string, DBItem>();
        Dictionary<int, DBItem> emuRandomArtwork = new Dictionary<int, DBItem>();
        Dictionary<int, DBItem> gameRandomArtwork = new Dictionary<int, DBItem>();
        static System.Text.RegularExpressions.Regex orderByRegEx = new System.Text.RegularExpressions.Regex(@"\bORDER BY\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        public void ResetThumbCache()
        {
            lock (artworkCacheSync)
            {
                sqlRandomArtwork.Clear();
                emuRandomArtwork.Clear();
                gameRandomArtwork.Clear();
            }
        }

        public DBItem GetRandomThumbItem(GroupItemInfo info)
        {
            DBItem thumbItem = null;
            if (!string.IsNullOrEmpty(info.Column))
                return null;
            else if (info.SQL != null)
            {
                lock (artworkCacheSync)
                {
                    if (sqlRandomArtwork.ContainsKey(info.SQL))
                        return sqlRandomArtwork[info.SQL];
                }
                string sql = info.SQL;
                int orderbyIndex = orderByRegEx.Match(sql).Index;
                if (orderbyIndex > -1)
                    sql = sql.Substring(0, orderbyIndex).Trim();
                sql += " ORDER BY RANDOM() LIMIT 1";
                List<Game> games = DB.Instance.GetGames(sql, false);
                if (games.Count > 0)
                {
                    thumbItem = games[0];
                    lock (artworkCacheSync)
                        sqlRandomArtwork[info.SQL] = thumbItem;
                }
            }
            else if (info.Emulator)
            {
                lock (artworkCacheSync)
                    if (emuRandomArtwork.ContainsKey(info.Id))
                        return emuRandomArtwork[info.Id];

                if (info.Id == -2)
                {
                    List<Emulator> emus = DB.Instance.GetEmulators();
                    if (emus.Count > 0)
                        thumbItem = emus[new Random().Next(emus.Count)];
                }
                else
                {
                    thumbItem = DB.Instance.GetEmulator(info.Id);
                }
                if (thumbItem != null)
                    lock (artworkCacheSync)
                        emuRandomArtwork[info.Id] = thumbItem;
            }
            else
            {
                lock (artworkCacheSync)
                    if (gameRandomArtwork.ContainsKey(info.Id))
                        return gameRandomArtwork[info.Id];

                if (info.Id == -2)
                {
                    List<Game> games = DB.Instance.GetGames("ORDER BY RANDOM() LIMIT 1", false);
                    if (games.Count > 0)
                        thumbItem = games[0];
                }
                else
                {
                    thumbItem = DB.Instance.GetGame(info.Id);
                }
                if (thumbItem != null)
                    lock (artworkCacheSync)
                        gameRandomArtwork[info.Id] = thumbItem;
            }
            return thumbItem;
        }

        #endregion
    }
}
