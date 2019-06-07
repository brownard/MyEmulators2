namespace MyEmulators2
{
    partial class Wzd_NewRom_Info
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Title = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.profileComboBox = new System.Windows.Forms.ComboBox();
            this.favCheckBox = new System.Windows.Forms.CheckBox();
            this.importCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(370, 36);
            this.label3.TabIndex = 35;
            this.label3.Text = "Please provide a title and a profile to use with the new game.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 111;
            this.label11.Text = "Title";
            // 
            // txt_Title
            // 
            this.txt_Title.Location = new System.Drawing.Point(86, 37);
            this.txt_Title.Name = "txt_Title";
            this.txt_Title.Size = new System.Drawing.Size(287, 20);
            this.txt_Title.TabIndex = 110;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 112;
            this.label1.Text = "Profile";
            // 
            // profileComboBox
            // 
            this.profileComboBox.DisplayMember = "Title";
            this.profileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileComboBox.FormattingEnabled = true;
            this.profileComboBox.Location = new System.Drawing.Point(86, 69);
            this.profileComboBox.Name = "profileComboBox";
            this.profileComboBox.Size = new System.Drawing.Size(121, 21);
            this.profileComboBox.TabIndex = 113;
            // 
            // favCheckBox
            // 
            this.favCheckBox.AutoSize = true;
            this.favCheckBox.Location = new System.Drawing.Point(6, 110);
            this.favCheckBox.Name = "favCheckBox";
            this.favCheckBox.Size = new System.Drawing.Size(70, 17);
            this.favCheckBox.TabIndex = 114;
            this.favCheckBox.Text = "Favourite";
            this.favCheckBox.UseVisualStyleBackColor = true;
            // 
            // importCheckBox
            // 
            this.importCheckBox.AutoSize = true;
            this.importCheckBox.Checked = true;
            this.importCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.importCheckBox.Location = new System.Drawing.Point(218, 269);
            this.importCheckBox.Name = "importCheckBox";
            this.importCheckBox.Size = new System.Drawing.Size(155, 17);
            this.importCheckBox.TabIndex = 115;
            this.importCheckBox.Text = "Send new game to importer";
            this.importCheckBox.UseVisualStyleBackColor = true;
            // 
            // Wzd_NewRom_Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importCheckBox);
            this.Controls.Add(this.favCheckBox);
            this.Controls.Add(this.profileComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_Title);
            this.Controls.Add(this.label3);
            this.Name = "Wzd_NewRom_Info";
            this.Size = new System.Drawing.Size(386, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox profileComboBox;
        private System.Windows.Forms.CheckBox favCheckBox;
        private System.Windows.Forms.CheckBox importCheckBox;

    }
}
