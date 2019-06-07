using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    internal partial class Conf_Groups : ContentPanel
    {
        ListViewItem selectedListItem = null;
        RomGroup selectedGroup = null;

        TreeNode selectedTreeNode = null;
        GroupItemInfo selectedGroupItem = null;

        bool groupsLoaded = false;
        List<RomGroup> allGroups = null;

        Dictionary<int, Game> allGames = new Dictionary<int, Game>();
        Dictionary<int, Emulator> allEmulators = new Dictionary<int, Emulator>();
        Dictionary<string, ListItemProperty> sortDropdownItems = null;
        Dictionary<string, int> layoutDropdownItems = null;
        List<string> dynamicColumns = new List<string>();

        bool allowGroupItemUpdate = true;
        bool updateGroupItem = false;

        public Conf_Groups()
        {
            InitializeComponent();
                        
            dynamicColumns.AddRange(new string[] { "company", "genre", "grade", "playcount", "yearmade" });
            foreach (string s in dynamicColumns)
                dyn_ColumnComboBox.Items.Add(s);

            newItemTypeComboBox.SelectedIndex = 0;
            getSortDropdownItems();
            getLayoutDropdownItems();
            groupsListView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(groupsListView_ItemSelectionChanged);
            txt_SQL.TextChanged += new EventHandler(groupItemChanged);
            dyn_ColumnComboBox.SelectedIndexChanged += new EventHandler(groupItemChanged);
            dyn_OrderComboBox.SelectedIndexChanged += new EventHandler(groupItemChanged);
        }
        
        #region Groups List
        
        void groupsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
                return;

            updateButtons();
            updateSelectedGroupItem();
            selectedGroupItem = null;
            updateSelectedGroup();
            selectedGroup = null;
            selectedListItem = null;

            allowGroupItemUpdate = false;
            resetSQLGroup();
            resetDynamicGroup();

            if (groupsListView.SelectedItems.Count > 1)
                return;

            selectedListItem = e.Item;
            selectedGroup = e.Item.Tag as RomGroup;
            if (selectedGroup == null)
            {
                selectedListItem = null;
                return;
            }

            txt_Title.Text = selectedGroup.Title;

            sortDescCheckBox.Checked = selectedGroup.SortDescending;
            bool found = false;
            foreach (KeyValuePair<string, ListItemProperty> keyVal in sortDropdownItems)
            {
                if (keyVal.Value == selectedGroup.SortProperty)
                {
                    found = true;
                    sortComboBox.SelectedItem = keyVal.Key;
                    break;
                }
            }
            if (!found)
                sortComboBox.SelectedIndex = 0;

            found = false;
            foreach (KeyValuePair<string, int> keyVal in layoutDropdownItems)
            {
                if (keyVal.Value == selectedGroup.Layout)
                {
                    found = true;
                    layoutComboBox.SelectedItem = keyVal.Key;
                    break;
                }
            }
            if (!found)
                layoutComboBox.SelectedIndex = 0;

            groupItemsTreeView.Nodes.Clear();

            foreach (GroupItemInfo item in selectedGroup.GroupItemInfos)
            {
                TreeNode newNode = new TreeNode() { Tag = item };
                if (item.SQL != null)
                {
                    if (string.IsNullOrEmpty(item.Column))
                        newNode.Text = "SQL";
                    else
                        newNode.Text = "Dynamic";

                    newNode.Nodes.Add("Loading...");
                }
                else if (item.Emulator)
                {
                    Emulator emu;
                    if (item.Id == -2)
                    {
                        newNode.Text = "All Emulators";
                        newNode.Nodes.Add("Loading...");
                    }
                    else if (allEmulators.TryGetValue(item.Id, out emu))
                        newNode.Text = emu.Title;
                    else
                        continue;
                }
                else
                {
                    Game game;
                    if (item.Id == -2)
                    {
                        newNode.Text = "All Games";
                        newNode.Nodes.Add("Loading...");
                    }
                    else if (allGames.TryGetValue(item.Id, out game))
                        newNode.Text = game.Title;
                    else
                        continue;
                }
                groupItemsTreeView.Nodes.Add(newNode);
            }

            allowGroupItemUpdate = true;
        }

        #endregion

        #region Update

        public override void UpdatePanel()
        {
            allowGroupItemUpdate = false;

            allGames.Clear();
            foreach (Game game in DB.Instance.GetGames())
                allGames.Add(game.GameID, game);

            allEmulators.Clear();
            foreach (Emulator emu in DB.Instance.GetEmulators())
                allEmulators.Add(emu.UID, emu);

            if (itemTypeComboBox.SelectedIndex < 0)
                itemTypeComboBox.SelectedIndex = 0;
            else
                itemTypeComboBox_SelectedIndexChanged(itemTypeComboBox, new EventArgs());

            if (!groupsLoaded)
            {
                groupsLoaded = true;
                allGroups = GroupHandler.Instance.LoadGroups();
                groupsListView.Items.Clear();

                foreach (RomGroup group in allGroups)
                {
                    groupsListView.Items.Add(new ListViewItem(group.Title) { Tag = group });
                }
                if (groupsListView.Items.Count > 0)
                {
                    groupsListView.SelectedItems.Clear();
                    groupsListView.Items[0].Selected = true;
                }
                columnHeader1.Width = -1;
            }

            base.UpdatePanel();
            allowGroupItemUpdate = true;
        }

        void updateSelectedGroup()
        {
            if (selectedGroup == null)
                return;

            selectedGroup.Title = txt_Title.Text;
            if (selectedListItem != null)
                selectedListItem.Text = txt_Title.Text;

            if (sortDropdownItems.ContainsKey(sortComboBox.Text))
                selectedGroup.SortProperty = sortDropdownItems[sortComboBox.Text];
            selectedGroup.SortDescending = sortDescCheckBox.Checked;
            if (layoutDropdownItems.ContainsKey(layoutComboBox.Text))
                selectedGroup.Layout = layoutDropdownItems[layoutComboBox.Text];

            selectedGroup.GroupItemInfos.Clear();
            foreach (TreeNode node in groupItemsTreeView.Nodes)
            {
                GroupItemInfo info = node.Tag as GroupItemInfo;
                if (info != null)
                    selectedGroup.GroupItemInfos.Add(info);
            }
        }

        void updateSelectedGroupItem()
        {
            if (!updateGroupItem || selectedGroupItem == null || selectedTreeNode == null || selectedGroupItem.SQL == null)
                return;

            if (!string.IsNullOrEmpty(selectedGroupItem.Column))
            {
                selectedGroupItem.Column = dyn_ColumnComboBox.Text;
                selectedGroupItem.Order = dyn_OrderComboBox.Text;
                selectedGroupItem.SQL = "";
            }
            else
                selectedGroupItem.SQL = txt_SQL.Text;

            addChildNodes(selectedTreeNode, selectedGroupItem);
            updateGroupItem = false;
        }

        #endregion

        #region DropDowns

        private void itemTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            allItemsListView.Items.Clear();
            if (itemTypeComboBox.SelectedIndex == 0)
            {
                allItemsListView.Items.Add(new ListViewItem("All Emulators") { Tag = new GroupItemInfo(-2, true) });
                foreach (Emulator emu in allEmulators.Values)
                    allItemsListView.Items.Add(new ListViewItem(emu.Title) { Tag = new GroupItemInfo(emu.UID, true) });
            }
            else
            {
                allItemsListView.Items.Add(new ListViewItem("All Games") { Tag = new GroupItemInfo(-2) });
                foreach (Game game in allGames.Values)
                    allItemsListView.Items.Add(new ListViewItem(game.Title) { Tag = new GroupItemInfo(game.GameID) });
            }
        }

        void getSortDropdownItems()
        {
            sortDropdownItems = new Dictionary<string, ListItemProperty>();
            sortDropdownItems.Add(Translator.Instance.defaultsort, ListItemProperty.DEFAULT);
            sortDropdownItems.Add(Translator.Instance.title, ListItemProperty.TITLE);
            sortDropdownItems.Add(Translator.Instance.developer, ListItemProperty.COMPANY);
            sortDropdownItems.Add(Translator.Instance.grade, ListItemProperty.GRADE);
            sortDropdownItems.Add(Translator.Instance.lastplayed, ListItemProperty.LASTPLAYED);
            sortDropdownItems.Add(Translator.Instance.playcount, ListItemProperty.PLAYCOUNT);
            sortDropdownItems.Add(Translator.Instance.year, ListItemProperty.YEAR);

            sortComboBox.Items.Clear();
            foreach (string key in sortDropdownItems.Keys)
                sortComboBox.Items.Add(key);

        }

        void getLayoutDropdownItems()
        {
            layoutDropdownItems = new Dictionary<string, int>();
            layoutDropdownItems.Add(Translator.Instance.layoutlastused, -1);
            layoutDropdownItems.Add(Translator.Instance.layoutlist, 0);
            layoutDropdownItems.Add(Translator.Instance.layouticons, 1);
            layoutDropdownItems.Add(Translator.Instance.layoutlargeicons, 2);
            layoutDropdownItems.Add(Translator.Instance.layoutfilmstrip, 3);
            layoutDropdownItems.Add(Translator.Instance.layoutcoverflow, 4);

            layoutComboBox.Items.Clear();
            foreach (string key in layoutDropdownItems.Keys)
                layoutComboBox.Items.Add(key);
        }

        #endregion

        void groupItemChanged(object sender, EventArgs e)
        {
            if (!allowGroupItemUpdate)
                return;
            updateGroupItem = true;
        }

        void resetSQLGroup()
        {
            txt_SQL.Text = "";
            sqlGroupBox.Enabled = false;
        }

        void resetDynamicGroup()
        {
            dyn_ColumnComboBox.SelectedIndex = 0;
            dyn_OrderComboBox.SelectedIndex = 0;
            dynamicGroupBox.Enabled = false;
        }

        #region Buttons

        private void addToGroupButton_Click(object sender, EventArgs e)
        {
            updateSelectedGroupItem();
            foreach (ListViewItem item in allItemsListView.SelectedItems)
            {
                GroupItemInfo info = item.Tag as GroupItemInfo;
                if (info == null)
                    continue;
                TreeNode newNode = new TreeNode(item.Text) { Tag = info };
                if (info.Id == -2)
                    newNode.Nodes.Add("Loading...");
                groupItemsTreeView.Nodes.Add(newNode);
            }
        }

        private void delFromGroupButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = groupItemsTreeView.SelectedNode;
            if (selectedNode == null)
                return;
            if (selectedNode.Parent != null)
                selectedNode = selectedNode.Parent;

            groupItemsTreeView.Nodes.Remove(selectedNode);
        }

        private void groupItemUpButton_Click(object sender, EventArgs e)
        {
            updateSelectedGroupItem();
            TreeNode selectedNode = groupItemsTreeView.SelectedNode;
            if (selectedNode == null)
                return;
            if (selectedNode.Parent != null)
                selectedNode = selectedNode.Parent;

            int index = selectedNode.Index;
            if (index < 1)
                return;
            groupItemsTreeView.Nodes.RemoveAt(index);
            groupItemsTreeView.Nodes.Insert(index - 1, selectedNode);
            groupItemsTreeView.SelectedNode = selectedNode;
        }

        private void groupItemDownButton_Click(object sender, EventArgs e)
        {
            updateSelectedGroupItem();
            TreeNode selectedNode = groupItemsTreeView.SelectedNode;
            if (selectedNode == null)
                return;
            if (selectedNode.Parent != null)
                selectedNode = selectedNode.Parent;

            int index = selectedNode.Index;
            if (index > groupItemsTreeView.Nodes.Count - 2)
                return;
            groupItemsTreeView.Nodes.RemoveAt(index);
            groupItemsTreeView.Nodes.Insert(index + 1, selectedNode);
            groupItemsTreeView.SelectedNode = selectedNode;
        }

        private void newGroupItemButton_Click(object sender, EventArgs e)
        {
            updateSelectedGroupItem();
            if (selectedGroup == null)
                return;

            string title = "SQL";
            GroupItemInfo info = new GroupItemInfo("");
            if (newItemTypeComboBox.Text == "Dynamic")
            {
                title = "Dynamic";
                info.Column = dynamicColumns[0];
            }
            TreeNode newNode = new TreeNode(title) { Tag = info };
            newNode.Nodes.Add("Loading...");
            groupItemsTreeView.Nodes.Add(newNode);
            groupItemsTreeView.SelectedNode = newNode;
        }

        #endregion

        #region Group Items TreeView

        private void groupItemsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateGroupItemButtons();
            allowGroupItemUpdate = false;
            updateSelectedGroupItem();
            resetSQLGroup();
            resetDynamicGroup();

            if (selectedGroup == null)
                return;

            TreeNode selectedNode = groupItemsTreeView.SelectedNode;
            if (selectedNode == null)
                return;

            selectedGroupItem = selectedNode.Tag as GroupItemInfo;
            if (selectedGroupItem == null)
            {
                selectedTreeNode = null;
                return;
            }
            selectedTreeNode = selectedNode;            

            if (selectedGroupItem.SQL != null)
            {
                if (string.IsNullOrEmpty(selectedGroupItem.Column))
                {
                    sqlGroupBox.Enabled = true;
                    txt_SQL.Text = selectedGroupItem.SQL;
                }
                else
                {
                    dynamicGroupBox.Enabled = true;
                    dyn_ColumnComboBox.SelectedItem = selectedGroupItem.Column;
                    if (selectedGroupItem.Order == "DESC")
                        dyn_OrderComboBox.SelectedIndex = 1;
                    else
                        dyn_OrderComboBox.SelectedIndex = 0;
                }
            }
            allowGroupItemUpdate = true;
        }

        private void groupItemsTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            updateSelectedGroupItem();
            if (selectedGroup == null)
                return;

            TreeNode selectedNode = e.Node;
            if (selectedNode == null)
                return;
            
            GroupItemInfo info = selectedNode.Tag as GroupItemInfo;
            if (info == null)
                return;

            if (selectedNode.Nodes.Count == 1 && selectedNode.Nodes[0].Text == "Loading...")
            {
                addChildNodes(selectedNode, info);
            }
        }

        void addChildNodes(TreeNode selectedNode, GroupItemInfo info)
        {
            selectedNode.Nodes.Clear();

            if (info.SQL != null)
            {
                if (string.IsNullOrEmpty(info.Column))
                {
                    foreach (Game game in DB.Instance.GetGames(info.SQL, false))
                        selectedNode.Nodes.Add(new TreeNode(game.Title) { BackColor = Color.DarkGray });
                }
                else
                {
                    foreach (RomGroup group in selectedGroup.GetSubGroups(info))
                        selectedNode.Nodes.Add(new TreeNode(group.Title) { BackColor = Color.DarkGray });
                }
            }
            else if (info.Id == -2)
            {
                if (info.Emulator)
                {
                    foreach (Emulator emu in DB.Instance.GetEmulators())
                        selectedNode.Nodes.Add(new TreeNode(emu.Title) { BackColor = Color.LightGray });
                }
                else
                {
                    foreach (Game game in DB.Instance.GetGames())
                        selectedNode.Nodes.Add(new TreeNode(game.Title) { BackColor = Color.LightGray });
                }
            }

            if (selectedNode.Nodes.Count < 1)
                selectedNode.Nodes.Add("No items");
        }

        #endregion

        private void newGroupButton_Click(object sender, EventArgs e)
        {
            string title = "New Group";
            ListViewItem newItem = new ListViewItem(title) { Tag = new RomGroup(title) };
            groupsListView.SelectedItems.Clear();
            groupsListView.Items.Add(newItem);
            newItem.Selected = true;
            txt_Title.Focus();
        }

        private void delGroupButton_Click(object sender, EventArgs e)
        {
            if (groupsListView.SelectedItems.Count < 1)
                return;

            DialogResult dlgRes = MessageBox.Show("Are you sure you want to delete the selected groups?", "Delete Groups", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dlgRes != DialogResult.OK)
                return;
            int selectedIndex = groupsListView.SelectedIndices[0];
            while (groupsListView.SelectedItems.Count > 0)
                groupsListView.Items.Remove(groupsListView.SelectedItems[0]);

            if (groupsListView.Items.Count < 1)
                return;

            if (selectedIndex > groupsListView.Items.Count - 1)
                selectedIndex = groupsListView.Items.Count - 1;

            groupsListView.SelectedItems.Clear();
            groupsListView.Items[selectedIndex].Selected = true;
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (groupsListView.SelectedIndices.Count != 1)
                return;

            int index = groupsListView.SelectedIndices[0];
            if (index == 0)
                return;
            ListViewItem item = groupsListView.Items[index];
            groupsListView.Items.RemoveAt(index);
            groupsListView.Items.Insert(index - 1, item);
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (groupsListView.SelectedIndices.Count != 1)
                return;

            int index = groupsListView.SelectedIndices[0];
            if (index == groupsListView.Items.Count - 1)
                return;
            ListViewItem item = groupsListView.Items[index];
            groupsListView.Items.RemoveAt(index);
            groupsListView.Items.Insert(index + 1, item);
        }

        void updateButtons()
        {
            upButton.Enabled = false;
            downButton.Enabled = false;
            if (groupsListView.SelectedIndices.Count == 1)
            {
                if (groupsListView.SelectedIndices[0] > 0)
                    upButton.Enabled = true;
                if (groupsListView.SelectedIndices[0] < groupsListView.Items.Count - 1)
                    downButton.Enabled = true;
            }

        }

        void updateGroupItemButtons()
        {
            groupItemUpButton.Enabled = false;
            groupItemDownButton.Enabled = false;
            TreeNode selectedNode = groupItemsTreeView.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Parent != null)
                    selectedNode = selectedNode.Parent;
                if (selectedNode.Index > 0)
                    groupItemUpButton.Enabled = true;
                if (selectedNode.Index < groupItemsTreeView.Nodes.Count - 1)
                    groupItemDownButton.Enabled = true;
            }
        }

        public override void ClosePanel()
        {
            if (!groupsLoaded)
                return;

            updateSelectedGroupItem();
            updateSelectedGroup();
            List<RomGroup> groups = new List<RomGroup>();
            foreach (ListViewItem item in groupsListView.Items)
            {
                RomGroup group = item.Tag as RomGroup;
                if (group != null)
                    groups.Add(group);
            }

            GroupHandler.Instance.SaveGroups(groups);

            base.ClosePanel();
        }
    }
}
