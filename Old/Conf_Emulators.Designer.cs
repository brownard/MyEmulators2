namespace myEmulators
{
    partial class Conf_Emulators
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Conf_Emulators));
            this.add = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.emuList = new System.Windows.Forms.ListBox();
            this.delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.movedown = new System.Windows.Forms.Button();
            this.moveup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(176, 19);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(82, 23);
            this.add.TabIndex = 0;
            this.add.Text = "Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // edit
            // 
            this.edit.Enabled = false;
            this.edit.Location = new System.Drawing.Point(176, 48);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(82, 23);
            this.edit.TabIndex = 1;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // emuList
            // 
            this.emuList.FormattingEnabled = true;
            this.emuList.Location = new System.Drawing.Point(15, 19);
            this.emuList.Name = "emuList";
            this.emuList.Size = new System.Drawing.Size(155, 251);
            this.emuList.TabIndex = 2;
            this.emuList.DoubleClick += new System.EventHandler(this.emuList_DoubleClick);
            this.emuList.SelectedIndexChanged += new System.EventHandler(this.updateButtonEnablings);
            this.emuList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.emuList_KeyDown);
            // 
            // delete
            // 
            this.delete.Enabled = false;
            this.delete.Location = new System.Drawing.Point(176, 77);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(82, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.movedown);
            this.groupBox1.Controls.Add(this.moveup);
            this.groupBox1.Controls.Add(this.emuList);
            this.groupBox1.Controls.Add(this.delete);
            this.groupBox1.Controls.Add(this.add);
            this.groupBox1.Controls.Add(this.edit);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(240, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 282);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configured emulators";
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
            this.movedown.Click += new System.EventHandler(this.movedown_Click);
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
            this.moveup.Click += new System.EventHandler(this.moveup_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Welcome to My Emulators";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 241);
            this.label2.TabIndex = 6;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // Conf_Emulators
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Conf_Emulators";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.ListBox emuList;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button movedown;
        private System.Windows.Forms.Button moveup;


    }
}
