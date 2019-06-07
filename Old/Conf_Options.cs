using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MediaPortal.Configuration;

namespace myEmulators
{
    internal partial class Conf_Options : myEmulators.ContentPanel
    {
        public Conf_Options()
        {
            InitializeComponent();
            keyMapLabel.PreviewKeyDown += new PreviewKeyDownEventHandler(myKeyDown);
            keyMapLabel.Text = Options.Instance.GetStringOption("mappedkey");
            stopEmuCheckBox.Checked = Options.Instance.GetBoolOption("domap");
            txt_7zdll.Text = Options.Instance.GetStringOption("7zdllpath");
            shownnameBox.Text = Options.Instance.GetStringOption("shownname");
            autoconfemuBox.Checked = Options.Instance.GetBoolOption("autoconfemu");
            languageBox.Items.AddRange(Translator.getLanguages());
            languageBox.Text = Options.Instance.GetStringOption("language");
            favouriteslaunchBox.Checked = Options.Instance.GetBoolOption("startwithfavourites");

            viewBox.Items.Add("Coverflow");
            pcViewBox.Items.Add("Coverflow");
            favouritesViewBox.Items.Add("Coverflow");

            viewBox.SelectedIndex = Options.Instance.GetIntOption("viewemus");
            pcViewBox.SelectedIndex = Options.Instance.GetIntOption("viewpcgames");
            favouritesViewBox.SelectedIndex = Options.Instance.GetIntOption("viewfavourites");

            hidelabeldecorationsBox.Checked = Options.Instance.GetBoolOption("hidelabeldecorations");
            onlyShowPCGamesBox.Checked = Options.Instance.GetBoolOption("onlyshowpcgames");

            showFanArtCheckBox.Checked = Options.Instance.GetBoolOption("showfanart");
            fanartDelayBox.Value = Options.Instance.GetIntOption("fanartdelay");

            showGameArtCheckBox.Checked = Options.Instance.GetBoolOption("showgameart");
            gameArtDelayBox.Value = Options.Instance.GetIntOption("gameartdelay");

            //advanced options
            thumbDirTextBox.Text = Options.Instance.GetStringOption("thumblocation");
            goodFiltersTextBox.Text = Options.Instance.GetStringOption("goodmergefilters");

            //validate threadcount
            int threadCount = Options.Instance.GetIntOption("importthreadcount");
            if (threadCount > 10)
                threadCount = 10;
            else if (threadCount < 1)
                threadCount = 1;
            threadCountUpDown.Value = threadCount;

            stopMediaCheckBox.Checked = Options.Instance.GetBoolOption("stopmediaplayback");

            //Add eventhandlers after content is loaded, so it won't report
            //changes made when loading from options file at startup
            keyMapLabel.TextChanged += new EventHandler(changesMade);
            stopEmuCheckBox.CheckedChanged += new EventHandler(changesMade);
            txt_7zdll.TextChanged += new EventHandler(changesMade);
            shownnameBox.TextChanged += new EventHandler(changesMade);
            autoconfemuBox.CheckedChanged += new EventHandler(changesMade);
            languageBox.SelectedIndexChanged += new EventHandler(changesMade);
            viewBox.SelectedIndexChanged += new EventHandler(changesMade);
            favouriteslaunchBox.CheckedChanged += new EventHandler(changesMade);
            favouritesViewBox.SelectedIndexChanged += new EventHandler(changesMade);
            pcViewBox.SelectedIndexChanged += new EventHandler(changesMade);
            hidelabeldecorationsBox.CheckedChanged += new EventHandler(changesMade);
            onlyShowPCGamesBox.CheckedChanged += new EventHandler(changesMade);

            fanartDelayBox.ValueChanged += new EventHandler(changesMade);
            showFanArtCheckBox.CheckedChanged += new EventHandler(changesMade);

            gameArtDelayBox.ValueChanged += new EventHandler(changesMade);
            showGameArtCheckBox.CheckedChanged += new EventHandler(changesMade);

            thumbDirTextBox.TextChanged += new EventHandler(changesMade);
            goodFiltersTextBox.TextChanged += new EventHandler(changesMade);
            threadCountUpDown.ValueChanged += new EventHandler(changesMade);
            stopMediaCheckBox.CheckedChanged += new EventHandler(changesMade);

        }


        void changesMade(object sender, EventArgs e)
        {
            OnChange(this, e);
        }

        public override void save()
        {
            Options.Instance.UpdateOption("mappedkey", keyMapLabel.Text);
            Options.Instance.UpdateOption("domap", stopEmuCheckBox.Checked);
            Options.Instance.UpdateOption("7zdllpath", txt_7zdll.Text);
            Options.Instance.UpdateOption("shownname", shownnameBox.Text);
            Options.Instance.UpdateOption("autoconfemu", autoconfemuBox.Checked);
            Options.Instance.UpdateOption("language", languageBox.Text);
            Options.Instance.UpdateOption("viewemus", viewBox.SelectedIndex);
            Options.Instance.UpdateOption("startwithfavourites", favouriteslaunchBox.Checked);
            Options.Instance.UpdateOption("viewfavourites", favouritesViewBox.SelectedIndex);
            Options.Instance.UpdateOption("viewpcgames", pcViewBox.SelectedIndex);
            Options.Instance.UpdateOption("hidelabeldecorations", hidelabeldecorationsBox.Checked);
            Options.Instance.UpdateOption("onlyshowpcgames", onlyShowPCGamesBox.Checked);

            Options.Instance.UpdateOption("fanartdelay", Convert.ToInt32(fanartDelayBox.Value));
            Options.Instance.UpdateOption("showfanart", showFanArtCheckBox.Checked);

            Options.Instance.UpdateOption("gameartdelay", Convert.ToInt32(gameArtDelayBox.Value));
            Options.Instance.UpdateOption("showgameart", showGameArtCheckBox.Checked);

            Options.Instance.UpdateOption("thumblocation", thumbDirTextBox.Text);
            Options.Instance.UpdateOption("goodmergefilters", goodFiltersTextBox.Text);
            Options.Instance.UpdateOption("importthreadcount", (int)threadCountUpDown.Value);
            Options.Instance.UpdateOption("stopmediaplayback", stopMediaCheckBox.Checked);

            base.save();
        }

        private void btnNew7z_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Title = "Select 7z.dll image";
                diag.Filter = "7z.dll (7z.dll) | 7z.dll";
                diag.RestoreDirectory = true;
                diag.ValidateNames = true;
                diag.CheckFileExists = true;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    txt_7zdll.Text = diag.FileName;
                }
            }
        }

        bool mapping = false;
        Timer timer = null;

        void myKeyDown(object o, PreviewKeyDownEventArgs e)
        {
            if (mapping)
            {
                keyMapLabel.Text = Enum.GetName(typeof(Keys), e.KeyCode);
                stopMapping();
            }
        }

        private void mapKeyButton_Click(object sender, EventArgs e)
        {
            keyMapLabel.Focus();
            mapping = true;
            mapKeyButton.Text = "Press Key";
            mapKeyButton.Enabled = false;
            timer = new Timer() { Interval = 5000 };
            timer.Tick += new EventHandler(delegate(object o, EventArgs ev)
                {
                    stopMapping();
                }
                );
            timer.Start();
        }

        private void stopMapping()
        {
            mapping = false;
            mapKeyButton.Text = "Map";
            mapKeyButton.Enabled = true;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        private void thumbDirButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select thumb directory";
                dlg.RootFolder = Environment.SpecialFolder.Desktop;

                if (Directory.Exists(thumbDirTextBox.Text))
                    dlg.SelectedPath = thumbDirTextBox.Text;
                else
                    dlg.SelectedPath = Config.GetFolder(Config.Dir.Thumbs);

                if (dlg.ShowDialog() == DialogResult.OK)
                    thumbDirTextBox.Text = dlg.SelectedPath;
            }
        }
    }
}

