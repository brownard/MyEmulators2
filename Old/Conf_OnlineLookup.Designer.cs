namespace myEmulators
{
    partial class Conf_OnlineLookup
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
            this.txt_SearchTerm = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearmade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_Title = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_company = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_yearmade = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_genre = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_grade = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_description = new System.Windows.Forms.TextBox();
            this.ddlPlatform = new System.Windows.Forms.ComboBox();
            this.pnlBoxArt = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlScreenshots = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlBoxFront = new System.Windows.Forms.Panel();
            this.txt_BoxFront = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBoxFrontView = new System.Windows.Forms.Button();
            this.btnDelFront = new System.Windows.Forms.Button();
            this.pnlBoxBack = new System.Windows.Forms.Panel();
            this.txt_BoxBack = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnBoxBackView = new System.Windows.Forms.Button();
            this.btnDelBack = new System.Windows.Forms.Button();
            this.pnlShotTitle = new System.Windows.Forms.Panel();
            this.txt_BoxTitle = new System.Windows.Forms.TextBox();
            this.btnBoxTitleView = new System.Windows.Forms.Button();
            this.btnDelTitle = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlShotIngame = new System.Windows.Forms.Panel();
            this.txt_BoxIngame = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnBoxIngameView = new System.Windows.Forms.Button();
            this.btnDelIngame = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCancelBatch = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblPath = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnNewManual = new System.Windows.Forms.Button();
            this.txt_Manual = new System.Windows.Forms.TextBox();
            this.btnManual = new System.Windows.Forms.Button();
            this.chk_Visible = new System.Windows.Forms.CheckBox();
            this.chk_Favourite = new System.Windows.Forms.CheckBox();
            this.btnViewFanart = new System.Windows.Forms.Button();
            this.btnDeleteFanart = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.pnlFanart = new System.Windows.Forms.Panel();
            this.txt_Fanart = new System.Windows.Forms.TextBox();
            this.searchProgressBar = new System.Windows.Forms.ProgressBar();
            this.searchLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.pnlBoxFront.SuspendLayout();
            this.pnlBoxBack.SuspendLayout();
            this.pnlShotTitle.SuspendLayout();
            this.pnlShotIngame.SuspendLayout();
            this.pnlFanart.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_SearchTerm
            // 
            this.txt_SearchTerm.Location = new System.Drawing.Point(12, 29);
            this.txt_SearchTerm.Name = "txt_SearchTerm";
            this.txt_SearchTerm.Size = new System.Drawing.Size(201, 20);
            this.txt_SearchTerm.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(346, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(51, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.title,
            this.yearmade,
            this.genre,
            this.Column1});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(12, 55);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(385, 102);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // title
            // 
            this.title.HeaderText = "Title";
            this.title.MaxInputLength = 100;
            this.title.Name = "title";
            this.title.ReadOnly = true;
            this.title.Width = 220;
            // 
            // yearmade
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.yearmade.DefaultCellStyle = dataGridViewCellStyle1;
            this.yearmade.HeaderText = "Year";
            this.yearmade.MaxInputLength = 4;
            this.yearmade.Name = "yearmade";
            this.yearmade.ReadOnly = true;
            this.yearmade.Width = 60;
            // 
            // genre
            // 
            this.genre.HeaderText = "System";
            this.genre.MaxInputLength = 50;
            this.genre.Name = "genre";
            this.genre.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "SiteID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // txt_Title
            // 
            this.txt_Title.Location = new System.Drawing.Point(464, 26);
            this.txt_Title.Name = "txt_Title";
            this.txt_Title.Size = new System.Drawing.Size(193, 20);
            this.txt_Title.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(403, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Title";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(403, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "company";
            // 
            // txt_company
            // 
            this.txt_company.Location = new System.Drawing.Point(464, 51);
            this.txt_company.Name = "txt_company";
            this.txt_company.Size = new System.Drawing.Size(193, 20);
            this.txt_company.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "yearmade";
            // 
            // txt_yearmade
            // 
            this.txt_yearmade.Location = new System.Drawing.Point(464, 78);
            this.txt_yearmade.MaxLength = 4;
            this.txt_yearmade.Name = "txt_yearmade";
            this.txt_yearmade.Size = new System.Drawing.Size(35, 20);
            this.txt_yearmade.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(403, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "genre";
            // 
            // txt_genre
            // 
            this.txt_genre.Location = new System.Drawing.Point(464, 104);
            this.txt_genre.Name = "txt_genre";
            this.txt_genre.Size = new System.Drawing.Size(104, 20);
            this.txt_genre.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(505, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "grade";
            // 
            // txt_grade
            // 
            this.txt_grade.Location = new System.Drawing.Point(545, 78);
            this.txt_grade.MaxLength = 2;
            this.txt_grade.Name = "txt_grade";
            this.txt_grade.Size = new System.Drawing.Size(23, 20);
            this.txt_grade.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(663, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "description";
            // 
            // txt_description
            // 
            this.txt_description.Location = new System.Drawing.Point(663, 68);
            this.txt_description.Multiline = true;
            this.txt_description.Name = "txt_description";
            this.txt_description.Size = new System.Drawing.Size(250, 56);
            this.txt_description.TabIndex = 13;
            // 
            // ddlPlatform
            // 
            this.ddlPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPlatform.FormattingEnabled = true;
            this.ddlPlatform.Location = new System.Drawing.Point(219, 28);
            this.ddlPlatform.Name = "ddlPlatform";
            this.ddlPlatform.Size = new System.Drawing.Size(121, 21);
            this.ddlPlatform.TabIndex = 16;
            // 
            // pnlBoxArt
            // 
            this.pnlBoxArt.AutoScroll = true;
            this.pnlBoxArt.Location = new System.Drawing.Point(12, 214);
            this.pnlBoxArt.Name = "pnlBoxArt";
            this.pnlBoxArt.Size = new System.Drawing.Size(385, 180);
            this.pnlBoxArt.TabIndex = 17;
            this.pnlBoxArt.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBoxArt_Paint);
            // 
            // pnlScreenshots
            // 
            this.pnlScreenshots.AutoScroll = true;
            this.pnlScreenshots.Location = new System.Drawing.Point(12, 413);
            this.pnlScreenshots.Name = "pnlScreenshots";
            this.pnlScreenshots.Size = new System.Drawing.Size(385, 180);
            this.pnlScreenshots.TabIndex = 18;
            // 
            // pnlBoxFront
            // 
            this.pnlBoxFront.AllowDrop = true;
            this.pnlBoxFront.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlBoxFront.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBoxFront.Controls.Add(this.txt_BoxFront);
            this.pnlBoxFront.Controls.Add(this.label7);
            this.pnlBoxFront.Controls.Add(this.btnBoxFrontView);
            this.pnlBoxFront.Controls.Add(this.btnDelFront);
            this.pnlBoxFront.Location = new System.Drawing.Point(406, 130);
            this.pnlBoxFront.Name = "pnlBoxFront";
            this.pnlBoxFront.Size = new System.Drawing.Size(250, 140);
            this.pnlBoxFront.TabIndex = 19;
            this.pnlBoxFront.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlBoxFront.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // txt_BoxFront
            // 
            this.txt_BoxFront.Location = new System.Drawing.Point(-2, 81);
            this.txt_BoxFront.Name = "txt_BoxFront";
            this.txt_BoxFront.Size = new System.Drawing.Size(225, 20);
            this.txt_BoxFront.TabIndex = 34;
            this.txt_BoxFront.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Box Front";
            // 
            // btnBoxFrontView
            // 
            this.btnBoxFrontView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBoxFrontView.Location = new System.Drawing.Point(199, 3);
            this.btnBoxFrontView.Name = "btnBoxFrontView";
            this.btnBoxFrontView.Size = new System.Drawing.Size(20, 21);
            this.btnBoxFrontView.TabIndex = 45;
            this.btnBoxFrontView.Text = "V";
            this.btnBoxFrontView.UseVisualStyleBackColor = true;
            this.btnBoxFrontView.Click += new System.EventHandler(this.btnBoxFrontView_Click);
            // 
            // btnDelFront
            // 
            this.btnDelFront.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelFront.Location = new System.Drawing.Point(225, 3);
            this.btnDelFront.Name = "btnDelFront";
            this.btnDelFront.Size = new System.Drawing.Size(20, 21);
            this.btnDelFront.TabIndex = 40;
            this.btnDelFront.Text = "X";
            this.btnDelFront.UseVisualStyleBackColor = true;
            this.btnDelFront.Click += new System.EventHandler(this.btnDelFront_Click);
            // 
            // pnlBoxBack
            // 
            this.pnlBoxBack.AllowDrop = true;
            this.pnlBoxBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlBoxBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBoxBack.Controls.Add(this.txt_BoxBack);
            this.pnlBoxBack.Controls.Add(this.label8);
            this.pnlBoxBack.Controls.Add(this.btnBoxBackView);
            this.pnlBoxBack.Controls.Add(this.btnDelBack);
            this.pnlBoxBack.Location = new System.Drawing.Point(663, 130);
            this.pnlBoxBack.Name = "pnlBoxBack";
            this.pnlBoxBack.Size = new System.Drawing.Size(250, 140);
            this.pnlBoxBack.TabIndex = 20;
            this.pnlBoxBack.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlBoxBack.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // txt_BoxBack
            // 
            this.txt_BoxBack.Location = new System.Drawing.Point(-2, 81);
            this.txt_BoxBack.Name = "txt_BoxBack";
            this.txt_BoxBack.Size = new System.Drawing.Size(225, 20);
            this.txt_BoxBack.TabIndex = 35;
            this.txt_BoxBack.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Box Back";
            // 
            // btnBoxBackView
            // 
            this.btnBoxBackView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBoxBackView.Location = new System.Drawing.Point(199, 3);
            this.btnBoxBackView.Name = "btnBoxBackView";
            this.btnBoxBackView.Size = new System.Drawing.Size(20, 21);
            this.btnBoxBackView.TabIndex = 46;
            this.btnBoxBackView.Text = "V";
            this.btnBoxBackView.UseVisualStyleBackColor = true;
            this.btnBoxBackView.Click += new System.EventHandler(this.btnBoxBackView_Click);
            // 
            // btnDelBack
            // 
            this.btnDelBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelBack.Location = new System.Drawing.Point(224, 3);
            this.btnDelBack.Name = "btnDelBack";
            this.btnDelBack.Size = new System.Drawing.Size(20, 21);
            this.btnDelBack.TabIndex = 41;
            this.btnDelBack.Text = "X";
            this.btnDelBack.UseVisualStyleBackColor = true;
            this.btnDelBack.Click += new System.EventHandler(this.btnDelBack_Click);
            // 
            // pnlShotTitle
            // 
            this.pnlShotTitle.AllowDrop = true;
            this.pnlShotTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlShotTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlShotTitle.Controls.Add(this.txt_BoxTitle);
            this.pnlShotTitle.Controls.Add(this.btnBoxTitleView);
            this.pnlShotTitle.Controls.Add(this.btnDelTitle);
            this.pnlShotTitle.Controls.Add(this.label9);
            this.pnlShotTitle.Location = new System.Drawing.Point(406, 276);
            this.pnlShotTitle.Name = "pnlShotTitle";
            this.pnlShotTitle.Size = new System.Drawing.Size(250, 140);
            this.pnlShotTitle.TabIndex = 20;
            this.pnlShotTitle.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlShotTitle.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // txt_BoxTitle
            // 
            this.txt_BoxTitle.Location = new System.Drawing.Point(-2, 81);
            this.txt_BoxTitle.Name = "txt_BoxTitle";
            this.txt_BoxTitle.Size = new System.Drawing.Size(225, 20);
            this.txt_BoxTitle.TabIndex = 35;
            this.txt_BoxTitle.Visible = false;
            // 
            // btnBoxTitleView
            // 
            this.btnBoxTitleView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBoxTitleView.Location = new System.Drawing.Point(199, 3);
            this.btnBoxTitleView.Name = "btnBoxTitleView";
            this.btnBoxTitleView.Size = new System.Drawing.Size(20, 21);
            this.btnBoxTitleView.TabIndex = 47;
            this.btnBoxTitleView.Text = "V";
            this.btnBoxTitleView.UseVisualStyleBackColor = true;
            this.btnBoxTitleView.Click += new System.EventHandler(this.btnBoxTitleView_Click);
            // 
            // btnDelTitle
            // 
            this.btnDelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelTitle.Location = new System.Drawing.Point(225, 3);
            this.btnDelTitle.Name = "btnDelTitle";
            this.btnDelTitle.Size = new System.Drawing.Size(20, 21);
            this.btnDelTitle.TabIndex = 42;
            this.btnDelTitle.Text = "X";
            this.btnDelTitle.UseVisualStyleBackColor = true;
            this.btnDelTitle.Click += new System.EventHandler(this.btnDelTitle_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Screenshot Title screen";
            // 
            // pnlShotIngame
            // 
            this.pnlShotIngame.AllowDrop = true;
            this.pnlShotIngame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlShotIngame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlShotIngame.Controls.Add(this.txt_BoxIngame);
            this.pnlShotIngame.Controls.Add(this.label10);
            this.pnlShotIngame.Controls.Add(this.btnBoxIngameView);
            this.pnlShotIngame.Controls.Add(this.btnDelIngame);
            this.pnlShotIngame.Location = new System.Drawing.Point(663, 276);
            this.pnlShotIngame.Name = "pnlShotIngame";
            this.pnlShotIngame.Size = new System.Drawing.Size(250, 140);
            this.pnlShotIngame.TabIndex = 20;
            this.pnlShotIngame.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlShotIngame.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // txt_BoxIngame
            // 
            this.txt_BoxIngame.Location = new System.Drawing.Point(-2, 81);
            this.txt_BoxIngame.Name = "txt_BoxIngame";
            this.txt_BoxIngame.Size = new System.Drawing.Size(225, 20);
            this.txt_BoxIngame.TabIndex = 35;
            this.txt_BoxIngame.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Screenshot In Game";
            // 
            // btnBoxIngameView
            // 
            this.btnBoxIngameView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBoxIngameView.Location = new System.Drawing.Point(199, 3);
            this.btnBoxIngameView.Name = "btnBoxIngameView";
            this.btnBoxIngameView.Size = new System.Drawing.Size(20, 21);
            this.btnBoxIngameView.TabIndex = 48;
            this.btnBoxIngameView.Text = "V";
            this.btnBoxIngameView.UseVisualStyleBackColor = true;
            this.btnBoxIngameView.Click += new System.EventHandler(this.btnBoxIngameView_Click);
            // 
            // btnDelIngame
            // 
            this.btnDelIngame.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelIngame.Location = new System.Drawing.Point(224, 3);
            this.btnDelIngame.Name = "btnDelIngame";
            this.btnDelIngame.Size = new System.Drawing.Size(20, 21);
            this.btnDelIngame.TabIndex = 43;
            this.btnDelIngame.Text = "X";
            this.btnDelIngame.UseVisualStyleBackColor = true;
            this.btnDelIngame.Click += new System.EventHandler(this.btnDelIngame_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(820, 422);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(748, 422);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCancelBatch
            // 
            this.btnCancelBatch.Location = new System.Drawing.Point(663, 422);
            this.btnCancelBatch.Name = "btnCancelBatch";
            this.btnCancelBatch.Size = new System.Drawing.Size(79, 23);
            this.btnCancelBatch.TabIndex = 23;
            this.btnCancelBatch.Text = "Cancel Batch";
            this.btnCancelBatch.UseVisualStyleBackColor = true;
            this.btnCancelBatch.Click += new System.EventHandler(this.btnCancelBatch_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Search Term";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(216, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "System";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(404, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Rom";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 198);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Box Art Found Online";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 397);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(132, 13);
            this.label15.TabIndex = 32;
            this.label15.Text = "Screenshots Found Online";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(461, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(28, 13);
            this.lblPath.TabIndex = 33;
            this.lblPath.Text = "path";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(663, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(42, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Manual";
            // 
            // btnNewManual
            // 
            this.btnNewManual.Location = new System.Drawing.Point(831, 26);
            this.btnNewManual.Name = "btnNewManual";
            this.btnNewManual.Size = new System.Drawing.Size(24, 20);
            this.btnNewManual.TabIndex = 36;
            this.btnNewManual.Text = "..";
            this.btnNewManual.UseVisualStyleBackColor = true;
            this.btnNewManual.Click += new System.EventHandler(this.btnNewManual_Click);
            // 
            // txt_Manual
            // 
            this.txt_Manual.AllowDrop = true;
            this.txt_Manual.Location = new System.Drawing.Point(711, 26);
            this.txt_Manual.Name = "txt_Manual";
            this.txt_Manual.Size = new System.Drawing.Size(117, 20);
            this.txt_Manual.TabIndex = 35;
            this.txt_Manual.DragDrop += new System.Windows.Forms.DragEventHandler(this.txt_Manual_DragDrop);
            this.txt_Manual.DragEnter += new System.Windows.Forms.DragEventHandler(this.txt_Manual_DragEnter);
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(858, 25);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(55, 23);
            this.btnManual.TabIndex = 34;
            this.btnManual.Text = "View";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // chk_Visible
            // 
            this.chk_Visible.AutoSize = true;
            this.chk_Visible.Location = new System.Drawing.Point(587, 81);
            this.chk_Visible.Name = "chk_Visible";
            this.chk_Visible.Size = new System.Drawing.Size(56, 17);
            this.chk_Visible.TabIndex = 38;
            this.chk_Visible.Text = "Visible";
            this.chk_Visible.UseVisualStyleBackColor = true;
            // 
            // chk_Favourite
            // 
            this.chk_Favourite.AutoSize = true;
            this.chk_Favourite.Location = new System.Drawing.Point(587, 106);
            this.chk_Favourite.Name = "chk_Favourite";
            this.chk_Favourite.Size = new System.Drawing.Size(70, 17);
            this.chk_Favourite.TabIndex = 39;
            this.chk_Favourite.Text = "Favourite";
            this.chk_Favourite.UseVisualStyleBackColor = true;
            // 
            // btnViewFanart
            // 
            this.btnViewFanart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewFanart.Location = new System.Drawing.Point(199, 4);
            this.btnViewFanart.Name = "btnViewFanart";
            this.btnViewFanart.Size = new System.Drawing.Size(20, 21);
            this.btnViewFanart.TabIndex = 52;
            this.btnViewFanart.Text = "V";
            this.btnViewFanart.UseVisualStyleBackColor = true;
            this.btnViewFanart.Click += new System.EventHandler(this.btnViewFanart_Click);
            // 
            // btnDeleteFanart
            // 
            this.btnDeleteFanart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteFanart.Location = new System.Drawing.Point(224, 4);
            this.btnDeleteFanart.Name = "btnDeleteFanart";
            this.btnDeleteFanart.Size = new System.Drawing.Size(20, 21);
            this.btnDeleteFanart.TabIndex = 51;
            this.btnDeleteFanart.Text = "X";
            this.btnDeleteFanart.UseVisualStyleBackColor = true;
            this.btnDeleteFanart.Click += new System.EventHandler(this.btnDeleteFanart_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(37, 13);
            this.label17.TabIndex = 50;
            this.label17.Text = "Fanart";
            // 
            // pnlFanart
            // 
            this.pnlFanart.AllowDrop = true;
            this.pnlFanart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlFanart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlFanart.Controls.Add(this.txt_Fanart);
            this.pnlFanart.Controls.Add(this.btnViewFanart);
            this.pnlFanart.Controls.Add(this.btnDeleteFanart);
            this.pnlFanart.Controls.Add(this.label17);
            this.pnlFanart.Location = new System.Drawing.Point(407, 422);
            this.pnlFanart.Name = "pnlFanart";
            this.pnlFanart.Size = new System.Drawing.Size(250, 140);
            this.pnlFanart.TabIndex = 49;
            this.pnlFanart.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
            this.pnlFanart.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
            // 
            // txt_Fanart
            // 
            this.txt_Fanart.Location = new System.Drawing.Point(-2, 81);
            this.txt_Fanart.Name = "txt_Fanart";
            this.txt_Fanart.Size = new System.Drawing.Size(225, 20);
            this.txt_Fanart.TabIndex = 35;
            this.txt_Fanart.Visible = false;
            // 
            // searchProgressBar
            // 
            this.searchProgressBar.Location = new System.Drawing.Point(12, 164);
            this.searchProgressBar.Name = "searchProgressBar";
            this.searchProgressBar.Size = new System.Drawing.Size(201, 23);
            this.searchProgressBar.TabIndex = 50;
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchLabel.Location = new System.Drawing.Point(219, 174);
            this.searchLabel.MinimumSize = new System.Drawing.Size(20, 0);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(20, 13);
            this.searchLabel.TabIndex = 51;
            // 
            // Conf_OnlineLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 595);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchProgressBar);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pnlFanart);
            this.Controls.Add(this.chk_Favourite);
            this.Controls.Add(this.chk_Visible);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnNewManual);
            this.Controls.Add(this.txt_Manual);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnCancelBatch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlShotIngame);
            this.Controls.Add(this.pnlShotTitle);
            this.Controls.Add(this.pnlBoxBack);
            this.Controls.Add(this.pnlBoxFront);
            this.Controls.Add(this.pnlScreenshots);
            this.Controls.Add(this.pnlBoxArt);
            this.Controls.Add(this.ddlPlatform);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_description);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_grade);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_genre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_yearmade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_company);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_Title);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txt_SearchTerm);
            this.Name = "Conf_OnlineLookup";
            this.ShowIcon = false;
            this.Text = "Update Game";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Conf_OnlineLookup_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.pnlBoxFront.ResumeLayout(false);
            this.pnlBoxFront.PerformLayout();
            this.pnlBoxBack.ResumeLayout(false);
            this.pnlBoxBack.PerformLayout();
            this.pnlShotTitle.ResumeLayout(false);
            this.pnlShotTitle.PerformLayout();
            this.pnlShotIngame.ResumeLayout(false);
            this.pnlShotIngame.PerformLayout();
            this.pnlFanart.ResumeLayout(false);
            this.pnlFanart.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_SearchTerm;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txt_Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_company;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_yearmade;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_genre;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_grade;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_description;
        private System.Windows.Forms.ComboBox ddlPlatform;
        private System.Windows.Forms.FlowLayoutPanel pnlBoxArt;
        private System.Windows.Forms.FlowLayoutPanel pnlScreenshots;
        private System.Windows.Forms.Panel pnlBoxFront;
        private System.Windows.Forms.Panel pnlBoxBack;
        private System.Windows.Forms.Panel pnlShotTitle;
        private System.Windows.Forms.Panel pnlShotIngame;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCancelBatch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewTextBoxColumn yearmade;
        private System.Windows.Forms.DataGridViewTextBoxColumn genre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.TextBox txt_BoxFront;
        private System.Windows.Forms.TextBox txt_BoxBack;
        private System.Windows.Forms.TextBox txt_BoxTitle;
        private System.Windows.Forms.TextBox txt_BoxIngame;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnNewManual;
        private System.Windows.Forms.TextBox txt_Manual;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.CheckBox chk_Visible;
        private System.Windows.Forms.CheckBox chk_Favourite;
        private System.Windows.Forms.Button btnDelFront;
        private System.Windows.Forms.Button btnDelBack;
        private System.Windows.Forms.Button btnDelTitle;
        private System.Windows.Forms.Button btnDelIngame;
        private System.Windows.Forms.Button btnBoxFrontView;
        private System.Windows.Forms.Button btnBoxBackView;
        private System.Windows.Forms.Button btnBoxTitleView;
        private System.Windows.Forms.Button btnBoxIngameView;
        private System.Windows.Forms.Button btnViewFanart;
        private System.Windows.Forms.Button btnDeleteFanart;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel pnlFanart;
        private System.Windows.Forms.TextBox txt_Fanart;
        private System.Windows.Forms.ProgressBar searchProgressBar;
        private System.Windows.Forms.Label searchLabel;
    }
}

