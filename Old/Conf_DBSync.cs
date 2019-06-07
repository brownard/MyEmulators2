using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_DBSync : myEmulators.ContentPanel
    {
        public Conf_DBSync()
        {
            InitializeComponent();
        }

        private void importBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Open meta data file";
            diag.Filter = "XML files (*.xml) | *.xml|All files (*.*) | *.*";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                importText.Text = diag.FileName;
            }
        }

        private void exportBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Title = "Save meta data file";
            diag.Filter = "XML files (*.xml) | *.xml|All files (*.*) | *.*";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                exportText.Text = diag.FileName;
            }
        }

        private void import_Click(object sender, EventArgs e)
        {
            List<Game> items = new List<Game>();
            foreach (Emulator emu in DB.Instance.GetEmulators())
            {
                foreach (Game game in DB.Instance.GetGames(emu))
                {
                    items.Add(game);
                }
            }
            if (DBSync.import(items.ToArray(), importText.Text, !noOverwritingCheck.Checked))
            {
                MessageBox.Show("Done!", "Importing meta data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Could not import. Please check that the path \nspecified is correct and that the file is valid.", "Importing meta data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void export_Click(object sender, EventArgs e)
        {
            List<Game> items = new List<Game>();
            foreach (Emulator emu in DB.Instance.GetEmulators())
            {
                foreach (Game game in DB.Instance.GetGames(emu))
                {
                    items.Add(game);
                }
            }
            if (DBSync.export(items.ToArray(), exportText.Text))
            {
                MessageBox.Show("Done!", "Exporting meta data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Could not export. Please check that the path \nspecified is correct and that you have write permissions.", "Exporting meta data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

