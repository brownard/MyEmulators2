namespace myEmulators
{
    partial class Conf_DBBackup
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.restoreBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.restoreLocationText = new System.Windows.Forms.TextBox();
            this.restore = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.backupBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.backupLocationText = new System.Windows.Forms.TextBox();
            this.backup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.restoreBrowse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.restoreLocationText);
            this.groupBox1.Controls.Add(this.restore);
            this.groupBox1.Location = new System.Drawing.Point(16, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Restore";
            // 
            // restoreBrowse
            // 
            this.restoreBrowse.Location = new System.Drawing.Point(334, 48);
            this.restoreBrowse.Name = "restoreBrowse";
            this.restoreBrowse.Size = new System.Drawing.Size(24, 20);
            this.restoreBrowse.TabIndex = 3;
            this.restoreBrowse.Text = "..";
            this.restoreBrowse.UseVisualStyleBackColor = true;
            this.restoreBrowse.Click += new System.EventHandler(this.restoreBrowse_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "From file:";
            // 
            // restoreLocationText
            // 
            this.restoreLocationText.Location = new System.Drawing.Point(21, 48);
            this.restoreLocationText.Name = "restoreLocationText";
            this.restoreLocationText.Size = new System.Drawing.Size(307, 20);
            this.restoreLocationText.TabIndex = 1;
            // 
            // restore
            // 
            this.restore.Location = new System.Drawing.Point(394, 45);
            this.restore.Name = "restore";
            this.restore.Size = new System.Drawing.Size(75, 23);
            this.restore.TabIndex = 0;
            this.restore.Text = "Restore";
            this.restore.UseVisualStyleBackColor = true;
            this.restore.Click += new System.EventHandler(this.restore_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.backupBrowse);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.backupLocationText);
            this.groupBox2.Controls.Add(this.backup);
            this.groupBox2.Location = new System.Drawing.Point(16, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(492, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Backup";
            // 
            // backupBrowse
            // 
            this.backupBrowse.Location = new System.Drawing.Point(334, 47);
            this.backupBrowse.Name = "backupBrowse";
            this.backupBrowse.Size = new System.Drawing.Size(24, 20);
            this.backupBrowse.TabIndex = 6;
            this.backupBrowse.Text = "..";
            this.backupBrowse.UseVisualStyleBackColor = true;
            this.backupBrowse.Click += new System.EventHandler(this.backupBrowse_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "To file:";
            // 
            // backupLocationText
            // 
            this.backupLocationText.Location = new System.Drawing.Point(21, 47);
            this.backupLocationText.Name = "backupLocationText";
            this.backupLocationText.Size = new System.Drawing.Size(307, 20);
            this.backupLocationText.TabIndex = 4;
            // 
            // backup
            // 
            this.backup.Location = new System.Drawing.Point(394, 44);
            this.backup.Name = "backup";
            this.backup.Size = new System.Drawing.Size(75, 23);
            this.backup.TabIndex = 0;
            this.backup.Text = "Backup";
            this.backup.UseVisualStyleBackColor = true;
            this.backup.Click += new System.EventHandler(this.backup_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(495, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "Note: The backup button will make a full backup of all emulators/games. The resto" +
                "re button restores the data over any existing data.";
            // 
            // Conf_DBBackup
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Conf_DBBackup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button restore;
        private System.Windows.Forms.Button backup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox restoreLocationText;
        private System.Windows.Forms.Button restoreBrowse;
        private System.Windows.Forms.Button backupBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox backupLocationText;
    }
}
