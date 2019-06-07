namespace MyEmulators2
{
    partial class Conf_Main
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
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.emuBrowserTab = new System.Windows.Forms.TabPage();
            this.conf_EmuBrowser1 = new MyEmulators2.Conf_EmuBrowser();
            this.romBrowserTab = new System.Windows.Forms.TabPage();
            this.conf_DBBrowser1 = new MyEmulators2.Conf_DBBrowser();
            this.importerTab = new System.Windows.Forms.TabPage();
            this.conf_DBImporter1 = new MyEmulators2.Conf_DBImporter();
            this.groupsTab = new System.Windows.Forms.TabPage();
            this.conf_Groups1 = new MyEmulators2.Conf_Groups();
            this.backupTab = new System.Windows.Forms.TabPage();
            this.conf_DBBackup1 = new MyEmulators2.Conf_DBBackup();
            this.optionsTab = new System.Windows.Forms.TabPage();
            this.conf_Options_New1 = new MyEmulators2.Conf_Options_New();
            this.mainTabControl.SuspendLayout();
            this.emuBrowserTab.SuspendLayout();
            this.romBrowserTab.SuspendLayout();
            this.importerTab.SuspendLayout();
            this.groupsTab.SuspendLayout();
            this.backupTab.SuspendLayout();
            this.optionsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.emuBrowserTab);
            this.mainTabControl.Controls.Add(this.romBrowserTab);
            this.mainTabControl.Controls.Add(this.importerTab);
            this.mainTabControl.Controls.Add(this.groupsTab);
            this.mainTabControl.Controls.Add(this.backupTab);
            this.mainTabControl.Controls.Add(this.optionsTab);
            this.mainTabControl.ItemSize = new System.Drawing.Size(58, 23);
            this.mainTabControl.Location = new System.Drawing.Point(1, 1);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(709, 562);
            this.mainTabControl.TabIndex = 0;
            // 
            // emuBrowserTab
            // 
            this.emuBrowserTab.Controls.Add(this.conf_EmuBrowser1);
            this.emuBrowserTab.Location = new System.Drawing.Point(4, 27);
            this.emuBrowserTab.Name = "emuBrowserTab";
            this.emuBrowserTab.Padding = new System.Windows.Forms.Padding(3);
            this.emuBrowserTab.Size = new System.Drawing.Size(701, 531);
            this.emuBrowserTab.TabIndex = 0;
            this.emuBrowserTab.Text = "Emulators";
            this.emuBrowserTab.UseVisualStyleBackColor = true;
            // 
            // conf_EmuBrowser1
            // 
            this.conf_EmuBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conf_EmuBrowser1.form = null;
            this.conf_EmuBrowser1.Location = new System.Drawing.Point(3, 3);
            this.conf_EmuBrowser1.MinimumSize = new System.Drawing.Size(699, 480);
            this.conf_EmuBrowser1.Name = "conf_EmuBrowser1";
            this.conf_EmuBrowser1.Size = new System.Drawing.Size(699, 525);
            this.conf_EmuBrowser1.TabIndex = 0;
            // 
            // romBrowserTab
            // 
            this.romBrowserTab.Controls.Add(this.conf_DBBrowser1);
            this.romBrowserTab.Location = new System.Drawing.Point(4, 27);
            this.romBrowserTab.Name = "romBrowserTab";
            this.romBrowserTab.Padding = new System.Windows.Forms.Padding(3);
            this.romBrowserTab.Size = new System.Drawing.Size(701, 531);
            this.romBrowserTab.TabIndex = 1;
            this.romBrowserTab.Text = "Games";
            this.romBrowserTab.UseVisualStyleBackColor = true;
            // 
            // conf_DBBrowser1
            // 
            this.conf_DBBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conf_DBBrowser1.form = null;
            this.conf_DBBrowser1.Location = new System.Drawing.Point(3, 3);
            this.conf_DBBrowser1.MinimumSize = new System.Drawing.Size(655, 522);
            this.conf_DBBrowser1.Name = "conf_DBBrowser1";
            this.conf_DBBrowser1.Size = new System.Drawing.Size(695, 525);
            this.conf_DBBrowser1.TabIndex = 0;
            // 
            // importerTab
            // 
            this.importerTab.Controls.Add(this.conf_DBImporter1);
            this.importerTab.Location = new System.Drawing.Point(4, 27);
            this.importerTab.Name = "importerTab";
            this.importerTab.Padding = new System.Windows.Forms.Padding(3);
            this.importerTab.Size = new System.Drawing.Size(701, 531);
            this.importerTab.TabIndex = 2;
            this.importerTab.Text = "Import";
            this.importerTab.UseVisualStyleBackColor = true;
            // 
            // conf_DBImporter1
            // 
            this.conf_DBImporter1.BackColor = System.Drawing.Color.Transparent;
            this.conf_DBImporter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conf_DBImporter1.form = null;
            this.conf_DBImporter1.Location = new System.Drawing.Point(3, 3);
            this.conf_DBImporter1.Name = "conf_DBImporter1";
            this.conf_DBImporter1.Size = new System.Drawing.Size(695, 525);
            this.conf_DBImporter1.TabIndex = 0;
            // 
            // groupsTab
            // 
            this.groupsTab.Controls.Add(this.conf_Groups1);
            this.groupsTab.Location = new System.Drawing.Point(4, 27);
            this.groupsTab.Name = "groupsTab";
            this.groupsTab.Padding = new System.Windows.Forms.Padding(3);
            this.groupsTab.Size = new System.Drawing.Size(701, 531);
            this.groupsTab.TabIndex = 5;
            this.groupsTab.Text = "Groups";
            this.groupsTab.UseVisualStyleBackColor = true;
            // 
            // conf_Groups1
            // 
            this.conf_Groups1.BackColor = System.Drawing.Color.White;
            this.conf_Groups1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conf_Groups1.form = null;
            this.conf_Groups1.Location = new System.Drawing.Point(3, 3);
            this.conf_Groups1.MinimumSize = new System.Drawing.Size(700, 480);
            this.conf_Groups1.Name = "conf_Groups1";
            this.conf_Groups1.Size = new System.Drawing.Size(700, 525);
            this.conf_Groups1.TabIndex = 0;
            // 
            // backupTab
            // 
            this.backupTab.Controls.Add(this.conf_DBBackup1);
            this.backupTab.Location = new System.Drawing.Point(4, 27);
            this.backupTab.Name = "backupTab";
            this.backupTab.Padding = new System.Windows.Forms.Padding(3);
            this.backupTab.Size = new System.Drawing.Size(701, 531);
            this.backupTab.TabIndex = 4;
            this.backupTab.Text = "Backup/Restore";
            this.backupTab.UseVisualStyleBackColor = true;
            // 
            // conf_DBBackup1
            // 
            this.conf_DBBackup1.BackColor = System.Drawing.Color.White;
            this.conf_DBBackup1.form = null;
            this.conf_DBBackup1.Location = new System.Drawing.Point(1, 0);
            this.conf_DBBackup1.MinimumSize = new System.Drawing.Size(700, 480);
            this.conf_DBBackup1.Name = "conf_DBBackup1";
            this.conf_DBBackup1.Size = new System.Drawing.Size(700, 531);
            this.conf_DBBackup1.TabIndex = 0;
            // 
            // optionsTab
            // 
            this.optionsTab.Controls.Add(this.conf_Options_New1);
            this.optionsTab.Location = new System.Drawing.Point(4, 27);
            this.optionsTab.Name = "optionsTab";
            this.optionsTab.Size = new System.Drawing.Size(701, 531);
            this.optionsTab.TabIndex = 3;
            this.optionsTab.Text = "Options";
            this.optionsTab.UseVisualStyleBackColor = true;
            // 
            // conf_Options_New1
            // 
            this.conf_Options_New1.BackColor = System.Drawing.Color.White;
            this.conf_Options_New1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conf_Options_New1.form = null;
            this.conf_Options_New1.Location = new System.Drawing.Point(0, 0);
            this.conf_Options_New1.MinimumSize = new System.Drawing.Size(700, 480);
            this.conf_Options_New1.Name = "conf_Options_New1";
            this.conf_Options_New1.Size = new System.Drawing.Size(701, 531);
            this.conf_Options_New1.TabIndex = 0;
            // 
            // Conf_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 564);
            this.Controls.Add(this.mainTabControl);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(725, 602);
            this.Name = "Conf_Main";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Emulators 2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Conf_Main_FormClosing);
            this.Load += new System.EventHandler(this.Conf_Main_Load);
            this.mainTabControl.ResumeLayout(false);
            this.emuBrowserTab.ResumeLayout(false);
            this.romBrowserTab.ResumeLayout(false);
            this.importerTab.ResumeLayout(false);
            this.groupsTab.ResumeLayout(false);
            this.backupTab.ResumeLayout(false);
            this.optionsTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage emuBrowserTab;
        private System.Windows.Forms.TabPage romBrowserTab;
        private System.Windows.Forms.TabPage importerTab;
        private Conf_EmuBrowser conf_EmuBrowser1;
        private Conf_DBBrowser conf_DBBrowser1;
        private Conf_DBImporter conf_DBImporter1;
        private System.Windows.Forms.TabPage optionsTab;
        private Conf_Options_New conf_Options_New1;
        private System.Windows.Forms.TabPage backupTab;
        private Conf_DBBackup conf_DBBackup1;
        private System.Windows.Forms.TabPage groupsTab;
        private Conf_Groups conf_Groups1;
    }
}