namespace myEmulators
{
    partial class Configuration
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Emulators");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("PC games");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Importer");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Import/export");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Backup/restore database");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Database options");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Game database", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Readme");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("FAQ");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Documentation", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Options");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuration));
            this.save = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.navigationTree = new System.Windows.Forms.TreeView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainContents = new myEmulators.ContentPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(805, 638);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 5;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            this.save.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(724, 638);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 4;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            this.apply.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(12, 638);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            this.cancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            // 
            // navigationTree
            // 
            this.navigationTree.Location = new System.Drawing.Point(12, 89);
            this.navigationTree.Name = "navigationTree";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Emulators";
            treeNode2.Name = "Node1";
            treeNode2.Text = "PC games";
            treeNode3.Name = "Importer";
            treeNode3.Text = "Importer";
            treeNode4.Name = "Node0";
            treeNode4.Text = "Import/export";
            treeNode5.Name = "Node10";
            treeNode5.Text = "Backup/restore database";
            treeNode6.Name = "Node11";
            treeNode6.Text = "Database options";
            treeNode7.Name = "Node2";
            treeNode7.Text = "Game database";
            treeNode8.Name = "Node5";
            treeNode8.Text = "Readme";
            treeNode9.Name = "Node6";
            treeNode9.Text = "FAQ";
            treeNode10.Name = "Node4";
            treeNode10.Text = "Documentation";
            treeNode11.Name = "Node3";
            treeNode11.Text = "Options";
            this.navigationTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode7,
            treeNode10,
            treeNode11});
            this.navigationTree.Size = new System.Drawing.Size(137, 309);
            this.navigationTree.TabIndex = 1;
            this.navigationTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.navigationTree_AfterSelect);
            this.navigationTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(29)))), ((int)(((byte)(72)))));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(918, 71);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Location = new System.Drawing.Point(11, 631);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(869, 1);
            this.panel1.TabIndex = 6;
            // 
            // mainContents
            // 
            this.mainContents.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainContents.form = null;
            this.mainContents.Location = new System.Drawing.Point(155, 89);
            this.mainContents.Name = "mainContents";
            this.mainContents.Size = new System.Drawing.Size(775, 536);
            this.mainContents.TabIndex = 2;
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 673);
            this.Controls.Add(this.mainContents);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.navigationTree);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.save);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "Configuration";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Will be replaced";
            this.Load += new System.EventHandler(this.Configuration_Load);
            this.Resize += new System.EventHandler(this.Configuration_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private ContentPanel mainContents;
        public System.Windows.Forms.TreeView navigationTree;
    }
}