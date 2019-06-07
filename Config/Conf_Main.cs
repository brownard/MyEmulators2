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
    public partial class Conf_Main : Form
    {
        ContentPanel selectedPanel = null;

        public Conf_Main()
        {
            InitializeComponent();

            emuBrowserTab.Tag = conf_EmuBrowser1;
            romBrowserTab.Tag = conf_DBBrowser1;
            importerTab.Tag = conf_DBImporter1;
            groupsTab.Tag = conf_Groups1;
            backupTab.Tag = conf_DBBackup1;
            optionsTab.Tag = conf_Options_New1;

            mainTabControl.SelectedIndexChanged += new EventHandler(mainTabControl_SelectedIndexChanged);

            selectedPanel = mainTabControl.SelectedTab.Tag as ContentPanel;
        }

        void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedPanel != null)
                selectedPanel.SavePanel(); //save any changes in previous tab

            selectedPanel = mainTabControl.SelectedTab.Tag as ContentPanel;
            if (selectedPanel != null)
                selectedPanel.UpdatePanel(); //refresh new tab
        }

        private void Conf_Main_Load(object sender, EventArgs e)
        {
            if (selectedPanel != null)
                selectedPanel.UpdatePanel();
        }

        private void Conf_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage tab in mainTabControl.TabPages)
            {
                ContentPanel panel = tab.Tag as ContentPanel;
                if (panel != null)
                {
                    panel.ClosePanel();
                    panel.Dispose();
                }
            }
        }
    }
}
