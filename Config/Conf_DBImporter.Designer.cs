namespace MyEmulators2
{
    partial class Conf_DBImporter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.importToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.delRomButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.ignoreButton = new System.Windows.Forms.Button();
            this.findButton = new System.Windows.Forms.Button();
            this.approveButton = new System.Windows.Forms.Button();
            this.mergeButton = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.progressLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.importGridView = new System.Windows.Forms.DataGridView();
            this.columnIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.columnFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTitle = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.importGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // importToolTip
            // 
            this.importToolTip.AutoPopDelay = 10000;
            this.importToolTip.InitialDelay = 250;
            this.importToolTip.ReshowDelay = 100;
            // 
            // delRomButton
            // 
            this.delRomButton.Image = global::MyEmulators2.Properties.Resources.cross;
            this.delRomButton.Location = new System.Drawing.Point(90, 4);
            this.delRomButton.Name = "delRomButton";
            this.delRomButton.Size = new System.Drawing.Size(28, 23);
            this.delRomButton.TabIndex = 9;
            this.importToolTip.SetToolTip(this.delRomButton, "Delete selected item(s)");
            this.delRomButton.UseVisualStyleBackColor = true;
            this.delRomButton.Click += new System.EventHandler(this.delRomButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = global::MyEmulators2.Properties.Resources.view_refresh;
            this.refreshButton.Location = new System.Drawing.Point(766, 4);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(28, 23);
            this.refreshButton.TabIndex = 7;
            this.importToolTip.SetToolTip(this.refreshButton, "Restart Importer");
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // ignoreButton
            // 
            this.ignoreButton.Image = global::MyEmulators2.Properties.Resources.page_white_edit;
            this.ignoreButton.Location = new System.Drawing.Point(61, 4);
            this.ignoreButton.Name = "ignoreButton";
            this.ignoreButton.Size = new System.Drawing.Size(28, 23);
            this.ignoreButton.TabIndex = 5;
            this.importToolTip.SetToolTip(this.ignoreButton, "Add as a blank game");
            this.ignoreButton.UseVisualStyleBackColor = true;
            this.ignoreButton.Click += new System.EventHandler(this.ignoreButton_Click);
            // 
            // findButton
            // 
            this.findButton.Image = global::MyEmulators2.Properties.Resources.find;
            this.findButton.Location = new System.Drawing.Point(32, 4);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(28, 23);
            this.findButton.TabIndex = 4;
            this.importToolTip.SetToolTip(this.findButton, "Manual search");
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.findButton_Click);
            // 
            // approveButton
            // 
            this.approveButton.Image = global::MyEmulators2.Properties.Resources.tick;
            this.approveButton.Location = new System.Drawing.Point(3, 4);
            this.approveButton.Name = "approveButton";
            this.approveButton.Size = new System.Drawing.Size(28, 23);
            this.approveButton.TabIndex = 3;
            this.importToolTip.SetToolTip(this.approveButton, "Approve selected item(s)");
            this.approveButton.UseVisualStyleBackColor = true;
            this.approveButton.Click += new System.EventHandler(this.approveButton_Click);
            // 
            // mergeButton
            // 
            this.mergeButton.Image = global::MyEmulators2.Properties.Resources.arrow_join;
            this.mergeButton.Location = new System.Drawing.Point(119, 4);
            this.mergeButton.Name = "mergeButton";
            this.mergeButton.Size = new System.Drawing.Size(28, 23);
            this.mergeButton.TabIndex = 8;
            this.mergeButton.UseVisualStyleBackColor = true;
            this.mergeButton.Click += new System.EventHandler(this.mergeButton_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Status";
            this.dataGridViewImageColumn1.Image = global::MyEmulators2.Properties.Resources.information;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 50;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(0, 485);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(35, 13);
            this.progressLabel.TabIndex = 2;
            this.progressLabel.Text = "label1";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 459);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(791, 23);
            this.progressBar.TabIndex = 1;
            // 
            // importGridView
            // 
            this.importGridView.AllowUserToAddRows = false;
            this.importGridView.AllowUserToDeleteRows = false;
            this.importGridView.AllowUserToResizeRows = false;
            this.importGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.importGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.importGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIcon,
            this.columnFilename,
            this.columnTitle});
            this.importGridView.Location = new System.Drawing.Point(3, 33);
            this.importGridView.Name = "importGridView";
            this.importGridView.RowHeadersVisible = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.importGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.importGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.importGridView.ShowCellErrors = false;
            this.importGridView.ShowEditingIcon = false;
            this.importGridView.Size = new System.Drawing.Size(791, 401);
            this.importGridView.TabIndex = 0;
            this.importGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.importGridView_CellMouseEnter);
            // 
            // columnIcon
            // 
            this.columnIcon.HeaderText = "Status";
            this.columnIcon.Image = global::MyEmulators2.Properties.Resources.information;
            this.columnIcon.Name = "columnIcon";
            this.columnIcon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnIcon.Width = 50;
            // 
            // columnFilename
            // 
            this.columnFilename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnFilename.DataPropertyName = "DisplayFilename";
            this.columnFilename.HeaderText = "Filename";
            this.columnFilename.Name = "columnFilename";
            this.columnFilename.ReadOnly = true;
            // 
            // columnTitle
            // 
            this.columnTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnTitle.HeaderText = "Title";
            this.columnTitle.MaxDropDownItems = 50;
            this.columnTitle.Name = "columnTitle";
            this.columnTitle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Conf_DBImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.delRomButton);
            this.Controls.Add(this.mergeButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.ignoreButton);
            this.Controls.Add(this.findButton);
            this.Controls.Add(this.approveButton);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.importGridView);
            this.DoubleBuffered = true;
            this.Name = "Conf_DBImporter";
            this.Size = new System.Drawing.Size(797, 506);
            this.Load += new System.EventHandler(this.Conf_DBImporter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.importGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView importGridView;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button approveButton;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.Button ignoreButton;
        private System.Windows.Forms.ToolTip importToolTip;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button mergeButton;
        private System.Windows.Forms.Button delRomButton;
        private System.Windows.Forms.DataGridViewImageColumn columnIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFilename;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnTitle;
    }
}