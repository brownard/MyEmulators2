namespace MyEmulators2
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.backupThumbsCheckBox = new System.Windows.Forms.CheckBox();
            this.backupButton = new System.Windows.Forms.Button();
            this.backupPathButton = new System.Windows.Forms.Button();
            this.backupPathTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.restoreThumbsCheckBox = new System.Windows.Forms.CheckBox();
            this.mergeGroupBox = new System.Windows.Forms.GroupBox();
            this.gameMergeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.profileMergeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.emuMergeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mergeRadioButton = new System.Windows.Forms.RadioButton();
            this.cleanRadioButton = new System.Windows.Forms.RadioButton();
            this.restoreButton = new System.Windows.Forms.Button();
            this.restorePathButton = new System.Windows.Forms.Button();
            this.restorePathTextBox = new System.Windows.Forms.TextBox();
            this.backupDropdownItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mergeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backupDropdownItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.backupThumbsCheckBox);
            this.groupBox1.Controls.Add(this.backupButton);
            this.groupBox1.Controls.Add(this.backupPathButton);
            this.groupBox1.Controls.Add(this.backupPathTextBox);
            this.groupBox1.Location = new System.Drawing.Point(28, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Backup";
            // 
            // backupThumbsCheckBox
            // 
            this.backupThumbsCheckBox.AutoSize = true;
            this.backupThumbsCheckBox.Location = new System.Drawing.Point(22, 57);
            this.backupThumbsCheckBox.Name = "backupThumbsCheckBox";
            this.backupThumbsCheckBox.Size = new System.Drawing.Size(100, 17);
            this.backupThumbsCheckBox.TabIndex = 104;
            this.backupThumbsCheckBox.Text = "Backup thumbs";
            this.backupThumbsCheckBox.UseVisualStyleBackColor = true;
            // 
            // backupButton
            // 
            this.backupButton.Location = new System.Drawing.Point(435, 29);
            this.backupButton.Name = "backupButton";
            this.backupButton.Size = new System.Drawing.Size(75, 23);
            this.backupButton.TabIndex = 103;
            this.backupButton.Text = "Backup";
            this.backupButton.UseVisualStyleBackColor = true;
            this.backupButton.Click += new System.EventHandler(this.backupButton_Click);
            // 
            // backupPathButton
            // 
            this.backupPathButton.Location = new System.Drawing.Point(388, 29);
            this.backupPathButton.Name = "backupPathButton";
            this.backupPathButton.Size = new System.Drawing.Size(29, 23);
            this.backupPathButton.TabIndex = 102;
            this.backupPathButton.Text = "...";
            this.backupPathButton.UseVisualStyleBackColor = true;
            this.backupPathButton.Click += new System.EventHandler(this.backupPathButton_Click);
            // 
            // backupPathTextBox
            // 
            this.backupPathTextBox.Location = new System.Drawing.Point(22, 31);
            this.backupPathTextBox.Name = "backupPathTextBox";
            this.backupPathTextBox.Size = new System.Drawing.Size(348, 20);
            this.backupPathTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.Controls.Add(this.restoreThumbsCheckBox);
            this.groupBox2.Controls.Add(this.mergeGroupBox);
            this.groupBox2.Controls.Add(this.mergeRadioButton);
            this.groupBox2.Controls.Add(this.cleanRadioButton);
            this.groupBox2.Controls.Add(this.restoreButton);
            this.groupBox2.Controls.Add(this.restorePathButton);
            this.groupBox2.Controls.Add(this.restorePathTextBox);
            this.groupBox2.Location = new System.Drawing.Point(28, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(533, 283);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Restore";
            // 
            // restoreThumbsCheckBox
            // 
            this.restoreThumbsCheckBox.AutoSize = true;
            this.restoreThumbsCheckBox.Location = new System.Drawing.Point(22, 67);
            this.restoreThumbsCheckBox.Name = "restoreThumbsCheckBox";
            this.restoreThumbsCheckBox.Size = new System.Drawing.Size(100, 17);
            this.restoreThumbsCheckBox.TabIndex = 105;
            this.restoreThumbsCheckBox.Text = "Restore thumbs";
            this.restoreThumbsCheckBox.UseVisualStyleBackColor = true;
            // 
            // mergeGroupBox
            // 
            this.mergeGroupBox.Controls.Add(this.gameMergeComboBox);
            this.mergeGroupBox.Controls.Add(this.label3);
            this.mergeGroupBox.Controls.Add(this.profileMergeComboBox);
            this.mergeGroupBox.Controls.Add(this.label2);
            this.mergeGroupBox.Controls.Add(this.emuMergeComboBox);
            this.mergeGroupBox.Controls.Add(this.label1);
            this.mergeGroupBox.Enabled = false;
            this.mergeGroupBox.Location = new System.Drawing.Point(40, 137);
            this.mergeGroupBox.Name = "mergeGroupBox";
            this.mergeGroupBox.Size = new System.Drawing.Size(356, 131);
            this.mergeGroupBox.TabIndex = 109;
            this.mergeGroupBox.TabStop = false;
            this.mergeGroupBox.Text = "Merge Settings";
            // 
            // gameMergeComboBox
            // 
            this.gameMergeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameMergeComboBox.FormattingEnabled = true;
            this.gameMergeComboBox.Location = new System.Drawing.Point(211, 92);
            this.gameMergeComboBox.Name = "gameMergeComboBox";
            this.gameMergeComboBox.Size = new System.Drawing.Size(121, 21);
            this.gameMergeComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "On existing game with same path: ";
            // 
            // profileMergeComboBox
            // 
            this.profileMergeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileMergeComboBox.FormattingEnabled = true;
            this.profileMergeComboBox.Location = new System.Drawing.Point(211, 58);
            this.profileMergeComboBox.Name = "profileMergeComboBox";
            this.profileMergeComboBox.Size = new System.Drawing.Size(121, 21);
            this.profileMergeComboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "On existing profile with same name: ";
            // 
            // emuMergeComboBox
            // 
            this.emuMergeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emuMergeComboBox.FormattingEnabled = true;
            this.emuMergeComboBox.Location = new System.Drawing.Point(211, 24);
            this.emuMergeComboBox.Name = "emuMergeComboBox";
            this.emuMergeComboBox.Size = new System.Drawing.Size(121, 21);
            this.emuMergeComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "On existing emulator with same name: ";
            // 
            // mergeRadioButton
            // 
            this.mergeRadioButton.AutoSize = true;
            this.mergeRadioButton.Location = new System.Drawing.Point(22, 114);
            this.mergeRadioButton.Name = "mergeRadioButton";
            this.mergeRadioButton.Size = new System.Drawing.Size(115, 17);
            this.mergeRadioButton.TabIndex = 108;
            this.mergeRadioButton.Text = "Merge with existing";
            this.mergeRadioButton.UseVisualStyleBackColor = true;
            this.mergeRadioButton.CheckedChanged += new System.EventHandler(this.mergeRadioButton_CheckedChanged);
            // 
            // cleanRadioButton
            // 
            this.cleanRadioButton.AutoSize = true;
            this.cleanRadioButton.Checked = true;
            this.cleanRadioButton.Location = new System.Drawing.Point(22, 90);
            this.cleanRadioButton.Name = "cleanRadioButton";
            this.cleanRadioButton.Size = new System.Drawing.Size(250, 17);
            this.cleanRadioButton.TabIndex = 107;
            this.cleanRadioButton.TabStop = true;
            this.cleanRadioButton.Text = "Clean restore. Caution: removes all existing data";
            this.cleanRadioButton.UseVisualStyleBackColor = true;
            // 
            // restoreButton
            // 
            this.restoreButton.Location = new System.Drawing.Point(435, 245);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(75, 23);
            this.restoreButton.TabIndex = 106;
            this.restoreButton.Text = "Restore";
            this.restoreButton.UseVisualStyleBackColor = true;
            this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
            // 
            // restorePathButton
            // 
            this.restorePathButton.Location = new System.Drawing.Point(388, 39);
            this.restorePathButton.Name = "restorePathButton";
            this.restorePathButton.Size = new System.Drawing.Size(29, 23);
            this.restorePathButton.TabIndex = 105;
            this.restorePathButton.Text = "...";
            this.restorePathButton.UseVisualStyleBackColor = true;
            this.restorePathButton.Click += new System.EventHandler(this.restorePathButton_Click);
            // 
            // restorePathTextBox
            // 
            this.restorePathTextBox.Location = new System.Drawing.Point(22, 41);
            this.restorePathTextBox.Name = "restorePathTextBox";
            this.restorePathTextBox.Size = new System.Drawing.Size(348, 20);
            this.restorePathTextBox.TabIndex = 104;
            // 
            // backupDropdownItemBindingSource
            // 
            this.backupDropdownItemBindingSource.DataSource = typeof(MyEmulators2.BackupDropdownItem);
            // 
            // Conf_DBBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(700, 480);
            this.Name = "Conf_DBBackup";
            this.Size = new System.Drawing.Size(700, 480);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.mergeGroupBox.ResumeLayout(false);
            this.mergeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backupDropdownItemBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button backupButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox mergeGroupBox;
        private System.Windows.Forms.ComboBox gameMergeComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox profileMergeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox emuMergeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton mergeRadioButton;
        private System.Windows.Forms.RadioButton cleanRadioButton;
        private System.Windows.Forms.Button restoreButton;
        private System.Windows.Forms.Button restorePathButton;
        private System.Windows.Forms.TextBox restorePathTextBox;
        private System.Windows.Forms.BindingSource backupDropdownItemBindingSource;
        private System.Windows.Forms.Button backupPathButton;
        private System.Windows.Forms.TextBox backupPathTextBox;
        private System.Windows.Forms.CheckBox backupThumbsCheckBox;
        private System.Windows.Forms.CheckBox restoreThumbsCheckBox;
    }
}