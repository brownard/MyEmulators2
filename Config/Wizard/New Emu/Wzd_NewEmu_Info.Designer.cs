namespace MyEmulators2
{
    partial class Wzd_NewEmu_Info
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (logo != null)
                    logo.Dispose();
                if (fanart != null)
                    fanart.Dispose();
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
            this.label15 = new System.Windows.Forms.Label();
            this.platformComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Title = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gradeUpDown = new System.Windows.Forms.NumericUpDown();
            this.txt_company = new System.Windows.Forms.TextBox();
            this.txt_yearmade = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_description = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.thumbAspectComboBox = new System.Windows.Forms.ComboBox();
            this.updateInfoButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gradeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 77);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 13);
            this.label15.TabIndex = 101;
            this.label15.Text = "Platform";
            // 
            // platformComboBox
            // 
            this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformComboBox.FormattingEnabled = true;
            this.platformComboBox.Location = new System.Drawing.Point(86, 74);
            this.platformComboBox.Name = "platformComboBox";
            this.platformComboBox.Size = new System.Drawing.Size(121, 21);
            this.platformComboBox.TabIndex = 100;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 99;
            this.label11.Text = "Title";
            // 
            // txt_Title
            // 
            this.txt_Title.Location = new System.Drawing.Point(86, 45);
            this.txt_Title.Name = "txt_Title";
            this.txt_Title.Size = new System.Drawing.Size(287, 20);
            this.txt_Title.TabIndex = 98;
            this.txt_Title.Text = "New Emulator";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 103;
            this.label8.Text = "Developer";
            // 
            // gradeUpDown
            // 
            this.gradeUpDown.Location = new System.Drawing.Point(231, 236);
            this.gradeUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gradeUpDown.Name = "gradeUpDown";
            this.gradeUpDown.Size = new System.Drawing.Size(34, 20);
            this.gradeUpDown.TabIndex = 109;
            // 
            // txt_company
            // 
            this.txt_company.Location = new System.Drawing.Point(86, 105);
            this.txt_company.Name = "txt_company";
            this.txt_company.Size = new System.Drawing.Size(287, 20);
            this.txt_company.TabIndex = 102;
            // 
            // txt_yearmade
            // 
            this.txt_yearmade.Location = new System.Drawing.Point(86, 235);
            this.txt_yearmade.MaxLength = 4;
            this.txt_yearmade.Name = "txt_yearmade";
            this.txt_yearmade.Size = new System.Drawing.Size(58, 20);
            this.txt_yearmade.TabIndex = 104;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 238);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 105;
            this.label9.Text = "Year";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(171, 238);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 106;
            this.label12.Text = "Grade";
            // 
            // txt_description
            // 
            this.txt_description.Location = new System.Drawing.Point(86, 137);
            this.txt_description.Multiline = true;
            this.txt_description.Name = "txt_description";
            this.txt_description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_description.Size = new System.Drawing.Size(287, 86);
            this.txt_description.TabIndex = 107;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 140);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 108;
            this.label13.Text = "Description";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 36);
            this.label1.TabIndex = 110;
            this.label1.Text = "Please provide a name for the new emulator and any additional information to be d" +
                "isplayed by the plugin.";
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 263);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(40, 26);
            this.label22.TabIndex = 119;
            this.label22.Text = "Case\r\nAspect";
            // 
            // thumbAspectComboBox
            // 
            this.thumbAspectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.thumbAspectComboBox.FormattingEnabled = true;
            this.thumbAspectComboBox.Items.AddRange(new object[] {
            "0 (Default)",
            "0.71 (DVD)",
            "1.14 (CD)",
            "1.45 (Cartridge)"});
            this.thumbAspectComboBox.Location = new System.Drawing.Point(86, 268);
            this.thumbAspectComboBox.Name = "thumbAspectComboBox";
            this.thumbAspectComboBox.Size = new System.Drawing.Size(121, 21);
            this.thumbAspectComboBox.TabIndex = 118;
            // 
            // updateInfoButton
            // 
            this.updateInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateInfoButton.Image = global::MyEmulators2.Properties.Resources.find;
            this.updateInfoButton.Location = new System.Drawing.Point(213, 71);
            this.updateInfoButton.Name = "updateInfoButton";
            this.updateInfoButton.Size = new System.Drawing.Size(30, 25);
            this.updateInfoButton.TabIndex = 120;
            this.updateInfoButton.UseVisualStyleBackColor = true;
            this.updateInfoButton.Click += new System.EventHandler(this.updateInfoButton_Click);
            // 
            // Wzd_NewEmu_Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.updateInfoButton);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.thumbAspectComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gradeUpDown);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txt_company);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txt_description);
            this.Controls.Add(this.platformComboBox);
            this.Controls.Add(this.txt_yearmade);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txt_Title);
            this.Controls.Add(this.label9);
            this.MaximumSize = new System.Drawing.Size(495, 305);
            this.Name = "Wzd_NewEmu_Info";
            this.Size = new System.Drawing.Size(495, 299);
            ((System.ComponentModel.ISupportInitialize)(this.gradeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_Title;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown gradeUpDown;
        private System.Windows.Forms.TextBox txt_company;
        private System.Windows.Forms.TextBox txt_yearmade;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_description;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox thumbAspectComboBox;
        private System.Windows.Forms.Button updateInfoButton;
    }
}