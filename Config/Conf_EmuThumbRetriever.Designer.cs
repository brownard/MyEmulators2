namespace MyEmulators2
{
    partial class Conf_EmuThumbRetriever
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
            this.resultsComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.coversPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.screensPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.statusLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // resultsComboBox
            // 
            this.resultsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resultsComboBox.FormattingEnabled = true;
            this.resultsComboBox.Location = new System.Drawing.Point(57, 40);
            this.resultsComboBox.Name = "resultsComboBox";
            this.resultsComboBox.Size = new System.Drawing.Size(223, 21);
            this.resultsComboBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Covers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Screens";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Results";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(300, 40);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(172, 20);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 9;
            // 
            // coversPanel
            // 
            this.coversPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.coversPanel.AutoScroll = true;
            this.coversPanel.Location = new System.Drawing.Point(12, 93);
            this.coversPanel.Name = "coversPanel";
            this.coversPanel.Size = new System.Drawing.Size(469, 91);
            this.coversPanel.TabIndex = 10;
            // 
            // screensPanel
            // 
            this.screensPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.screensPanel.AutoScroll = true;
            this.screensPanel.Location = new System.Drawing.Point(12, 203);
            this.screensPanel.Name = "screensPanel";
            this.screensPanel.Size = new System.Drawing.Size(469, 91);
            this.screensPanel.TabIndex = 11;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(297, 63);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 13);
            this.statusLabel.TabIndex = 12;
            this.statusLabel.Text = "status";
            // 
            // Conf_EmuThumbRetriever
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 320);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.screensPanel);
            this.Controls.Add(this.coversPanel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resultsComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(510, 344);
            this.Name = "Conf_EmuThumbRetriever";
            this.Text = "Thumb Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Conf_ThumbRetriever_FormClosing);
            this.Load += new System.EventHandler(this.Conf_ThumbRetriever_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox resultsComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.FlowLayoutPanel coversPanel;
        private System.Windows.Forms.FlowLayoutPanel screensPanel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}