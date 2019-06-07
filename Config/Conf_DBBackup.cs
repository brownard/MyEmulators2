using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    internal partial class Conf_DBBackup : ContentPanel
    {
        public Conf_DBBackup()
        {
            InitializeComponent();

            Options opts = Options.Instance;

            backupPathTextBox.Text = opts.GetStringOption("backupfile");
            backupThumbsCheckBox.Checked = opts.GetBoolOption("backupthumbs");

            restorePathTextBox.Text = opts.GetStringOption("restorefile");
            restoreThumbsCheckBox.Checked = opts.GetBoolOption("restorethumbs");
            mergeRadioButton.Checked = opts.GetBoolOption("restoremerge");

            BackupDropdownItem create = new BackupDropdownItem("Create new", MergeType.Create);
            BackupDropdownItem ignore = new BackupDropdownItem("Ignore", MergeType.Ignore);
            BackupDropdownItem merge = new BackupDropdownItem("Merge", MergeType.Merge);

            //emu merge settings
            emuMergeComboBox.DisplayMember = "DisplayMember";
            emuMergeComboBox.ValueMember = "ValueMember";
            emuMergeComboBox.Items.AddRange(new object[] { create, ignore, merge }); //allow all 3 options

            int index = sanitiseIndex(opts.GetIntOption("restoreemusetting"), emuMergeComboBox.Items.Count);
            emuMergeComboBox.SelectedItem = emuMergeComboBox.Items[index];

            //profiles
            profileMergeComboBox.DisplayMember = "DisplayMember";
            profileMergeComboBox.ValueMember = "ValueMember";
            profileMergeComboBox.Items.AddRange(new object[] { create, ignore }); //don't allow Merge

            index = sanitiseIndex(opts.GetIntOption("restoreprofilesetting"), profileMergeComboBox.Items.Count);
            profileMergeComboBox.SelectedItem = profileMergeComboBox.Items[index];

            //games
            gameMergeComboBox.DisplayMember = "DisplayMember";
            gameMergeComboBox.ValueMember = "ValueMember";
            gameMergeComboBox.Items.AddRange(new object[] { ignore, merge }); //don't allow Create

            index = sanitiseIndex(opts.GetIntOption("restoregamesetting"), gameMergeComboBox.Items.Count);
            gameMergeComboBox.SelectedItem = gameMergeComboBox.Items[index];

            updateMergePanels();
        }

        //restore from specified xml
        private void restoreButton_Click(object sender, EventArgs e)
        {
            Conf_RestoreDlg dlg;
            if (mergeRadioButton.Checked) //if we're merging
            {
                //get merge settings
                MergeType emuMergeType = ((BackupDropdownItem)emuMergeComboBox.SelectedItem).ValueMember;
                MergeType profileMergeType = ((BackupDropdownItem)profileMergeComboBox.SelectedItem).ValueMember;
                MergeType gameMergeType = ((BackupDropdownItem)gameMergeComboBox.SelectedItem).ValueMember;

                //create restore dialog with merge settings
                dlg = new Conf_RestoreDlg(restorePathTextBox.Text, emuMergeType, profileMergeType, gameMergeType);
            }
            else //else clean restore
            {
                //warn user of db deletion
                DialogResult shouldClean = MessageBox.Show("All existing data in the database will be deleted.\r\nAre you sure you want to continue?", "Clean restore", MessageBoxButtons.YesNo);
                if (shouldClean != DialogResult.Yes)
                    return;

                //create restore dialog
                dlg = new Conf_RestoreDlg(restorePathTextBox.Text, RestoreType.Restore);
            }
            dlg.RestoreThumbs = restoreThumbsCheckBox.Checked;
            //display dialog (starts restore)
            if (dlg.ShowDialog() == DialogResult.OK)
                MessageBox.Show("Restore completed successfully.");
            dlg.Dispose();
        }

        //update merge panel enabled status
        private void mergeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            updateMergePanels();
        }

        void updateMergePanels()
        {
            if (mergeRadioButton.Checked)
                mergeGroupBox.Enabled = true;
            else
                mergeGroupBox.Enabled = false;
        }
        
        //display dialog and start db backup
        private void backupButton_Click(object sender, EventArgs e)
        {
            using (Conf_RestoreDlg dlg = new Conf_RestoreDlg(backupPathTextBox.Text, RestoreType.Backup))
            {
                dlg.BackupThumbs = backupThumbsCheckBox.Checked;
                if (dlg.ShowDialog() == DialogResult.OK)
                    MessageBox.Show("Backup completed successfully.");
            }
        }

        //show dialog allowing user to specify where to save backup
        private void backupPathButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save file as...";
                dlg.Filter = "XML (*.xml)|*.xml";

                string initialDirectory = null;
                string path = backupPathTextBox.Text;

                if (!string.IsNullOrEmpty(path))
                {
                    int index = path.LastIndexOf("\\");
                    if (index > -1)
                        path = path.Remove(index);
                    if (System.IO.Directory.Exists(path))
                        initialDirectory = path;
                }
                if (initialDirectory == null)
                    initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                dlg.RestoreDirectory = true;
                dlg.InitialDirectory = initialDirectory;

                if (dlg.ShowDialog() == DialogResult.OK)
                    backupPathTextBox.Text = dlg.FileName;
            }
        }

        //show dialog allowing user to select path to backup
        private void restorePathButton_Click(object sender, EventArgs e)
        {
            string title = "Select backup file...";
            string filter = "XML (*.xml)|*.xml";
            string initialDirectory = null;
            string path = restorePathTextBox.Text;

            if (!string.IsNullOrEmpty(path))
            {
                int index = path.LastIndexOf("\\");
                if (index > -1)
                    path = path.Remove(index);
                if (System.IO.Directory.Exists(path))
                    initialDirectory = path;
            }
            if (initialDirectory == null)
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (OpenFileDialog dlg = Emulators2Settings.OpenFileDialog(title, filter, initialDirectory))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    restorePathTextBox.Text = dlg.FileName;
            }
        }

        int sanitiseIndex(int index, int itemCount)
        {
            if (index < 0)
                index = 0;
            else if (index >= itemCount)
                index = itemCount - 1;
           
            return index;
        }

        public override void ClosePanel()
        {
            Options opts = Options.Instance;
            opts.UpdateOption("backupfile", backupPathTextBox.Text);
            opts.UpdateOption("backupthumbs", backupThumbsCheckBox.Checked);

            opts.UpdateOption("restorefile", restorePathTextBox.Text);
            opts.UpdateOption("restorethumbs", restoreThumbsCheckBox.Checked);
            opts.UpdateOption("restoremerge", mergeRadioButton.Checked);

            opts.UpdateOption("restoreemusetting", emuMergeComboBox.SelectedIndex);
            opts.UpdateOption("restoreprofilesetting", profileMergeComboBox.SelectedIndex);
            opts.UpdateOption("restoregamesetting", gameMergeComboBox.SelectedIndex);
        }
    }

    class BackupDropdownItem
    {
        public BackupDropdownItem(string displayMember, MergeType mergeType)
        {
            DisplayMember = displayMember;
            ValueMember = mergeType;
        }
        public string DisplayMember { get; set; }
        public MergeType ValueMember { get; set; }
    }
}
