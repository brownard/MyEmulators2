namespace myEmulators
{
    partial class Conf_PC_Details
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
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_Description = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.btnLogoView = new System.Windows.Forms.Button();
            this.txt_Logo = new System.Windows.Forms.TextBox();
            this.btnLogo = new System.Windows.Forms.Button();
            this.btnDelFanart = new System.Windows.Forms.Button();
            this.txt_Fanart = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlFanart = new System.Windows.Forms.Panel();
            this.btnFanartView = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlFanart.SuspendLayout();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save.Location = new System.Drawing.Point(259, 439);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 0;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click +=new System.EventHandler(save_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(178, 439);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.titleBox);
            this.groupBox1.Controls.Add(this.titleLabel);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 135);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System info";
            // 
            // titleBox
            // 
            this.titleBox.Location = new System.Drawing.Point(115, 27);
            this.titleBox.MaxLength = 50;
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new System.Drawing.Size(164, 20);
            this.titleBox.TabIndex = 7;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(8, 30);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(63, 13);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "System title:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_Description);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(341, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(258, 141);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Extra Info (optional)";
            // 
            // txt_Description
            // 
            this.txt_Description.Location = new System.Drawing.Point(9, 38);
            this.txt_Description.MaxLength = 2000;
            this.txt_Description.Multiline = true;
            this.txt_Description.Name = "txt_Description";
            this.txt_Description.Size = new System.Drawing.Size(234, 99);
            this.txt_Description.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Description";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Box Front / Logo";
            // 
            // pnlLogo
            // 
            this.pnlLogo.AllowDrop = true;
            this.pnlLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlLogo.Controls.Add(this.btnLogoView);
            this.pnlLogo.Controls.Add(this.txt_Logo);
            this.pnlLogo.Controls.Add(this.label10);
            this.pnlLogo.Controls.Add(this.btnLogo);
            this.pnlLogo.Location = new System.Drawing.Point(8, 208);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(245, 187);
            this.pnlLogo.TabIndex = 25;
            this.pnlLogo.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlLogo.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // btnLogoView
            // 
            this.btnLogoView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogoView.Location = new System.Drawing.Point(192, 3);
            this.btnLogoView.Name = "btnLogoView";
            this.btnLogoView.Size = new System.Drawing.Size(20, 21);
            this.btnLogoView.TabIndex = 48;
            this.btnLogoView.Text = "V";
            this.btnLogoView.UseVisualStyleBackColor = true;
            this.btnLogoView.Click += new System.EventHandler(this.btnLogoView_Click);
            // 
            // txt_Logo
            // 
            this.txt_Logo.Location = new System.Drawing.Point(-2, 81);
            this.txt_Logo.Name = "txt_Logo";
            this.txt_Logo.Size = new System.Drawing.Size(225, 21);
            this.txt_Logo.TabIndex = 34;
            this.txt_Logo.Visible = false;
            // 
            // btnLogo
            // 
            this.btnLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogo.Location = new System.Drawing.Point(218, 3);
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.Size = new System.Drawing.Size(20, 21);
            this.btnLogo.TabIndex = 41;
            this.btnLogo.Text = "X";
            this.btnLogo.UseVisualStyleBackColor = true;
            this.btnLogo.Click += new System.EventHandler(this.btnLogo_Click);
            // 
            // btnDelFanart
            // 
            this.btnDelFanart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelFanart.Location = new System.Drawing.Point(218, 3);
            this.btnDelFanart.Name = "btnDelFanart";
            this.btnDelFanart.Size = new System.Drawing.Size(20, 21);
            this.btnDelFanart.TabIndex = 44;
            this.btnDelFanart.Text = "X";
            this.btnDelFanart.UseVisualStyleBackColor = true;
            this.btnDelFanart.Click += new System.EventHandler(this.btnDelFanart_Click);
            // 
            // txt_Fanart
            // 
            this.txt_Fanart.Location = new System.Drawing.Point(-2, 81);
            this.txt_Fanart.Name = "txt_Fanart";
            this.txt_Fanart.Size = new System.Drawing.Size(225, 21);
            this.txt_Fanart.TabIndex = 34;
            this.txt_Fanart.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 43;
            this.label11.Text = "Fanart";
            // 
            // pnlFanart
            // 
            this.pnlFanart.AllowDrop = true;
            this.pnlFanart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlFanart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlFanart.Controls.Add(this.btnFanartView);
            this.pnlFanart.Controls.Add(this.txt_Fanart);
            this.pnlFanart.Controls.Add(this.label11);
            this.pnlFanart.Controls.Add(this.btnDelFanart);
            this.pnlFanart.Location = new System.Drawing.Point(341, 208);
            this.pnlFanart.Name = "pnlFanart";
            this.pnlFanart.Size = new System.Drawing.Size(245, 187);
            this.pnlFanart.TabIndex = 42;
            this.pnlFanart.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlFanart.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // btnFanartView
            // 
            this.btnFanartView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFanartView.Location = new System.Drawing.Point(192, 3);
            this.btnFanartView.Name = "btnFanartView";
            this.btnFanartView.Size = new System.Drawing.Size(20, 21);
            this.btnFanartView.TabIndex = 48;
            this.btnFanartView.Text = "V";
            this.btnFanartView.UseVisualStyleBackColor = true;
            this.btnFanartView.Click += new System.EventHandler(this.btnFanartView_Click);
            // 
            // Conf_PC_Details
            // 
            this.AcceptButton = this.save;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(600, 473);
            this.Controls.Add(this.pnlFanart);
            this.Controls.Add(this.pnlLogo);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Conf_PC_Details";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Conf_PC_Details_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            this.pnlFanart.ResumeLayout(false);
            this.pnlFanart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txt_Description;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.TextBox txt_Logo;
        private System.Windows.Forms.Button btnLogo;
        private System.Windows.Forms.Button btnDelFanart;
        private System.Windows.Forms.TextBox txt_Fanart;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel pnlFanart;
        private System.Windows.Forms.Button btnLogoView;
        private System.Windows.Forms.Button btnFanartView;


    }
}