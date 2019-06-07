using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    class EmulatorScraperHandler
    {
        public EmulatorInfo UpdateEmuInfo(string platformText, Func<object, bool> completedDelegate)
        {
            return updateEmuInfo(platformText, null, null, completedDelegate);
        }

        EmulatorInfo updateEmuInfo(string platformText, string selectedKey, Dictionary<string, string> emuInfos, Func<object, bool> completedDelegate)
        {
            EmulatorInfo emuInfo = null;
            bool completed = false;

            BackgroundTaskHandler handler = new BackgroundTaskHandler();
            handler.ActionDelegate = () =>
            {
                handler.ExecuteProgressHandler(0, "Looking up platforms...");
                string selectedId;
                if (emuInfos == null || string.IsNullOrEmpty(selectedKey))
                {
                    emuInfos = new EmulatorScraper().GetEmulators(platformText, out selectedKey);
                    if (selectedKey == null || !emuInfos.TryGetValue(selectedKey, out selectedId))
                        return;
                }
                else if (!emuInfos.TryGetValue(selectedKey, out selectedId))
                    return;

                handler.ExecuteProgressHandler(33, "Retrieving info for " + selectedKey);
                emuInfo = new EmulatorScraper().GetInfo(selectedId);
                if (emuInfo == null)
                    return;

                handler.ExecuteProgressHandler(67, "Updating " + emuInfo.Title);

                if (completedDelegate != null)
                    completed = completedDelegate(emuInfo);
                else
                    completed = true;
            };

            using (Conf_ProgressDialog progressDlg = new Conf_ProgressDialog(handler))
                progressDlg.ShowDialog();

            if (completed)
                return emuInfo;

            if (emuInfos != null && string.IsNullOrEmpty(selectedKey))
            {
                using (Conf_EmuLookupDialog lookupDlg = new Conf_EmuLookupDialog(emuInfos))
                {
                    if (lookupDlg.ShowDialog() == DialogResult.OK && lookupDlg.SelectedKey != null)
                        return updateEmuInfo(null, lookupDlg.SelectedKey, emuInfos, completedDelegate);
                }
            }
            else
                MessageBox.Show("Error retrieving online info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }
    }
}
