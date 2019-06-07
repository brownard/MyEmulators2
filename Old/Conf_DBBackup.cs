using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_DBBackup : myEmulators.ContentPanel
    {
        public Conf_DBBackup()
        {
            InitializeComponent();
        }

        private void restoreBrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Open meta data file";
            diag.Filter = "XML files (*.xml) | *.xml|All files (*.*) | *.*";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                restoreLocationText.Text = diag.FileName;
            }
        }

        private void backupBrowse_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Title = "Save meta data file";
            diag.Filter = "XML files (*.xml) | *.xml|All files (*.*) | *.*";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                backupLocationText.Text = diag.FileName;
            }
        }

        private void restore_Click_1(object sender, EventArgs e)
        {
            if (DBSync.restore(restoreLocationText.Text))
            {
                MessageBox.Show("Done!", "Restoring database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Could not restore. Please check that the path \nspecified is correct and that the file is valid.", "Restoring database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backup_Click(object sender, EventArgs e)
        {
            if (DBSync.backup(backupLocationText.Text))
            {
                MessageBox.Show("Done!", "Backup database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Could not backup. Please check that the path \nspecified is correct and that you have write permissions.", "Backup database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

