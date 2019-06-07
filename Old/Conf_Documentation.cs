using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace myEmulators
{
    class Conf_Documentation : ContentPanel
    {
        private Label label2;
        private Label label1;
        private Label label3;
        private LinkLabel linkLabel1;
        private Label label4;
        private LinkLabel linkLabel2;
        private PictureBox pictureBox1;

        public Conf_Documentation()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Conf_Documentation));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(314, 89);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 220);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(498, 42);
            this.label2.TabIndex = 8;
            this.label2.Text = "The main web site for the plugin is the Mediaportal forum. To get help, report a " +
                "bug, suggest improvements or just discuss the plugin, this is the place to go to" +
                ". The threads can be found here:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Documentation";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 55);
            this.label3.TabIndex = 9;
            this.label3.Text = "The latest version of the plugin is available from a subversion server, which can" +
                " be found here:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(16, 99);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(262, 13);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://forum.team-mediaportal.com/my-emulators-247/";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 214);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(292, 60);
            this.label4.TabIndex = 11;
            this.label4.Text = "A big thanks to the contributors: gamingexpert for the FAQ, ajp8164 for improveme" +
                "nts, TheOncleJuna for the images and all the great community!";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(16, 178);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(301, 13);
            this.linkLabel2.TabIndex = 12;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://myemulators.svn.sourceforge.net/svnroot/myemulators";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // Conf_Documentation
            // 
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Conf_Documentation";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = new System.Diagnostics.ProcessStartInfo();
            proc.StartInfo.FileName = "http://forum.team-mediaportal.com/my-emulators-247/";
            proc.Start();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = new System.Diagnostics.ProcessStartInfo();
            proc.StartInfo.FileName = "https://myemulators.svn.sourceforge.net/svnroot/myemulators";
            proc.Start();
        }
    }

    //Don't need designer for these, 
    //create them by code instead

    class Conf_Readme : ContentPanel
    {
        RichTextBox box;

        public Conf_Readme()
        {
            box = new RichTextBox();
            box.Font = new System.Drawing.Font("Verdana", 10.0f);
            box.Top = 0;
            box.Left = 10;
            box.Width = this.Width - 14;
            box.Height = this.Height - 4;
            box.BorderStyle = BorderStyle.None;
            Stream input = Assembly.GetCallingAssembly().GetManifestResourceStream("myEmulators.Docs.Readme.txt");
            StreamReader readme = new StreamReader(input);
            box.Text = readme.ReadToEnd();
            readme.Close();
            input.Close();
            this.Controls.Add(box);
            this.BackColor = box.BackColor;
        }

        public override void resize(object sender, EventArgs e)
        {
            base.resize(sender, e);
            box.Width = this.Width - 14;
            box.Height = this.Height - 4;
        }
    }

    class Conf_FAQ : ContentPanel
    {
        private RichTextBox box;

        public Conf_FAQ()
        {
            box = new RichTextBox();
            box.Font = new System.Drawing.Font("Verdana", 10.0f);
            box.Top = 0;
            box.Left = 10;
            box.Width = this.Width - 14;
            box.Height = this.Height - 4;
            box.BorderStyle = BorderStyle.None;
            Stream input = Assembly.GetCallingAssembly().GetManifestResourceStream("myEmulators.Docs.FAQ.txt");
            StreamReader readme = new StreamReader(input);
            box.Text = readme.ReadToEnd();
            readme.Close();
            input.Close();
            this.Controls.Add(box);
            this.BackColor = box.BackColor;
        }

        public override void resize(object sender, EventArgs e)
        {
            base.resize(sender, e);
            box.Width = this.Width - 14;
            box.Height = this.Height - 4;
        }
    }
}
