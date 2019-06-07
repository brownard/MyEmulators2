namespace MyEmulators2
{
    partial class Wzd_NewRom_Start
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
            this.label1 = new System.Windows.Forms.Label();
            this.emuComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pathBrowseButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.argsLabel = new System.Windows.Forms.Label();
            this.argsTextBox = new System.Windows.Forms.TextBox();
            this.argsInfoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(370, 36);
            this.label3.TabIndex = 35;
            this.label3.Text = "This wizard will guide you through the process of adding a new game.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 12);
            this.label1.TabIndex = 33;
            this.label1.Text = "Please specify the parent emulator for the new game.";
            // 
            // emuComboBox
            // 
            this.emuComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emuComboBox.FormattingEnabled = true;
            this.emuComboBox.Location = new System.Drawing.Point(6, 85);
            this.emuComboBox.MaxDropDownItems = 100;
            this.emuComboBox.Name = "emuComboBox";
            this.emuComboBox.Size = new System.Drawing.Size(165, 21);
            this.emuComboBox.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(370, 15);
            this.label2.TabIndex = 37;
            this.label2.Text = "Please specify the path to the new game.";
            // 
            // pathBrowseButton
            // 
            this.pathBrowseButton.Location = new System.Drawing.Point(345, 154);
            this.pathBrowseButton.Name = "pathBrowseButton";
            this.pathBrowseButton.Size = new System.Drawing.Size(29, 23);
            this.pathBrowseButton.TabIndex = 39;
            this.pathBrowseButton.Text = "...";
            this.pathBrowseButton.UseVisualStyleBackColor = true;
            this.pathBrowseButton.Click += new System.EventHandler(this.pathBrowseButton_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(86, 156);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(253, 20);
            this.pathTextBox.TabIndex = 38;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 159);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 112;
            this.label11.Text = "Path";
            // 
            // argsLabel
            // 
            this.argsLabel.AutoSize = true;
            this.argsLabel.Location = new System.Drawing.Point(3, 216);
            this.argsLabel.Name = "argsLabel";
            this.argsLabel.Size = new System.Drawing.Size(57, 13);
            this.argsLabel.TabIndex = 114;
            this.argsLabel.Text = "Arguments";
            // 
            // argsTextBox
            // 
            this.argsTextBox.Location = new System.Drawing.Point(86, 213);
            this.argsTextBox.Name = "argsTextBox";
            this.argsTextBox.Size = new System.Drawing.Size(253, 20);
            this.argsTextBox.TabIndex = 113;
            // 
            // argsInfoText
            // 
            this.argsInfoText.AutoSize = true;
            this.argsInfoText.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.argsInfoText.Location = new System.Drawing.Point(4, 189);
            this.argsInfoText.Name = "argsInfoText";
            this.argsInfoText.Size = new System.Drawing.Size(233, 12);
            this.argsInfoText.TabIndex = 115;
            this.argsInfoText.Text = "Specify arguments for the PC game here.";
            // 
            // Wzd_NewRom_Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.argsInfoText);
            this.Controls.Add(this.argsLabel);
            this.Controls.Add(this.argsTextBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pathBrowseButton);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.emuComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "Wzd_NewRom_Start";
            this.Size = new System.Drawing.Size(386, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox emuComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button pathBrowseButton;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label argsLabel;
        private System.Windows.Forms.TextBox argsTextBox;
        private System.Windows.Forms.Label argsInfoText;

    }
}
