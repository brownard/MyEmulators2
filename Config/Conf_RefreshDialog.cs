using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    public partial class Conf_RefreshDialog : Form
    {
        BackgroundScraper backgroundScraper = new BackgroundScraper(null);

        public Conf_RefreshDialog()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Conf_RefreshDialog_FormClosing);

            backgroundScraper.StartRefresh(new BackgroundScraper.refreshProgress(onProgress));
        }

        public Conf_RefreshDialog(bool shouldClean)
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Conf_RefreshDialog_FormClosing);

            if (shouldClean)
                backgroundScraper.StartClean(new BackgroundScraper.refreshProgress(onProgress));
            else
                backgroundScraper.StartRefresh(new BackgroundScraper.refreshProgress(onProgress));
        }

        void Conf_RefreshDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundScraper.Stop();
        }

        void onProgress(string label, int progress, bool complete)
        {
            if (InvokeRequired)
            {
                if (complete)
                    BeginInvoke(new BackgroundScraper.refreshProgress(onProgress), new object[] { label, progress, complete });
                else
                    Invoke(new BackgroundScraper.refreshProgress(onProgress), new object[] { label, progress, complete });
                return;
            }
            progressLabel1.Text = label;
            progressBar1.Value = progress;
            if (complete)
                button1.Enabled = true;
        }

        void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Conf_RefreshDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
