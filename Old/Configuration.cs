using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
           InitializeComponent();
        }

        //Return a flat list of the navigation tree
        private ContentPanel[] getAllContentPanels()
        {
            List<ContentPanel> flatList = new List<ContentPanel>();
            foreach (TreeNode node in navigationTree.Nodes)
            {
                flatList.Add((ContentPanel)node.Tag);
                //Only support one level of children
                foreach (TreeNode child in node.Nodes)
                {
                    flatList.Add((ContentPanel)child.Tag);
                }
            }
            return flatList.ToArray();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Configuration_FormClosing);

            Conf_GlobalSettings.Instance.ReInit();

            navigationTree.Nodes[0].Tag = new Conf_EmuBrowser(); //Conf_Emulators() { form = this };
            navigationTree.Nodes[1].Tag = new Conf_PCGames() { form = this };
            //navigationTree.Nodes[2].Tag = new Conf_Thumbs();
            navigationTree.Nodes[2].Tag = new Conf_DBImporter();
            navigationTree.Nodes[3].Tag = new Conf_DBBrowser();//new Conf_Database(); 
            navigationTree.Nodes[3].Nodes[0].Tag = new Conf_DBSync();
            navigationTree.Nodes[3].Nodes[1].Tag = new Conf_DBBackup();
            navigationTree.Nodes[3].Nodes[2].Tag = new Conf_DBRefresh();
            navigationTree.Nodes[4].Tag = new Conf_Documentation();
            navigationTree.Nodes[4].Nodes[0].Tag = new Conf_Readme();
            navigationTree.Nodes[4].Nodes[1].Tag = new Conf_FAQ();
            navigationTree.Nodes[5].Tag = new Conf_Options();

            Text = "Emulators 2 " + Options.getVersionNumber() + " Configuration";
            navigationTree.SelectedNode = navigationTree.Nodes[0];

            //ThumbsHandler.Instance.removeUnusedThumbs();
        }

        void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            Conf_GlobalSettings.Instance.Importer.Stop();
        }

        private void navigationTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ContentPanel replacer = (ContentPanel)navigationTree.SelectedNode.Tag;
            if (!replacer.isInitialized)
            {
                replacer.init();
                replacer.changeEvent += new UpdatedPanelEventHandler(panel_hasChanged);
            }
            else
            {
                replacer.update();
            }

            if (mainContents.Controls.Count > 0 && mainContents.Controls[0] as ContentPanel != null)
                ((ContentPanel)mainContents.Controls[0]).close();

            mainContents.Controls.Clear();
            mainContents.Controls.Add(replacer);
            //replacer.update();
            //Mark it
            foreach (TreeNode node in navigationTree.Nodes)
            {
                node.BackColor = System.Drawing.Color.White;
                foreach (TreeNode child in node.Nodes)
                {
                    child.BackColor = System.Drawing.Color.White;
                }
            }
            navigationTree.SelectedNode.BackColor = System.Drawing.Color.FromArgb(215, 215, 215);
        }

        void panel_hasChanged(object sender, EventArgs e)
        {
            if (!Text.EndsWith("*"))
            {
                Text += " *";
            }
        }

        private void apply_Click(object sender, EventArgs e)
        {
            if (ThumbsHandler.Instance.NeedThumbUpdate)
            {
                DialogResult msg = MessageBox.Show("Emulator titles have been changed, do you want to copy the associated game thumbs to the new directory?", "Update Thumbs", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (msg != System.Windows.Forms.DialogResult.Yes)
                    ThumbsHandler.Instance.NeedThumbUpdate = false;
            }
            foreach (ContentPanel x in getAllContentPanels())
            {
                x.save();
            }
            Options.Instance.Save();
            ThumbsHandler.Instance.NeedThumbUpdate = false;
            if (Text.EndsWith("*"))
            {
                Text = Text.Remove(Text.Length - 2);
            }
            ((ContentPanel)mainContents.Controls[0]).update();
        }

        private void save_Click(object sender, EventArgs e)
        {
            apply_Click(sender, e);
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                ((ContentPanel)mainContents.Controls[0]).keyDown(sender, e);
            }
        }

        private void Configuration_Resize(object sender, EventArgs e)
        {
            try
            {
                save.Left = this.Width - 95;
                save.Top = this.Height - 62;
                apply.Left = this.Width - 176;
                apply.Top = this.Height - 62;
                cancel.Top = this.Height - 62;
                panel1.Width = this.Width - 32;
                panel1.Top = this.Height - 81;
                navigationTree.Height = this.Height - 191;
                pictureBox1.Width = this.Width - 32;
                mainContents.Height = this.Height - 191;
                mainContents.Width = this.Width - 175;
                mainContents.Controls[0].Size = mainContents.Size;

                //The ones that aren't visible
                foreach (ContentPanel x in getAllContentPanels())
                {
                    x.Size = mainContents.Size;
                }
            }
            catch (Exception) { }
        }
    }
}