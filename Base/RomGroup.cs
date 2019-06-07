using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyEmulators2
{
    public class RomGroup : DBItem
    {
        List<int> gameIds = new List<int>();
        List<int> emuIds = new List<int>();
        List<DBItem> groupItems = null;

        public RomGroup(string title, string sql = null)
        {
            if (title == null)
                title = "";
            Title = title;

            if (!string.IsNullOrEmpty(sql))
                groupItemInfos.Add(new GroupItemInfo(sql));
        }

        public RomGroup(XmlNode groupNode, bool isDefault = false)
        {
            initFromXml(groupNode);
            IsDefault = isDefault;
        }

        #region Init

        void initFromXml(XmlNode groupNode)
        {
            if (groupNode.Attributes == null)
            {
                Logger.LogError("No attributes found for group xml\r\n{0}", groupNode.InnerText);
                return;
            }

            XmlAttribute title = groupNode.Attributes["title"];
            if (title == null)
            {
                Logger.LogError("No title attribute found for group xml\r\n{0}", groupNode.InnerText);
                return;
            }

            isReady = true;
            Title = title.Value;

            XmlAttribute fav = groupNode.Attributes["favourite"];
            if (fav != null)
            {
                bool favourite;
                if (bool.TryParse(fav.Value, out favourite))
                {
                    if (favourite)
                    {
                        layout = Options.Instance.GetIntOption("viewfavourites");
                        Favourite = favourite;
                    }
                }
                else
                    Logger.LogError("Unable to parse favourite attribute '{0}' to boolean", fav.Value);
            }

            if (groupNode.Attributes["sort"] != null)
                getSortProperty(groupNode.Attributes["sort"].Value);
            if (groupNode.Attributes["desc"] != null)
                bool.TryParse(groupNode.Attributes["desc"].Value, out sortDesc);

            if (layout < 0 && groupNode.Attributes["layout"] != null)
                if (!int.TryParse(groupNode.Attributes["layout"].Value, out layout))
                    layout = -1;

            foreach (XmlNode childNode in groupNode.ChildNodes)
            {
                addQuery(childNode);
            }
        }

        void getSortProperty(string p)
        {
            if (string.IsNullOrEmpty(p))
                return;
            try
            {
                sortProperty = (ListItemProperty)Enum.Parse(typeof(ListItemProperty), p, true);
            }
            catch
            {
                Logger.LogError("Error parsing sort property for group '{0}'", Title);
            }
        }

        void addQuery(XmlNode childNode)
        {
            if (childNode == null || childNode.Name != "item" || childNode.Attributes == null)
                return;

            XmlAttribute attr = childNode.Attributes["type"];
            if (attr == null)
                return;

            int id;
            switch (attr.Value)
            {
                case "SQL":
                    if (!string.IsNullOrEmpty(childNode.InnerText))
                        groupItemInfos.Add(new GroupItemInfo(childNode.InnerText));
                    break;
                case "Game":
                    if (int.TryParse(childNode.InnerText, out id))
                    {
                        groupItemInfos.Add(new GroupItemInfo(id));
                        if (id > -2 && !gameIds.Contains(id))
                            gameIds.Add(id);
                    }
                    break;
                case "Emulator":
                    if (int.TryParse(childNode.InnerText, out id))
                    {
                        groupItemInfos.Add(new GroupItemInfo(id, true));
                        if (id > -2 && !emuIds.Contains(id))
                            emuIds.Add(id);
                    }
                    break;
                case "Dynamic":
                    XmlAttribute column = childNode.Attributes["column"];
                    if (column == null)
                        break;
                    XmlAttribute orderAttr = childNode.Attributes["order"];
                    string order = null;
                    if (orderAttr != null)
                        order = orderAttr.Value;
                    groupItemInfos.Add(new GroupItemInfo(childNode.InnerText) { Column = column.Value, Order = order });
                    break;
            }
        }

        void getItems()
        {
            groupItems = new List<DBItem>();
            Dictionary<int, Emulator> emus = DB.Instance.GetEmulators(emuIds);
            Dictionary<int, Game> games = DB.Instance.GetGames(gameIds);
            foreach (GroupItemInfo info in groupItemInfos)
            {
                if (info.SQL != null)
                {
                    if (string.IsNullOrEmpty(info.Column))
                        groupItems.AddRange(DB.Instance.GetGames(info.SQL, false));
                    else
                        groupItems.AddRange(GetSubGroups(info));
                }
                else if (info.Emulator)
                {
                    if (info.Id == -2)
                        groupItems.AddRange(DB.Instance.GetEmulators());
                    else if (emus.ContainsKey(info.Id))
                        groupItems.Add(emus[info.Id]);
                }
                else
                {
                    if (info.Id == -2)
                        groupItems.AddRange(DB.Instance.GetGames());
                    else if (games.ContainsKey(info.Id))
                        groupItems.Add(games[info.Id]);
                }
            }
        }

        public List<DBItem> GetSubGroups(GroupItemInfo info)
        {
            List<DBItem> groups = new List<DBItem>();

            if (string.IsNullOrEmpty(info.Column))
            {
                Logger.LogError("No column specified for dynamic group");
                return groups;
            }
            if (info.Column == "genre")
            {
                foreach (string genre in DB.Instance.GetGenres())
                {
                    groups.Add(new RomGroup(genre, string.Format(@"WHERE '|' || genre || '|' LIKE '%|{0}|%'", genre)));
                }
                return groups;
            }

            string order = string.IsNullOrEmpty(info.Order) ? "" : info.Order;
            string sql = string.Format("SELECT DISTINCT {0} FROM {1} ORDER BY {0} {2}", info.Column, Game.TABLE_NAME, order).Trim();
            Logger.LogDebug("Created sql for dynamic group '{0}' - {1}", Title, sql);

            SQLite.NET.SQLiteResultSet results = DB.Instance.Execute(sql);
            foreach (SQLite.NET.SQLiteResultSet.Row row in results.Rows)
            {
                try
                {
                    RomGroup newGroup = new RomGroup(row.fields[0] == "" ? Translator.Instance.unknown : row.fields[0], string.Format("WHERE {0}='{1}' ORDER BY title", info.Column, row.fields[0]));
                    newGroup.SortProperty = SortProperty;
                    groups.Add(newGroup);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error adding subgroup - {0}\r\n{1}", ex.Message, ex.StackTrace);
                }
            }

            return groups;
        }

        #endregion

        #region Public Properties

        bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
        }

        string title = "";
        public string Title 
        { 
            get { return title; } 
            set { title = value; } 
        }

        public bool IsDefault { get; protected set; }
        public bool Favourite { get; set; }

        int layout = -1;
        public int Layout 
        { 
            get { return layout; } 
            set { layout = value; } 
        }

        ListItemProperty sortProperty = ListItemProperty.DEFAULT;
        public ListItemProperty SortProperty
        {
            get { return sortProperty; }
            set { sortProperty = value; }
        }

        bool sortDesc = false;
        public bool SortDescending 
        { 
            get { return sortDesc; } 
            set { sortDesc = value; } 
        }

        List<GroupItemInfo> groupItemInfos = new List<GroupItemInfo>();
        public List<GroupItemInfo> GroupItemInfos
        {
            get { return groupItemInfos; }
        }

        public List<DBItem> GroupItems
        {
            get
            {
                if (groupItems == null)
                    getItems();
                return groupItems;
            }
        }

        static System.Text.RegularExpressions.Regex orderByRegEx = new System.Text.RegularExpressions.Regex(@"\bORDER BY\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        bool checkedThumbs = false;
        ThumbGroup thumbs = null;
        public ThumbGroup ThumbGroup
        {
            get
            {
                if (checkedThumbs)
                    return thumbs;
                checkedThumbs = true;

                DBItem thumbItem = null;
                Random r = new Random();
                int index = r.Next(groupItemInfos.Count);
                int tries = 0;
                while (tries < groupItemInfos.Count)
                {
                    if (index == groupItemInfos.Count)
                        index = 0;
                    tries++;
                    thumbItem = GroupHandler.Instance.GetRandomThumbItem(groupItemInfos[index]);
                    if (thumbItem != null)
                        break;
                }

                if (thumbItem != null)
                    thumbs = new ThumbGroup(thumbItem);
                return thumbs;
            }
        }

        #endregion

        #region Public Methods

        public void RefreshThumbs()
        {
            checkedThumbs = false;
            if (thumbs != null)
            {
                thumbs.Dispose();
                thumbs = null;
            }
        }

        public void Refresh()
        {
            groupItems = null;
        }

        public override ExtendedGUIListItem CreateGUIListItem()
        {
            ExtendedGUIListItem item = new ExtendedGUIListItem(Title);
            if (ThumbGroup != null)
                item.ThumbnailImage = ThumbGroup.FrontCoverDefaultPath;
            else
                item.ThumbnailImage = Emulators2Settings.Instance.DefaultLogo;
            item.IsGroup = true;
            item.RomGroup = this;
            item.IsFavourites = Favourite;
            return item;
        }

        public XmlElement GetXML(XmlDocument doc)
        {
            XmlElement group = doc.CreateElement("Group");
            XmlAttribute attr = doc.CreateAttribute("title");
            attr.Value = Title;
            group.Attributes.Append(attr);

            if (Favourite)
            {
                attr = doc.CreateAttribute("favourite");
                attr.Value = "true";
                group.Attributes.Append(attr);
            }

            if (SortProperty != ListItemProperty.NONE)
            {
                attr = doc.CreateAttribute("sort");
                attr.Value = SortProperty.ToString();
                group.Attributes.Append(attr);
            }

            attr = doc.CreateAttribute("desc");
            attr.Value = SortDescending.ToString();
            group.Attributes.Append(attr);

            attr = doc.CreateAttribute("layout");
            attr.Value = layout.ToString();
            group.Attributes.Append(attr);

            foreach (GroupItemInfo info in groupItemInfos)
            {
                XmlElement item = doc.CreateElement("item");
                XmlAttribute typeAttr = doc.CreateAttribute("type");
                item.Attributes.Append(typeAttr);

                if (info.SQL != null)
                {
                    typeAttr.Value = "SQL";
                    if (!string.IsNullOrEmpty(info.Column))
                    {
                        typeAttr.Value = "Dynamic";
                        XmlAttribute columnAttr = doc.CreateAttribute("column");
                        columnAttr.Value = info.Column;
                        item.Attributes.Append(columnAttr);

                        if (!string.IsNullOrEmpty(info.Order))
                        {
                            XmlAttribute orderAttr = doc.CreateAttribute("order");
                            orderAttr.Value = info.Order;
                            item.Attributes.Append(orderAttr);
                        }
                    }
                    if (info.SQL != "")
                        item.AppendChild(doc.CreateCDataSection(info.SQL));
                }
                else
                {
                    if (info.Emulator)
                        typeAttr.Value = "Emulator";
                    else
                        typeAttr.Value = "Game";
                    item.AppendChild(doc.CreateTextNode(info.Id.ToString()));
                }
                group.AppendChild(item);
            }

            return group;
        }

        #endregion
    }    
}
