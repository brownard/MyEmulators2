namespace MyEmulators2
{
    partial class Wzd_NewEmu_Config
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
            this.useQuotesCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.argumentsTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.workingDirTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // useQuotesCheckBox
            // 
            this.useQuotesCheckBox.AutoSize = true;
            this.useQuotesCheckBox.Location = new System.Drawing.Point(86, 99);
            this.useQuotesCheckBox.Name = "useQuotesCheckBox";
            this.useQuotesCheckBox.Size = new System.Drawing.Size(120, 17);
            this.useQuotesCheckBox.TabIndex = 29;
            this.useQuotesCheckBox.Text = "Use quotes in paths";
            this.useQuotesCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Arguments:";
            // 
            // argumentsTextBox
            // 
            this.argumentsTextBox.Location = new System.Drawing.Point(86, 73);
            this.argumentsTextBox.Name = "argumentsTextBox";
            this.argumentsTextBox.Size = new System.Drawing.Size(287, 20);
            this.argumentsTextBox.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Working Dir:";
            // 
            // workingDirTextBox
            // 
            this.workingDirTextBox.Location = new System.Drawing.Point(86, 252);
            this.workingDirTextBox.Name = "workingDirTextBox";
            this.workingDirTextBox.Size = new System.Drawing.Size(252, 20);
            this.workingDirTextBox.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 215);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(370, 31);
            this.label10.TabIndex = 38;
            this.label10.Text = "You can specify an alternate working directory to use instead of the exe/bat dire" +
                "ctory.";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(230, 150);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 41);
            this.label8.TabIndex = 37;
            this.label8.Text = "The filename of the selected rom without path or extension.";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(83, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 41);
            this.label7.TabIndex = 36;
            this.label7.Text = "The full path to the selected rom. (Appended by default if no wildcards are used)" +
                "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(230, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "%ROM_WITHOUT_EXT%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(83, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "%ROM%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Wildcards:";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(4, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(370, 36);
            this.label9.TabIndex = 34;
            this.label9.Text = "Please create the default \'profile\' for the new emulator. Additional profiles can" +
                " be created later allowing you to specify alternate settings. ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Path:";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Enabled = false;
            this.pathTextBox.Location = new System.Drawing.Point(86, 45);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(287, 20);
            this.pathTextBox.TabIndex = 41;
            // 
            // Wzd_NewEmu_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.argumentsTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.workingDirTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.useQuotesCheckBox);
            this.Controls.Add(this.label4);
            this.Name = "Wzd_NewEmu_Config";
            this.Size = new System.Drawing.Size(386, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox useQuotesCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox argumentsTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox workingDirTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pathTextBox;
    }
}