using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MyEmulators2
{
    public partial class Conf_RestoreDlg : Form
    {
        DBBackup dbBackup = null;

        Thread worker = null;

        string path;
        MergeType emuMergeType = MergeType.Create;
        MergeType profileMergeType = MergeType.Create;
        MergeType gameMergeType = MergeType.Ignore;

        bool backup = false;
        bool clean = false;

        bool backupThumbs = true;
        public bool BackupThumbs
        {
            get { return backupThumbs; }
            set { backupThumbs = value; }
        }

        bool restoreThumbs = true;
        public bool RestoreThumbs
        {
            get { return restoreThumbs; }
            set { restoreThumbs = value; }
        }

        internal Conf_RestoreDlg(string path, RestoreType restoreType)
        {
            InitializeComponent();

            if (restoreType == RestoreType.Backup)
            {
                backup = true;
                this.Text = "Backup";
            }
            else
                clean = true;

            this.path = path;
            this.FormClosing += new FormClosingEventHandler(Conf_RestoreDlg_FormClosing);
        }

        internal Conf_RestoreDlg(string path, MergeType emuMergeType, MergeType profileMergeType, MergeType gameMergeType)
        {
            InitializeComponent();

            this.path = path;
            this.emuMergeType = emuMergeType;
            this.profileMergeType = profileMergeType;
            this.gameMergeType = gameMergeType;

            this.FormClosing += new FormClosingEventHandler(Conf_RestoreDlg_FormClosing);
        }

        void Conf_RestoreDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker != null && worker.IsAlive)
                e.Cancel = true;
        }

        private void Conf_RestoreDlg_Load(object sender, EventArgs e)
        {
            dbBackup = new DBBackup();
            dbBackup.BackupThumbs = backupThumbs;
            dbBackup.RestoreThumbs = restoreThumbs;

            dbBackup.OnBackupProgress += new BackupProgressHandler(dbBackup_OnBackupProgress);
            dbBackup.OnBackupDataError += new BackupDataErrorHandler(dbBackup_OnBackupDataError);

            string lPath = path;
            bool isBackup = backup;

            worker = new Thread(new ThreadStart(delegate()
                {
                    if (isBackup)
                        dbBackup.Backup(lPath);
                    else
                        dbBackup.Restore(lPath, emuMergeType, profileMergeType, gameMergeType, clean);
                }));
            worker.Start();
        }

        void dbBackup_OnBackupDataError(DataErrorType errorType, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { dbBackup_OnBackupDataError(errorType, message); }));
                return;
            }

            bool shouldContinue = false;
            if (errorType == DataErrorType.InvalidData || errorType == DataErrorType.SQLError)
            {
                shouldContinue =
                    MessageBox.Show(string.Format("Error with backup data: {0}\r\nWould you like to continue and skip this item?", message), "Error", MessageBoxButtons.YesNo)
                        == System.Windows.Forms.DialogResult.Yes;
                dbBackup.ShouldContinue = shouldContinue;
            }
            else
                MessageBox.Show(string.Format("Error: {0}", message), "Error", MessageBoxButtons.OK);

            if (!shouldContinue)
            {
                if (worker != null && worker.IsAlive)
                    worker.Join();
                Close();
            }
        }

        void dbBackup_OnBackupProgress(int perc, int currentItem, int totalItems, string message, params object[] args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { dbBackup_OnBackupProgress(perc, currentItem, totalItems, message, args); }));
                return;
            }

            progressBar1.Value = perc;

            if (perc == 100 && currentItem == 0 && totalItems == 0)
            {
                label1.Text = "Complete";
                if (worker != null && worker.IsAlive)
                    worker.Join();

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                message = string.Format(message, args);
                label1.Text = string.Format("{0} / {1} - {2}", currentItem, totalItems, message);
            }
        }
    }

    enum RestoreType
    {
        Backup,
        Restore
    }
}
