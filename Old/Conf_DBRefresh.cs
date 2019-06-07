using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_DBRefresh : myEmulators.ContentPanel
    {
        public Conf_DBRefresh()
        {
            InitializeComponent();

            autoRefreshGames.Checked = Options.Instance.GetBoolOption("autorefreshgames");
            autoImportCheckBox.Checked = Options.Instance.GetBoolOption("autoimportgames");
            exactMatchCheckBox.Checked = Options.Instance.GetBoolOption("importexact");
            approveTopCheckBox.Checked = Options.Instance.GetBoolOption("importtop");
            resizeThumbCheckBox.Checked = Options.Instance.GetBoolOption("resizethumbs");

            autoRefreshGames.CheckedChanged += new EventHandler(changesMade);
            autoImportCheckBox.CheckedChanged += new EventHandler(changesMade);
            exactMatchCheckBox.CheckedChanged += new EventHandler(changesMade);
            approveTopCheckBox.CheckedChanged += new EventHandler(changesMade);
            resizeThumbCheckBox.CheckedChanged += new EventHandler(changesMade);

            exactMatchCheckBox.CheckedChanged += new EventHandler(updateImportSettings);
            approveTopCheckBox.CheckedChanged += new EventHandler(updateImportSettings);
        }

        void changesMade(object sender, EventArgs e)
        {
            OnChange(this, e);
        }

        private void romRefreshButton_Click(object sender, EventArgs e)
        {
            //DB.refreshGamesDatabase();
            //MessageBox.Show("Done!", "Refresh Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            new Conf_RefreshDialog().ShowDialog();
        }

        public override void save()
        {
            Options.Instance.UpdateOption("autorefreshgames", autoRefreshGames.Checked);
            Options.Instance.UpdateOption("autoimportgames", autoImportCheckBox.Checked);
            Options.Instance.UpdateOption("importexact", exactMatchCheckBox.Checked);
            Options.Instance.UpdateOption("importtop", approveTopCheckBox.Checked);
            Options.Instance.UpdateOption("resizethumbs", resizeThumbCheckBox.Checked);
            base.save();
        }

        private void cleanButton_Click(object sender, EventArgs e)
        {
            //DB.cleanGameDatabase();
            new Conf_RefreshDialog(true).ShowDialog();

            //if (removeThumbCheckBox.Checked)
                //ThumbsHandler.Instance.removeUnusedThumbs();
            //MessageBox.Show("Done!", "Refresh Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void updateImportSettings(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null)
                return;
            if(cb.Checked)
            {
                if (cb.Name == "exactMatchCheckBox")
                    approveTopCheckBox.Checked = false;
                else if (cb.Name == "approveTopCheckBox")
                    exactMatchCheckBox.Checked = false;
            }
        }

    }
}

