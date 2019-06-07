namespace myEmulators
{
    partial class Conf_DBSync
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.noOverwritingCheck = new System.Windows.Forms.CheckBox();
            this.importBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.importText = new System.Windows.Forms.TextBox();
            this.import = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.exportBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.exportText = new System.Windows.Forms.TextBox();
            this.export = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.noOverwritingCheck);
            this.groupBox1.Controls.Add(this.importBrowse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.importText);
            this.groupBox1.Controls.Add(this.import);
            this.groupBox1.Location = new System.Drawing.Point(16, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import meta data";
            // 
            // noOverwritingCheck
            // 
            this.noOverwritingCheck.AutoSize = true;
            this.noOverwritingCheck.Checked = true;
            this.noOverwritingCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noOverwritingCheck.Location = new System.Drawing.Point(21, 77);
            this.noOverwritingCheck.Name = "noOverwritingCheck";
            this.noOverwritingCheck.Size = new System.Drawing.Size(203, 17);
            this.noOverwritingCheck.TabIndex = 4;
            this.noOverwritingCheck.Text = "Do not overwrite already existing data";
            this.noOverwritingCheck.UseVisualStyleBackColor = true;
            // 
            // importBrowse
            // 
            this.importBrowse.Location = new System.Drawing.Point(334, 48);
            this.importBrowse.Name = "importBrowse";
            this.importBrowse.Size = new System.Drawing.Size(24, 20);
            this.importBrowse.TabIndex = 3;
            this.importBrowse.Text = "..";
            this.importBrowse.UseVisualStyleBackColor = true;
            this.importBrowse.Click += new System.EventHandler(this.importBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "From file:";
            // 
            // importText
            // 
            this.importText.Location = new System.Drawing.Point(21, 48);
            this.importText.Name = "importText";
            this.importText.Size = new System.Drawing.Size(307, 20);
            this.importText.TabIndex = 1;
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(394, 45);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(75, 23);
            this.import.TabIndex = 0;
            this.import.Text = "Import";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.exportBrowse);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.exportText);
            this.groupBox2.Controls.Add(this.export);
            this.groupBox2.Location = new System.Drawing.Point(16, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(492, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Export meta data";
            // 
            // exportBrowse
            // 
            this.exportBrowse.Location = new System.Drawing.Point(334, 48);
            this.exportBrowse.Name = "exportBrowse";
            this.exportBrowse.Size = new System.Drawing.Size(24, 20);
            this.exportBrowse.TabIndex = 6;
            this.exportBrowse.Text = "..";
            this.exportBrowse.UseVisualStyleBackColor = true;
            this.exportBrowse.Click += new System.EventHandler(this.exportBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "To file:";
            // 
            // exportText
            // 
            this.exportText.Location = new System.Drawing.Point(21, 48);
            this.exportText.Name = "exportText";
            this.exportText.Size = new System.Drawing.Size(307, 20);
            this.exportText.TabIndex = 4;
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(394, 45);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(75, 23);
            this.export.TabIndex = 0;
            this.export.Text = "Export";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(495, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "Note: This will only export/import metadata such as grade and description about t" +
                "he games, not the path to the game itself.";
            // 
            // Conf_DBSync
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Conf_DBSync";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox importText;
        private System.Windows.Forms.Button importBrowse;
        private System.Windows.Forms.Button exportBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox exportText;
        private System.Windows.Forms.CheckBox noOverwritingCheck;
    }
}
