namespace MyEmulators2
{
    partial class Wzd_NewEmu_Config2
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
            this.enableGMCheckBox = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.suspendMPCheckBox = new System.Windows.Forms.CheckBox();
            this.escExitCheckBox = new System.Windows.Forms.CheckBox();
            this.mountImagesCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // enableGMCheckBox
            // 
            this.enableGMCheckBox.AutoSize = true;
            this.enableGMCheckBox.Location = new System.Drawing.Point(6, 159);
            this.enableGMCheckBox.Name = "enableGMCheckBox";
            this.enableGMCheckBox.Size = new System.Drawing.Size(217, 17);
            this.enableGMCheckBox.TabIndex = 51;
            this.enableGMCheckBox.Text = "Enable support for GoodMerge archives.";
            this.enableGMCheckBox.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(4, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(370, 36);
            this.label12.TabIndex = 50;
            this.label12.Text = "Additional settings.";
            // 
            // suspendMPCheckBox
            // 
            this.suspendMPCheckBox.AutoSize = true;
            this.suspendMPCheckBox.Location = new System.Drawing.Point(6, 124);
            this.suspendMPCheckBox.Name = "suspendMPCheckBox";
            this.suspendMPCheckBox.Size = new System.Drawing.Size(344, 17);
            this.suspendMPCheckBox.TabIndex = 47;
            this.suspendMPCheckBox.Text = "Suspend MediaPortal while running to increase available resources.";
            this.suspendMPCheckBox.UseVisualStyleBackColor = true;
            // 
            // escExitCheckBox
            // 
            this.escExitCheckBox.AutoSize = true;
            this.escExitCheckBox.Location = new System.Drawing.Point(6, 89);
            this.escExitCheckBox.Name = "escExitCheckBox";
            this.escExitCheckBox.Size = new System.Drawing.Size(377, 17);
            this.escExitCheckBox.TabIndex = 49;
            this.escExitCheckBox.Text = "Send the \'ESC\' key instead of a Close Window message to stop emulation.";
            this.escExitCheckBox.UseVisualStyleBackColor = true;
            // 
            // mountImagesCheckBox
            // 
            this.mountImagesCheckBox.AutoSize = true;
            this.mountImagesCheckBox.Location = new System.Drawing.Point(6, 43);
            this.mountImagesCheckBox.Name = "mountImagesCheckBox";
            this.mountImagesCheckBox.Size = new System.Drawing.Size(262, 17);
            this.mountImagesCheckBox.TabIndex = 48;
            this.mountImagesCheckBox.Text = "Mount disk images with MediaPortal\'s VirtualDrive.";
            this.mountImagesCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "(VirtualDrive must be configured and enabled)";
            // 
            // Wzd_NewEmu_Config2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enableGMCheckBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.suspendMPCheckBox);
            this.Controls.Add(this.escExitCheckBox);
            this.Controls.Add(this.mountImagesCheckBox);
            this.MaximumSize = new System.Drawing.Size(495, 305);
            this.Name = "Wzd_NewEmu_Config2";
            this.Size = new System.Drawing.Size(386, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enableGMCheckBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox suspendMPCheckBox;
        private System.Windows.Forms.CheckBox escExitCheckBox;
        private System.Windows.Forms.CheckBox mountImagesCheckBox;
        private System.Windows.Forms.Label label1;
    }
}
