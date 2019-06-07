namespace MyEmulators2
{
    partial class Wzd_NewEmu_Start
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
            this.label1 = new System.Windows.Forms.Label();
            this.pathBrowseButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please specify the default exe/bat file to use for the new emulator.";
            // 
            // pathBrowseButton
            // 
            this.pathBrowseButton.Location = new System.Drawing.Point(345, 88);
            this.pathBrowseButton.Name = "pathBrowseButton";
            this.pathBrowseButton.Size = new System.Drawing.Size(29, 23);
            this.pathBrowseButton.TabIndex = 31;
            this.pathBrowseButton.Text = "...";
            this.pathBrowseButton.UseVisualStyleBackColor = true;
            this.pathBrowseButton.Click += new System.EventHandler(this.pathBrowseButton_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(6, 90);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(333, 20);
            this.pathTextBox.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(370, 36);
            this.label3.TabIndex = 32;
            this.label3.Text = "This wizard will guide you through the process of creating a new emulator.";
            // 
            // Wzd_NewEmu_Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pathBrowseButton);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(495, 305);
            this.Name = "Wzd_NewEmu_Start";
            this.Size = new System.Drawing.Size(386, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button pathBrowseButton;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Label label3;

    }
}
