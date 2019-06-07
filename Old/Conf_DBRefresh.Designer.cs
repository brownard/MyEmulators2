namespace myEmulators
{
    partial class Conf_DBRefresh
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cleanButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.romRefreshButton = new System.Windows.Forms.Button();
            this.autoRefreshGames = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.autoImportCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.resizeThumbCheckBox = new System.Windows.Forms.CheckBox();
            this.approveTopCheckBox = new System.Windows.Forms.CheckBox();
            this.exactMatchCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cleanButton);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.romRefreshButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(492, 159);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Refresh Games Data";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(102, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(384, 45);
            this.label2.TabIndex = 9;
            this.label2.Text = "This will scan the rom database and remove any items where the corresponding file" +
                " can no longer be located. Ensure all Network Shares/Removable drives are connec" +
                "ted before use. ";
            // 
            // cleanButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(18, 71);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Size = new System.Drawing.Size(75, 23);
            this.cleanButton.TabIndex = 8;
            this.cleanButton.Text = "Clean";
            this.cleanButton.UseVisualStyleBackColor = true;
            this.cleanButton.Click += new System.EventHandler(this.cleanButton_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label4.Location = new System.Drawing.Point(102, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(384, 44);
            this.label4.TabIndex = 6;
            this.label4.Text = "This will scan all of the rom directories (including PC games) specified in your " +
                "configuration and load all of the needed data into the database. Use this when y" +
                "ou want to add new games.";
            // 
            // romRefreshButton
            // 
            this.romRefreshButton.Location = new System.Drawing.Point(18, 19);
            this.romRefreshButton.Name = "romRefreshButton";
            this.romRefreshButton.Size = new System.Drawing.Size(75, 23);
            this.romRefreshButton.TabIndex = 0;
            this.romRefreshButton.Text = "Refresh";
            this.romRefreshButton.UseVisualStyleBackColor = true;
            this.romRefreshButton.Click += new System.EventHandler(this.romRefreshButton_Click);
            // 
            // autoRefreshGames
            // 
            this.autoRefreshGames.AutoSize = true;
            this.autoRefreshGames.Location = new System.Drawing.Point(12, 179);
            this.autoRefreshGames.Name = "autoRefreshGames";
            this.autoRefreshGames.Size = new System.Drawing.Size(157, 17);
            this.autoRefreshGames.TabIndex = 12;
            this.autoRefreshGames.Text = "Automatically refresh games";
            this.autoRefreshGames.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label1.Location = new System.Drawing.Point(186, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 42);
            this.label1.TabIndex = 13;
            this.label1.Text = "This option will set the plugin to automatically check for new games upon startup" +
                ". Turn this off if you experience performance issues.";
            // 
            // autoImportCheckBox
            // 
            this.autoImportCheckBox.AutoSize = true;
            this.autoImportCheckBox.Location = new System.Drawing.Point(12, 230);
            this.autoImportCheckBox.Name = "autoImportCheckBox";
            this.autoImportCheckBox.Size = new System.Drawing.Size(168, 17);
            this.autoImportCheckBox.TabIndex = 14;
            this.autoImportCheckBox.Text = "Automatically import game info";
            this.autoImportCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(186, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(311, 40);
            this.label3.TabIndex = 15;
            this.label3.Text = "This option will set the plugin to automatically retrieve details/thumbs for any " +
                "new games (games where year is 0).";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.resizeThumbCheckBox);
            this.groupBox1.Controls.Add(this.approveTopCheckBox);
            this.groupBox1.Controls.Add(this.exactMatchCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 100);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import Settings";
            // 
            // resizeThumbCheckBox
            // 
            this.resizeThumbCheckBox.AutoSize = true;
            this.resizeThumbCheckBox.Location = new System.Drawing.Point(7, 68);
            this.resizeThumbCheckBox.Name = "resizeThumbCheckBox";
            this.resizeThumbCheckBox.Size = new System.Drawing.Size(180, 17);
            this.resizeThumbCheckBox.TabIndex = 2;
            this.resizeThumbCheckBox.Text = "Resize box art based on platform";
            this.resizeThumbCheckBox.UseVisualStyleBackColor = true;
            // 
            // approveTopCheckBox
            // 
            this.approveTopCheckBox.AutoSize = true;
            this.approveTopCheckBox.Location = new System.Drawing.Point(7, 44);
            this.approveTopCheckBox.Name = "approveTopCheckBox";
            this.approveTopCheckBox.Size = new System.Drawing.Size(251, 17);
            this.approveTopCheckBox.TabIndex = 1;
            this.approveTopCheckBox.Text = "Approve top result when better match not found";
            this.approveTopCheckBox.UseVisualStyleBackColor = true;
            // 
            // exactMatchCheckBox
            // 
            this.exactMatchCheckBox.AutoSize = true;
            this.exactMatchCheckBox.Location = new System.Drawing.Point(7, 20);
            this.exactMatchCheckBox.Name = "exactMatchCheckBox";
            this.exactMatchCheckBox.Size = new System.Drawing.Size(230, 17);
            this.exactMatchCheckBox.TabIndex = 0;
            this.exactMatchCheckBox.Text = "Only approve exact title and platform match";
            this.exactMatchCheckBox.UseVisualStyleBackColor = true;
            // 
            // Conf_DBRefresh
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.autoImportCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.autoRefreshGames);
            this.Controls.Add(this.groupBox3);
            this.Name = "Conf_DBRefresh";
            this.Size = new System.Drawing.Size(525, 408);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button romRefreshButton;
        private System.Windows.Forms.CheckBox autoRefreshGames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cleanButton;
        private System.Windows.Forms.CheckBox autoImportCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox resizeThumbCheckBox;
        private System.Windows.Forms.CheckBox approveTopCheckBox;
        private System.Windows.Forms.CheckBox exactMatchCheckBox;
    }
}
