namespace myEmulators
{
    partial class Conf_Database
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.btnUpdateAll = new System.Windows.Forms.Button();
            this.btnUpdateWithoutData = new System.Windows.Forms.Button();
            this.btnBatchNew = new System.Windows.Forms.Button();
            this.btnBatchAll = new System.Windows.Forms.Button();
            this.ddlPlatform = new System.Windows.Forms.ComboBox();
            this.path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOnline = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnLocal = new System.Windows.Forms.DataGridViewButtonColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.yearmade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.favourite = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.path,
            this.btnOnline,
            this.btnLocal,
            this.title,
            this.grade,
            this.yearmade,
            this.description,
            this.genre,
            this.company,
            this.favourite,
            this.visible});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(720, 501);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(104, 446);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(128, 20);
            this.searchBox.TabIndex = 4;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyDown);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(40, 453);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(44, 13);
            this.searchLabel.TabIndex = 3;
            this.searchLabel.Text = "Search:";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Location = new System.Drawing.Point(594, 504);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(123, 23);
            this.btnUpdateAll.TabIndex = 5;
            this.btnUpdateAll.Text = "Update All From Web";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // btnUpdateWithoutData
            // 
            this.btnUpdateWithoutData.Location = new System.Drawing.Point(458, 504);
            this.btnUpdateWithoutData.Name = "btnUpdateWithoutData";
            this.btnUpdateWithoutData.Size = new System.Drawing.Size(130, 23);
            this.btnUpdateWithoutData.TabIndex = 6;
            this.btnUpdateWithoutData.Text = "Update New From Web";
            this.btnUpdateWithoutData.UseVisualStyleBackColor = true;
            this.btnUpdateWithoutData.Click += new System.EventHandler(this.btnUpdateWithoutData_Click);
            // 
            // btnBatchNew
            // 
            this.btnBatchNew.Location = new System.Drawing.Point(186, 505);
            this.btnBatchNew.Name = "btnBatchNew";
            this.btnBatchNew.Size = new System.Drawing.Size(130, 23);
            this.btnBatchNew.TabIndex = 7;
            this.btnBatchNew.Text = "Batch Missing Data";
            this.btnBatchNew.UseVisualStyleBackColor = true;
            this.btnBatchNew.Click += new System.EventHandler(this.btnBatchNew_Click);
            // 
            // btnBatchAll
            // 
            this.btnBatchAll.Location = new System.Drawing.Point(322, 505);
            this.btnBatchAll.Name = "btnBatchAll";
            this.btnBatchAll.Size = new System.Drawing.Size(130, 23);
            this.btnBatchAll.TabIndex = 8;
            this.btnBatchAll.Text = "Batch Update All";
            this.btnBatchAll.UseVisualStyleBackColor = true;
            this.btnBatchAll.Click += new System.EventHandler(this.btnBatchAll_Click);
            // 
            // ddlPlatform
            // 
            this.ddlPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPlatform.FormattingEnabled = true;
            this.ddlPlatform.Location = new System.Drawing.Point(59, 506);
            this.ddlPlatform.Name = "ddlPlatform";
            this.ddlPlatform.Size = new System.Drawing.Size(121, 21);
            this.ddlPlatform.TabIndex = 17;
            this.ddlPlatform.SelectedIndexChanged += new System.EventHandler(this.ddlPlatform_SelectedIndexChanged);
            // 
            // path
            // 
            this.path.HeaderText = "Path";
            this.path.MaxInputLength = 200;
            this.path.Name = "path";
            this.path.ReadOnly = true;
            this.path.Width = 150;
            // 
            // btnOnline
            // 
            this.btnOnline.HeaderText = "O";
            this.btnOnline.Name = "btnOnline";
            this.btnOnline.Text = "O";
            this.btnOnline.Width = 25;
            // 
            // btnLocal
            // 
            this.btnLocal.HeaderText = "L";
            this.btnLocal.Name = "btnLocal";
            this.btnLocal.Text = "L";
            this.btnLocal.Width = 25;
            // 
            // title
            // 
            this.title.HeaderText = "Title";
            this.title.MaxInputLength = 100;
            this.title.Name = "title";
            this.title.Width = 150;
            // 
            // grade
            // 
            this.grade.HeaderText = "Grade";
            this.grade.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.grade.Name = "grade";
            this.grade.Width = 60;
            // 
            // yearmade
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.yearmade.DefaultCellStyle = dataGridViewCellStyle1;
            this.yearmade.HeaderText = "Year";
            this.yearmade.MaxInputLength = 4;
            this.yearmade.Name = "yearmade";
            this.yearmade.Width = 60;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.MaxInputLength = 200;
            this.description.Name = "description";
            // 
            // genre
            // 
            this.genre.HeaderText = "Genre";
            this.genre.MaxInputLength = 50;
            this.genre.Name = "genre";
            // 
            // company
            // 
            this.company.HeaderText = "Company";
            this.company.MaxInputLength = 50;
            this.company.Name = "company";
            // 
            // favourite
            // 
            this.favourite.HeaderText = "Favourite";
            this.favourite.Name = "favourite";
            this.favourite.Width = 90;
            // 
            // visible
            // 
            this.visible.HeaderText = "Visible";
            this.visible.Name = "visible";
            this.visible.Width = 50;
            // 
            // Conf_Database
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ddlPlatform);
            this.Controls.Add(this.btnBatchAll);
            this.Controls.Add(this.btnBatchNew);
            this.Controls.Add(this.btnUpdateWithoutData);
            this.Controls.Add(this.btnUpdateAll);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Conf_Database";
            this.Size = new System.Drawing.Size(720, 530);
            this.Load += new System.EventHandler(this.Conf_Database_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button btnUpdateAll;
        private System.Windows.Forms.Button btnUpdateWithoutData;
        private System.Windows.Forms.Button btnBatchNew;
        private System.Windows.Forms.Button btnBatchAll;
        private System.Windows.Forms.ComboBox ddlPlatform;
        private System.Windows.Forms.DataGridViewTextBoxColumn path;
        private System.Windows.Forms.DataGridViewButtonColumn btnOnline;
        private System.Windows.Forms.DataGridViewButtonColumn btnLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewComboBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn yearmade;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn genre;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewCheckBoxColumn favourite;
        private System.Windows.Forms.DataGridViewCheckBoxColumn visible;
    }
}
