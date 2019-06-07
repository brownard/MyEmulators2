namespace MyEmulators2
{
    partial class Conf_Groups
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("test");
            this.delFromGroupButton = new System.Windows.Forms.Button();
            this.addToGroupButton = new System.Windows.Forms.Button();
            this.itemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.allItemsListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dynamicGroupBox = new System.Windows.Forms.GroupBox();
            this.dyn_OrderComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dyn_ColumnComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sqlGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_SQL = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.sortComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Title = new System.Windows.Forms.TextBox();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.delGroupButton = new System.Windows.Forms.Button();
            this.newGroupButton = new System.Windows.Forms.Button();
            this.groupsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupItemUpButton = new System.Windows.Forms.Button();
            this.groupItemDownButton = new System.Windows.Forms.Button();
            this.newGroupItemButton = new System.Windows.Forms.Button();
            this.newItemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.groupItemsTreeView = new System.Windows.Forms.TreeView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.layoutComboBox = new System.Windows.Forms.ComboBox();
            this.sortDescCheckBox = new System.Windows.Forms.CheckBox();
            this.dynamicGroupBox.SuspendLayout();
            this.sqlGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // delFromGroupButton
            // 
            this.delFromGroupButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.delFromGroupButton.Location = new System.Drawing.Point(428, 217);
            this.delFromGroupButton.Name = "delFromGroupButton";
            this.delFromGroupButton.Size = new System.Drawing.Size(28, 23);
            this.delFromGroupButton.TabIndex = 124;
            this.delFromGroupButton.Text = "<<";
            this.toolTip1.SetToolTip(this.delFromGroupButton, "Remove from group");
            this.delFromGroupButton.UseVisualStyleBackColor = true;
            this.delFromGroupButton.Click += new System.EventHandler(this.delFromGroupButton_Click);
            // 
            // addToGroupButton
            // 
            this.addToGroupButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.addToGroupButton.Location = new System.Drawing.Point(428, 188);
            this.addToGroupButton.Name = "addToGroupButton";
            this.addToGroupButton.Size = new System.Drawing.Size(28, 23);
            this.addToGroupButton.TabIndex = 122;
            this.addToGroupButton.Text = ">>";
            this.toolTip1.SetToolTip(this.addToGroupButton, "Add to group");
            this.addToGroupButton.UseVisualStyleBackColor = true;
            this.addToGroupButton.Click += new System.EventHandler(this.addToGroupButton_Click);
            // 
            // itemTypeComboBox
            // 
            this.itemTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemTypeComboBox.FormattingEnabled = true;
            this.itemTypeComboBox.Items.AddRange(new object[] {
            "Emulators",
            "Games"});
            this.itemTypeComboBox.Location = new System.Drawing.Point(217, 112);
            this.itemTypeComboBox.Name = "itemTypeComboBox";
            this.itemTypeComboBox.Size = new System.Drawing.Size(131, 21);
            this.itemTypeComboBox.TabIndex = 121;
            this.itemTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.itemTypeComboBox_SelectedIndexChanged);
            // 
            // allItemsListView
            // 
            this.allItemsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allItemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.allItemsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.allItemsListView.Location = new System.Drawing.Point(217, 141);
            this.allItemsListView.Name = "allItemsListView";
            this.allItemsListView.Size = new System.Drawing.Size(207, 163);
            this.allItemsListView.TabIndex = 119;
            this.allItemsListView.UseCompatibleStateImageBehavior = false;
            this.allItemsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 100;
            // 
            // dynamicGroupBox
            // 
            this.dynamicGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dynamicGroupBox.Controls.Add(this.dyn_OrderComboBox);
            this.dynamicGroupBox.Controls.Add(this.label3);
            this.dynamicGroupBox.Controls.Add(this.dyn_ColumnComboBox);
            this.dynamicGroupBox.Controls.Add(this.label2);
            this.dynamicGroupBox.Location = new System.Drawing.Point(217, 417);
            this.dynamicGroupBox.Name = "dynamicGroupBox";
            this.dynamicGroupBox.Size = new System.Drawing.Size(450, 68);
            this.dynamicGroupBox.TabIndex = 114;
            this.dynamicGroupBox.TabStop = false;
            this.dynamicGroupBox.Text = "Dynamic";
            // 
            // dyn_OrderComboBox
            // 
            this.dyn_OrderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dyn_OrderComboBox.FormattingEnabled = true;
            this.dyn_OrderComboBox.Items.AddRange(new object[] {
            "ASC",
            "DESC"});
            this.dyn_OrderComboBox.Location = new System.Drawing.Point(270, 27);
            this.dyn_OrderComboBox.Name = "dyn_OrderComboBox";
            this.dyn_OrderComboBox.Size = new System.Drawing.Size(142, 21);
            this.dyn_OrderComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Order";
            // 
            // dyn_ColumnComboBox
            // 
            this.dyn_ColumnComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dyn_ColumnComboBox.FormattingEnabled = true;
            this.dyn_ColumnComboBox.Location = new System.Drawing.Point(54, 27);
            this.dyn_ColumnComboBox.Name = "dyn_ColumnComboBox";
            this.dyn_ColumnComboBox.Size = new System.Drawing.Size(142, 21);
            this.dyn_ColumnComboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Column";
            // 
            // sqlGroupBox
            // 
            this.sqlGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sqlGroupBox.Controls.Add(this.label1);
            this.sqlGroupBox.Controls.Add(this.txt_SQL);
            this.sqlGroupBox.Location = new System.Drawing.Point(217, 325);
            this.sqlGroupBox.Name = "sqlGroupBox";
            this.sqlGroupBox.Size = new System.Drawing.Size(450, 68);
            this.sqlGroupBox.TabIndex = 113;
            this.sqlGroupBox.TabStop = false;
            this.sqlGroupBox.Text = "SQL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "SELECT * FROM Games";
            // 
            // txt_SQL
            // 
            this.txt_SQL.Location = new System.Drawing.Point(139, 24);
            this.txt_SQL.Name = "txt_SQL";
            this.txt_SQL.Size = new System.Drawing.Size(273, 20);
            this.txt_SQL.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(425, 79);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(26, 13);
            this.label15.TabIndex = 110;
            this.label15.Text = "Sort";
            // 
            // sortComboBox
            // 
            this.sortComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortComboBox.FormattingEnabled = true;
            this.sortComboBox.Items.AddRange(new object[] {
            "Default",
            "Last used"});
            this.sortComboBox.Location = new System.Drawing.Point(460, 74);
            this.sortComboBox.Name = "sortComboBox";
            this.sortComboBox.Size = new System.Drawing.Size(121, 21);
            this.sortComboBox.TabIndex = 109;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(217, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 108;
            this.label11.Text = "Title";
            // 
            // txt_Title
            // 
            this.txt_Title.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Title.Location = new System.Drawing.Point(278, 39);
            this.txt_Title.Name = "txt_Title";
            this.txt_Title.Size = new System.Drawing.Size(389, 20);
            this.txt_Title.TabIndex = 107;
            // 
            // downButton
            // 
            this.downButton.Image = global::MyEmulators2.Properties.Resources.Down;
            this.downButton.Location = new System.Drawing.Point(90, 4);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(28, 23);
            this.downButton.TabIndex = 106;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::MyEmulators2.Properties.Resources.Up;
            this.upButton.Location = new System.Drawing.Point(61, 4);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(28, 23);
            this.upButton.TabIndex = 105;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // delGroupButton
            // 
            this.delGroupButton.Image = global::MyEmulators2.Properties.Resources.cross;
            this.delGroupButton.Location = new System.Drawing.Point(32, 4);
            this.delGroupButton.Name = "delGroupButton";
            this.delGroupButton.Size = new System.Drawing.Size(28, 23);
            this.delGroupButton.TabIndex = 104;
            this.delGroupButton.UseVisualStyleBackColor = true;
            this.delGroupButton.Click += new System.EventHandler(this.delGroupButton_Click);
            // 
            // newGroupButton
            // 
            this.newGroupButton.Image = global::MyEmulators2.Properties.Resources.Add;
            this.newGroupButton.Location = new System.Drawing.Point(3, 4);
            this.newGroupButton.Name = "newGroupButton";
            this.newGroupButton.Size = new System.Drawing.Size(28, 23);
            this.newGroupButton.TabIndex = 103;
            this.newGroupButton.UseVisualStyleBackColor = true;
            this.newGroupButton.Click += new System.EventHandler(this.newGroupButton_Click);
            // 
            // groupsListView
            // 
            this.groupsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.groupsListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupsListView.FullRowSelect = true;
            this.groupsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.groupsListView.HideSelection = false;
            this.groupsListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.groupsListView.Location = new System.Drawing.Point(3, 33);
            this.groupsListView.Name = "groupsListView";
            this.groupsListView.Size = new System.Drawing.Size(208, 486);
            this.groupsListView.TabIndex = 102;
            this.groupsListView.UseCompatibleStateImageBehavior = false;
            this.groupsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 100;
            // 
            // groupItemUpButton
            // 
            this.groupItemUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupItemUpButton.Image = global::MyEmulators2.Properties.Resources.Up;
            this.groupItemUpButton.Location = new System.Drawing.Point(460, 112);
            this.groupItemUpButton.Name = "groupItemUpButton";
            this.groupItemUpButton.Size = new System.Drawing.Size(28, 23);
            this.groupItemUpButton.TabIndex = 117;
            this.groupItemUpButton.UseVisualStyleBackColor = true;
            this.groupItemUpButton.Click += new System.EventHandler(this.groupItemUpButton_Click);
            // 
            // groupItemDownButton
            // 
            this.groupItemDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupItemDownButton.Image = global::MyEmulators2.Properties.Resources.Down;
            this.groupItemDownButton.Location = new System.Drawing.Point(489, 112);
            this.groupItemDownButton.Name = "groupItemDownButton";
            this.groupItemDownButton.Size = new System.Drawing.Size(28, 23);
            this.groupItemDownButton.TabIndex = 118;
            this.groupItemDownButton.UseVisualStyleBackColor = true;
            this.groupItemDownButton.Click += new System.EventHandler(this.groupItemDownButton_Click);
            // 
            // newGroupItemButton
            // 
            this.newGroupItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newGroupItemButton.Image = global::MyEmulators2.Properties.Resources.Add;
            this.newGroupItemButton.Location = new System.Drawing.Point(639, 112);
            this.newGroupItemButton.Name = "newGroupItemButton";
            this.newGroupItemButton.Size = new System.Drawing.Size(28, 25);
            this.newGroupItemButton.TabIndex = 125;
            this.newGroupItemButton.UseVisualStyleBackColor = true;
            this.newGroupItemButton.Click += new System.EventHandler(this.newGroupItemButton_Click);
            // 
            // newItemTypeComboBox
            // 
            this.newItemTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newItemTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.newItemTypeComboBox.FormattingEnabled = true;
            this.newItemTypeComboBox.Items.AddRange(new object[] {
            "SQL",
            "Dynamic"});
            this.newItemTypeComboBox.Location = new System.Drawing.Point(527, 114);
            this.newItemTypeComboBox.Name = "newItemTypeComboBox";
            this.newItemTypeComboBox.Size = new System.Drawing.Size(110, 21);
            this.newItemTypeComboBox.TabIndex = 126;
            // 
            // groupItemsTreeView
            // 
            this.groupItemsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupItemsTreeView.FullRowSelect = true;
            this.groupItemsTreeView.HideSelection = false;
            this.groupItemsTreeView.Location = new System.Drawing.Point(460, 141);
            this.groupItemsTreeView.Name = "groupItemsTreeView";
            this.groupItemsTreeView.ShowLines = false;
            this.groupItemsTreeView.Size = new System.Drawing.Size(207, 163);
            this.groupItemsTreeView.TabIndex = 127;
            this.groupItemsTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.groupItemsTreeView_AfterExpand);
            this.groupItemsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.groupItemsTreeView_AfterSelect);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 129;
            this.label4.Text = "Layout";
            // 
            // layoutComboBox
            // 
            this.layoutComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.layoutComboBox.FormattingEnabled = true;
            this.layoutComboBox.Items.AddRange(new object[] {
            "Last used",
            "List",
            "Small Icons",
            "Large Icons",
            "Filmstrip",
            "Coverflow"});
            this.layoutComboBox.Location = new System.Drawing.Point(278, 76);
            this.layoutComboBox.Name = "layoutComboBox";
            this.layoutComboBox.Size = new System.Drawing.Size(121, 21);
            this.layoutComboBox.TabIndex = 128;
            // 
            // sortDescCheckBox
            // 
            this.sortDescCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sortDescCheckBox.AutoSize = true;
            this.sortDescCheckBox.Location = new System.Drawing.Point(584, 78);
            this.sortDescCheckBox.Name = "sortDescCheckBox";
            this.sortDescCheckBox.Size = new System.Drawing.Size(83, 17);
            this.sortDescCheckBox.TabIndex = 130;
            this.sortDescCheckBox.Text = "Descending";
            this.sortDescCheckBox.UseVisualStyleBackColor = true;
            // 
            // Conf_Groups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.sortDescCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.layoutComboBox);
            this.Controls.Add(this.groupItemsTreeView);
            this.Controls.Add(this.newItemTypeComboBox);
            this.Controls.Add(this.newGroupItemButton);
            this.Controls.Add(this.delFromGroupButton);
            this.Controls.Add(this.addToGroupButton);
            this.Controls.Add(this.itemTypeComboBox);
            this.Controls.Add(this.allItemsListView);
            this.Controls.Add(this.groupItemDownButton);
            this.Controls.Add(this.groupItemUpButton);
            this.Controls.Add(this.dynamicGroupBox);
            this.Controls.Add(this.sqlGroupBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.sortComboBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_Title);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.delGroupButton);
            this.Controls.Add(this.newGroupButton);
            this.Controls.Add(this.groupsListView);
            this.MinimumSize = new System.Drawing.Size(700, 480);
            this.Name = "Conf_Groups";
            this.Size = new System.Drawing.Size(727, 522);
            this.dynamicGroupBox.ResumeLayout(false);
            this.dynamicGroupBox.PerformLayout();
            this.sqlGroupBox.ResumeLayout(false);
            this.sqlGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button delGroupButton;
        private System.Windows.Forms.Button newGroupButton;
        private System.Windows.Forms.ListView groupsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox sortComboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_Title;
        private System.Windows.Forms.GroupBox sqlGroupBox;
        private System.Windows.Forms.TextBox txt_SQL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox dynamicGroupBox;
        private System.Windows.Forms.ComboBox dyn_ColumnComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox dyn_OrderComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView allItemsListView;
        private System.Windows.Forms.ComboBox itemTypeComboBox;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button addToGroupButton;
        private System.Windows.Forms.Button delFromGroupButton;
        private System.Windows.Forms.Button groupItemUpButton;
        private System.Windows.Forms.Button groupItemDownButton;
        private System.Windows.Forms.Button newGroupItemButton;
        private System.Windows.Forms.ComboBox newItemTypeComboBox;
        private System.Windows.Forms.TreeView groupItemsTreeView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox layoutComboBox;
        private System.Windows.Forms.CheckBox sortDescCheckBox;

    }
}