namespace myEmulators
{
    partial class Conf_PCGames
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
            this.movedown = new System.Windows.Forms.Button();
            this.moveup = new System.Windows.Forms.Button();
            this.pcList = new System.Windows.Forms.ListBox();
            this.delete = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.add = new System.Windows.Forms.Button();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.emuPathLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.movedown);
            this.groupBox1.Controls.Add(this.moveup);
            this.groupBox1.Controls.Add(this.pcList);
            this.groupBox1.Controls.Add(this.delete);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(240, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 282);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configured games";
            // 
            // movedown
            // 
            this.movedown.Enabled = false;
            this.movedown.Location = new System.Drawing.Point(176, 247);
            this.movedown.Name = "movedown";
            this.movedown.Size = new System.Drawing.Size(82, 23);
            this.movedown.TabIndex = 5;
            this.movedown.Text = "Move down";
            this.movedown.UseVisualStyleBackColor = true;
            this.movedown.Visible = false;
            // 
            // moveup
            // 
            this.moveup.Enabled = false;
            this.moveup.Location = new System.Drawing.Point(176, 218);
            this.moveup.Name = "moveup";
            this.moveup.Size = new System.Drawing.Size(82, 23);
            this.moveup.TabIndex = 4;
            this.moveup.Text = "Move up";
            this.moveup.UseVisualStyleBackColor = true;
            this.moveup.Visible = false;
            // 
            // pcList
            // 
            this.pcList.FormattingEnabled = true;
            this.pcList.Location = new System.Drawing.Point(15, 19);
            this.pcList.Name = "pcList";
            this.pcList.Size = new System.Drawing.Size(155, 251);
            this.pcList.TabIndex = 2;
            this.pcList.SelectedIndexChanged += new System.EventHandler(this.updateButtonEnablings);
            this.pcList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pcList_KeyDown);
            // 
            // delete
            // 
            this.delete.Enabled = false;
            this.delete.Location = new System.Drawing.Point(176, 20);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(82, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.add);
            this.groupBox2.Controls.Add(this.titleBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.browse);
            this.groupBox2.Controls.Add(this.pathBox);
            this.groupBox2.Controls.Add(this.emuPathLabel);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 192);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add game";
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(129, 150);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(75, 23);
            this.add.TabIndex = 7;
            this.add.Text = "Add game";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // titleBox
            // 
            this.titleBox.Location = new System.Drawing.Point(15, 101);
            this.titleBox.MaxLength = 200;
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new System.Drawing.Size(189, 21);
            this.titleBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Game title:";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(180, 45);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(24, 20);
            this.browse.TabIndex = 4;
            this.browse.Text = "..";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(15, 45);
            this.pathBox.MaxLength = 200;
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(159, 21);
            this.pathBox.TabIndex = 3;
            // 
            // emuPathLabel
            // 
            this.emuPathLabel.AutoSize = true;
            this.emuPathLabel.Location = new System.Drawing.Point(14, 29);
            this.emuPathLabel.Name = "emuPathLabel";
            this.emuPathLabel.Size = new System.Drawing.Size(88, 13);
            this.emuPathLabel.TabIndex = 2;
            this.emuPathLabel.Text = "Path to game:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 45);
            this.label2.TabIndex = 8;
            this.label2.Text = "These games run without an emulator executing them, ie they are stand-alone.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Configure PC Games";
            // 
            // Conf_PCGames
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Conf_PCGames";
            this.Size = new System.Drawing.Size(525, 335);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button movedown;
        private System.Windows.Forms.Button moveup;
        private System.Windows.Forms.ListBox pcList;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Label emuPathLabel;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
