using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_PCGames : myEmulators.ContentPanel
    {
        Emulator pcEmu = null;

        public Conf_PCGames()
        {
            InitializeComponent();

            pcEmu = Emulator.GetPC();

            foreach (Game item in DB.Instance.GetGames(pcEmu))
            {
                pcList.Items.Add(item);
            }
        }

        private void updateButtonEnablings()
        {
            updateButtonEnablings(this, EventArgs.Empty);
        }
        private void updateButtonEnablings(object sender, EventArgs e)
        {
            if (pcList.SelectedIndex < 0)
            {
                moveup.Enabled = false;
                movedown.Enabled = false;
                delete.Enabled = false;
            }
            else
            {
                delete.Enabled = true;
                if (pcList.SelectedIndex > 0)
                {
                    moveup.Enabled = true;
                }
                else
                {
                    moveup.Enabled = false;
                }
                if (pcList.SelectedIndex < pcList.Items.Count - 1)
                {
                    movedown.Enabled = true;
                }
                else
                {
                    movedown.Enabled = false;
                }
            }
        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.CheckFileExists = true;
            diag.CheckPathExists = true;
            diag.Filter = "Executables (*.bat, *.exe) | *.bat;*.exe";
            diag.Multiselect = false;
            diag.RestoreDirectory = true;
            diag.Title = "Select path to game";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                pathBox.Text = diag.FileName;
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            String pathArgsRemoved = pathBox.Text;
            if ((pathArgsRemoved.Contains(".exe") && !pathArgsRemoved.EndsWith(".exe")) || (pathArgsRemoved.Contains(".bat") && !pathArgsRemoved.EndsWith(".bat")))
            {
                pathArgsRemoved = pathBox.Text.Remove(pathBox.Text.LastIndexOf('.') + 4);
            }
            if(!(pathArgsRemoved.EndsWith(".exe") || pathArgsRemoved.EndsWith(".bat")) || !System.IO.File.Exists(pathArgsRemoved))
            {
                MessageBox.Show("Please specify a valid game executable path", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (titleBox.Text.Length == 0)
            {
                MessageBox.Show("Please specify a game title", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Game newGame = new Game(pathBox.Text, pcEmu);
                newGame.Title = titleBox.Text;

                pathBox.Text = "";
                titleBox.Text = "";

                pcList.Items.Add(newGame);
                //Sort the list according to titles
                Game[] games = new Game[pcList.Items.Count];
                String[] titles = new String[pcList.Items.Count];
                for (int i = 0; i < pcList.Items.Count; i++)
                {
                    games[i] = (Game)pcList.Items[i];
                    titles[i] = games[i].Title;
                }
                Array.Sort(titles, games);
                pcList.Items.Clear();
                foreach (Game item in games)
                {
                    pcList.Items.Add(item);
                }
                pcList.SelectedItem = newGame;

                updateButtonEnablings();
                OnChange(this, e);
            }
        }

        List<Game> gamesToDelete = new List<Game>();

        private void delete_Click(object sender, EventArgs e)
        {
            gamesToDelete.Add((Game)pcList.SelectedItem);
            pcList.Items.Remove(pcList.SelectedItem);
            updateButtonEnablings();
            OnChange(this, e);
        }

        private void pcList_KeyDown(object sender, KeyEventArgs e)
        {
            if (delete.Enabled && e.KeyCode == Keys.Delete)
            {
                delete_Click(sender, EventArgs.Empty);
            }
        }

        public override void save()
        {
            foreach (Game item in gamesToDelete)
            {
                item.Delete();
            }
            gamesToDelete = new List<Game>();
            foreach (Game item in pcList.Items)
            {
                item.Save();
            }
            base.save();
        }


        public override void update()
        {
            //game titles may have been changed in database view
            if (form == null)
                return;
            if (!form.Text.EndsWith("*")) //only update if all changes have been saved
            {
                pcList.Items.Clear();
                foreach (Game item in DB.Instance.GetGames(pcEmu))
                    pcList.Items.Add(item);
            }
            base.update();
        }

    }
}

