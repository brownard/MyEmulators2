using MyEmulators2.Import;
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
    partial class Conf_Options_New : ContentPanel
    {
        bool updateGeneral = false;
        bool updateLayout = false;
        bool updateStopEmu = false;
        bool updateGoodmerge = false;
        bool updateDatabase = false;
        bool updateAdvanced = false;
        bool updateCommunityServer = false;

        public Conf_Options_New()
        {
            InitializeComponent();            
        }

        void Conf_Options_New_Load(object sender, EventArgs e)
        {
            getOptions();
            keyMapLabel.PreviewKeyDown += new PreviewKeyDownEventHandler(mapKeyDown);
            addEventHandlers();
            if (scraperListBox.Items.Count > 0)
                scraperListBox.SelectedIndex = 0;
        }

        #region Event Handlers

        void addEventHandlers()
        {
            //General options
            shownnameBox.TextChanged += new EventHandler(generalOptionsChanged);
            languageBox.TextChanged += new EventHandler(generalOptionsChanged);
            startupComboBox.TextChanged += new EventHandler(generalOptionsChanged);
            clickToDetailsCheckBox.CheckedChanged += new EventHandler(generalOptionsChanged);
            showSortPropertyBox.CheckedChanged += new EventHandler(generalOptionsChanged);
            autoconfemuBox.CheckedChanged += new EventHandler(generalOptionsChanged);
            stopMediaCheckBox.CheckedChanged += new EventHandler(generalOptionsChanged);

            //Layout options
            viewBox.TextChanged += new EventHandler(layoutOptionsChanged);
            pcViewBox.TextChanged += new EventHandler(layoutOptionsChanged);
            favouritesViewBox.TextChanged += new EventHandler(layoutOptionsChanged);
            showFanArtCheckBox.CheckedChanged += new EventHandler(layoutOptionsChanged);
            fanartDelayBox.ValueChanged += new EventHandler(layoutOptionsChanged);
            showGameArtCheckBox.CheckedChanged += new EventHandler(layoutOptionsChanged);
            gameArtDelayBox.ValueChanged += new EventHandler(layoutOptionsChanged);

            showPrevVideoCheckBox.CheckedChanged += new EventHandler(layoutOptionsChanged);
            prevVidDelayBox.ValueChanged += new EventHandler(layoutOptionsChanged);
            loopPrevVidCheckBox.CheckedChanged += new EventHandler(layoutOptionsChanged);
            useEmuPrevVidCheckBox.CheckedChanged += new EventHandler(layoutOptionsChanged);

            //Stop emu options
            stopEmuCheckBox.CheckedChanged += new EventHandler(stopEmulationOptionsChanged);
            keyMapLabel.TextChanged += new EventHandler(stopEmulationOptionsChanged);

            //Goodmerge options
            goodFiltersTextBox.TextChanged += new EventHandler(goodmergeOptionsChanged);
            showGMDialogCheckBox.CheckStateChanged += new EventHandler(goodmergeOptionsChanged);

            // Community server options
            //submitGameInfoToServer.CheckedChanged += new EventHandler(communityServerOptionsChanged);
            //retrieveGameInfoToServer.CheckedChanged += new EventHandler(communityServerOptionsChanged);
            //communityServerAddress.TextChanged += new EventHandler(communityServerOptionsChanged);
            //communityServerErrorWaitTime.ValueChanged += new EventHandler(communityServerOptionsChanged);

            //Database options
            autoRefreshGames.CheckedChanged += new EventHandler(databaseOptionsChanged);
            autoImportCheckBox.CheckedChanged += new EventHandler(databaseOptionsChanged);
            exactMatchCheckBox.CheckedChanged += new EventHandler(databaseOptionsChanged);
            approveTopCheckBox.CheckedChanged += new EventHandler(databaseOptionsChanged);
            resizeThumbCheckBox.CheckedChanged += new EventHandler(databaseOptionsChanged);
            thoroughThumbCheckBox.CheckedChanged += new EventHandler(databaseOptionsChanged);

            //Advanced options
            thumbDirTextBox.TextChanged += new EventHandler(advancedOptionsChanged);
            threadCountUpDown.ValueChanged += new EventHandler(advancedOptionsChanged);
            threadCountUpDown.ValueChanged += new EventHandler(threadCountUpDown_ValueChanged);
            hashThreadUpDown.ValueChanged += new EventHandler(advancedOptionsChanged);

            scraperListBox.ItemCheck += new ItemCheckEventHandler(advancedOptionsChanged);
            scraperListBox.SelectedIndexChanged += new EventHandler(scraperListBox_SelectedIndexChanged);
            coversScraperComboBox.SelectedIndexChanged += new EventHandler(advancedOptionsChanged);
            screensScraperComboBox.SelectedIndexChanged += new EventHandler(advancedOptionsChanged);
            fanartScraperComboBox.SelectedIndexChanged += new EventHandler(advancedOptionsChanged);
        }

        void scraperListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            scraperUpButton.Enabled = true;
            scraperDownButton.Enabled = true;
            int index = scraperListBox.SelectedIndex;
            if (index == 0)
                scraperUpButton.Enabled = false;
            if (index == scraperListBox.Items.Count - 1)
                scraperDownButton.Enabled = false;
        }

        void threadCountUpDown_ValueChanged(object sender, EventArgs e)
        {
            hashThreadUpDown.Maximum = threadCountUpDown.Value;
        }

        void communityServerOptionsChanged(object sender, EventArgs e)
        {
            updateCommunityServer = true;
        }

        void advancedOptionsChanged(object sender, EventArgs e)
        {
            updateAdvanced = true;
        }

        void databaseOptionsChanged(object sender, EventArgs e)
        {
            updateDatabase = true;
        }

        void layoutOptionsChanged(object sender, EventArgs e)
        {
            updateLayout = true;
        }

        void generalOptionsChanged(object sender, EventArgs e)
        {
            updateGeneral = true;
        }

        void stopEmulationOptionsChanged(object sender, EventArgs e)
        {
            updateStopEmu = true;
        }

        void goodmergeOptionsChanged(object sender, EventArgs e)
        {
            updateGoodmerge = true;
        }

        #endregion

        void getOptions()
        {
            Options options = Options.Instance;

            shownnameBox.Text = options.GetStringOption("shownname");
            foreach (string language in Translator.Instance.GetLanguages())
                languageBox.Items.Add(language);
            languageBox.Text = options.GetStringOption("language");

            int selectedValue;
            startupComboBox.DataSource = Options.Instance.GetStartupOptions(out selectedValue);//.ToArray();
            startupComboBox.SelectedValue = selectedValue;

            clickToDetailsCheckBox.Checked = Options.Instance.GetBoolOption("clicktodetails");
            showSortPropertyBox.Checked = options.GetBoolOption("showsortvalue");
            autoconfemuBox.Checked = options.GetBoolOption("autoconfemu");
            stopMediaCheckBox.Checked = options.GetBoolOption("stopmediaplayback");

            viewBox.SelectedIndex = options.GetIntOption("viewemus");
            pcViewBox.SelectedIndex = options.GetIntOption("viewpcgames");
            favouritesViewBox.SelectedIndex = options.GetIntOption("viewfavourites");
            showFanArtCheckBox.Checked = options.GetBoolOption("showfanart");
            fanartDelayBox.Value = options.GetIntOption("fanartdelay");
            showGameArtCheckBox.Checked = options.GetBoolOption("showgameart");
            gameArtDelayBox.Value = options.GetIntOption("gameartdelay");

            showPrevVideoCheckBox.Checked = options.GetBoolOption("showvideopreview");
            prevVidDelayBox.Value = options.GetIntOption("videopreviewdelay");
            loopPrevVidCheckBox.Checked = options.GetBoolOption("loopvideopreview");
            useEmuPrevVidCheckBox.Checked = options.GetBoolOption("defaultvideopreview");
                
            stopEmuCheckBox.Checked = options.GetBoolOption("domap");
            keyData = options.GetIntOption("mappedkeydata");
            if (keyData > 0)
                keyMapLabel.Text = Options.GetKeyDisplayString(keyData);

            goodFiltersTextBox.Text = options.GetStringOption("goodmergefilters");

            if (options.GetBoolOption("showgmdialogonce"))
                showGMDialogCheckBox.CheckState = CheckState.Indeterminate;
            else
                showGMDialogCheckBox.CheckState = options.GetBoolOption("showgmdialog") ? CheckState.Checked : CheckState.Unchecked;

            //submitGameInfoToServer.Checked = options.GetBoolOption("submitGameDetails");
            //retrieveGameInfoToServer.Checked = options.GetBoolOption("retrieveGameDetials");
            //communityServerAddress.Text = options.GetStringOption("communityServerAddress");
            //communityServerErrorWaitTime.Value = options.GetIntOption("communityServerConnectionRetryTime");

            autoRefreshGames.Checked = options.GetBoolOption("autorefreshgames");
            autoImportCheckBox.Checked = options.GetBoolOption("autoimportgames");
            exactMatchCheckBox.Checked = options.GetBoolOption("importexact");
            approveTopCheckBox.Checked = options.GetBoolOption("importtop");
            resizeThumbCheckBox.Checked = options.GetBoolOption("resizethumbs");
            thoroughThumbCheckBox.Checked = options.GetBoolOption("thoroughthumbsearch");

            thumbDirTextBox.Text = options.GetStringOption("thumblocation");

            //validate threadcount
            int threadCount = options.GetIntOption("importthreadcount");
            if (threadCount > 10)
            {
                threadCount = 10;
                updateAdvanced = true;
            }
            else if (threadCount < 1)
            {
                threadCount = 1;
                updateAdvanced = true;
            }
            threadCountUpDown.Value = threadCount;

            hashThreadUpDown.Maximum = threadCount;
            int hashThreads = options.GetIntOption("hashthreadcount");
            if (hashThreads > threadCount)
            {
                hashThreads = threadCount;
                updateAdvanced = true;
            }
            else if (hashThreads < 1)
            {
                hashThreads = 1;
                updateAdvanced = true;
            }
            hashThreadUpDown.Value = hashThreads;

            //scraper checkbox
            List<string> ignoredScripts = new List<string>();
            string[] optStr = Options.Instance.GetStringOption("ignoredscrapers").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            ignoredScripts.AddRange(optStr);

            coversScraperComboBox.Items.Add("Use search result");
            coversScraperComboBox.SelectedIndex = 0;
            screensScraperComboBox.Items.Add("Use search result");
            screensScraperComboBox.SelectedIndex = 0;
            fanartScraperComboBox.Items.Add("Use search result");
            fanartScraperComboBox.SelectedIndex = 0;

            foreach (Scraper script in ScraperProvider.GetScrapers(true))
            {
                scraperListBox.Items.Add(script, !ignoredScripts.Contains(script.IdString));
                coversScraperComboBox.Items.Add(script);
                screensScraperComboBox.Items.Add(script);
                int index = fanartScraperComboBox.Items.Add(script);
                if (script.IdString == options.GetStringOption("coversscraperid"))
                    coversScraperComboBox.SelectedIndex = index;
                if (script.IdString == options.GetStringOption("screensscraperid"))
                    screensScraperComboBox.SelectedIndex = index;
                if (script.IdString == options.GetStringOption("fanartscraperid"))
                    fanartScraperComboBox.SelectedIndex = index;
            }
        }

        public override void SavePanel()
        {
            Options options = Options.Instance;

            if (updateGeneral)
            {
                options.UpdateOption("shownname", shownnameBox.Text);
                options.UpdateOption("language", languageBox.Text);
                options.UpdateOption("startupstate", startupComboBox.SelectedValue);
                options.UpdateOption("clicktodetails", clickToDetailsCheckBox.Checked);
                options.UpdateOption("showsortvalue", showSortPropertyBox.Checked);
                options.UpdateOption("autoconfemu", autoconfemuBox.Checked);
                options.UpdateOption("stopmediaplayback", stopMediaCheckBox.Checked);
                updateGeneral = false;
            }

            if (updateLayout)
            {
                options.UpdateOption("viewemus", viewBox.SelectedIndex);
                options.UpdateOption("viewpcgames", pcViewBox.SelectedIndex);
                options.UpdateOption("viewfavourites", favouritesViewBox.SelectedIndex);
                options.UpdateOption("fanartdelay", Convert.ToInt32(fanartDelayBox.Value));
                options.UpdateOption("showfanart", showFanArtCheckBox.Checked);
                options.UpdateOption("gameartdelay", Convert.ToInt32(gameArtDelayBox.Value));
                options.UpdateOption("showgameart", showGameArtCheckBox.Checked);

                options.UpdateOption("showvideopreview", showPrevVideoCheckBox.Checked);
                options.UpdateOption("videopreviewdelay", Convert.ToInt32(prevVidDelayBox.Value));
                options.UpdateOption("loopvideopreview", loopPrevVidCheckBox.Checked);
                options.UpdateOption("defaultvideopreview", useEmuPrevVidCheckBox.Checked);

                updateLayout = false;
            }

            if (updateStopEmu)
            {
                if (keyData > 0)
                    options.UpdateOption("mappedkeydata", keyData);
                options.UpdateOption("domap", stopEmuCheckBox.Checked);
                updateStopEmu = false;
            }

            if (updateGoodmerge)
            {
                options.UpdateOption("goodmergefilters", goodFiltersTextBox.Text);
                if (showGMDialogCheckBox.CheckState == CheckState.Indeterminate)
                {
                    options.UpdateOption("showgmdialogonce", true);
                    options.UpdateOption("showgmdialog", false);
                }
                else
                {
                    options.UpdateOption("showgmdialogonce", false);
                    options.UpdateOption("showgmdialog", showGMDialogCheckBox.Checked);
                }

                updateGoodmerge = false;
            }

            if (updateDatabase)
            {
                options.UpdateOption("autorefreshgames", autoRefreshGames.Checked);
                options.UpdateOption("autoimportgames", autoImportCheckBox.Checked);
                options.UpdateOption("importexact", exactMatchCheckBox.Checked);
                options.UpdateOption("importtop", approveTopCheckBox.Checked);
                options.UpdateOption("resizethumbs", resizeThumbCheckBox.Checked);
                options.UpdateOption("thoroughthumbsearch", thoroughThumbCheckBox.Checked);
                updateDatabase = false;
            }

            if (updateAdvanced)
            {
                options.UpdateOption("thumblocation", thumbDirTextBox.Text);
                options.UpdateOption("importthreadcount", (int)threadCountUpDown.Value);
                options.UpdateOption("hashthreadcount", (int)hashThreadUpDown.Value);

                string ignoredScripts = "";
                string scriptPriorities = "";
                for (int x = 0; x < scraperListBox.Items.Count; x++)
                {
                    Scraper scraper = (Scraper)scraperListBox.Items[x];
                    string optStr = scraper.IdString + ";";
                    scriptPriorities += optStr;
                    if (!scraperListBox.GetItemChecked(x))
                        ignoredScripts += optStr;
                }
                options.UpdateOption("ignoredscrapers", ignoredScripts);
                options.UpdateOption("scraperpriorities", scriptPriorities);

                if (coversScraperComboBox.SelectedIndex < 1)
                    options.UpdateOption("coversscraperid", "");
                else
                    options.UpdateOption("coversscraperid", ((Scraper)coversScraperComboBox.SelectedItem).IdString);

                if (screensScraperComboBox.SelectedIndex < 1)
                    options.UpdateOption("screensscraperid", "");
                else
                    options.UpdateOption("screensscraperid", ((Scraper)screensScraperComboBox.SelectedItem).IdString);

                if (fanartScraperComboBox.SelectedIndex < 1)
                    options.UpdateOption("fanartscraperid", "");
                else
                    options.UpdateOption("fanartscraperid", ((Scraper)fanartScraperComboBox.SelectedItem).IdString);

                updateAdvanced = false;
            }

            if (updateCommunityServer)
            {
                //options.UpdateOption("submitGameDetails", submitGameInfoToServer.Checked);
                //options.UpdateOption("retrieveGameDetials", retrieveGameInfoToServer.Checked);
                //options.UpdateOption("communityServerAddress", communityServerAddress.Text);
                //options.UpdateOption("communityServerConnectionRetryTime", communityServerErrorWaitTime.Value);
            }

            base.SavePanel();
        }

        public override void ClosePanel()
        {
            SavePanel();
            base.ClosePanel();
        }

        private void thumbDirButton_Click(object sender, EventArgs e)
        {
            string initialDir = thumbDirTextBox.Text;
            if(!System.IO.Directory.Exists(initialDir))
                initialDir = MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Thumbs);

            using (FolderBrowserDialog dlg = Emulators2Settings.OpenFolderDialog("Select thumb directory", initialDir))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    thumbDirTextBox.Text = dlg.SelectedPath;
            }
        }

        #region Key Mapping

        bool mapping = false;
        Timer timer = null;

        int keyData = -1;

        void mapKeyDown(object o, PreviewKeyDownEventArgs e)
        {
            if (mapping && e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.Menu)
            {
                keyData = (int)e.KeyData;
                keyMapLabel.Text = Options.GetKeyDisplayString(keyData); //Enum.GetName(typeof(Keys), e.KeyCode);
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

        #endregion

        void ignoredFilesButton_Click(object sender, EventArgs e)
        {
            using (Conf_IgnoredFiles dlg = new Conf_IgnoredFiles())
                dlg.ShowDialog();
        }

        void moveScript(int direction)
        {
            object item = scraperListBox.SelectedItem;
            if (item == null)
                return;
            int currIndex = scraperListBox.SelectedIndex;
            int newIndex = currIndex + direction;
            if (newIndex < 0)
                newIndex = 0;
            else if (newIndex > scraperListBox.Items.Count - 1)
                newIndex = scraperListBox.Items.Count - 1;

            if (newIndex == currIndex)
                return;

            bool isChecked = scraperListBox.GetItemChecked(currIndex);
            scraperListBox.Items.RemoveAt(scraperListBox.SelectedIndex);
            scraperListBox.Items.Insert(newIndex, item);
            scraperListBox.SelectedItem = item;
            scraperListBox.SetItemChecked(newIndex, isChecked);
            updateAdvanced = true;
        }

        private void scraperUpButton_Click(object sender, EventArgs e)
        {
            moveScript(-1);
        }

        private void scraperDownButton_Click(object sender, EventArgs e)
        {
            moveScript(1);
        }
    }
}
