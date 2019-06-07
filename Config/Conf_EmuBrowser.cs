using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MyEmulators2.Import;

namespace MyEmulators2
{
    internal partial class Conf_EmuBrowser : ContentPanel
    {
        public Conf_EmuBrowser()
        {
            InitializeComponent();
            platformComboBox.DisplayMember = "Text";
            platformComboBox.DataSource = Dropdowns.GetSystems();
            addEventHandlers();
            setupToolTip();
        }

        List<ListViewItem> dbEmus = null;
        ListViewItem selectedListItem = null;
        Emulator selectedEmulator = null;
        bool saveSelectedEmulator = false;
        bool saveThumbs = false;
        EmulatorProfile selectedProfile = null;
        bool saveProfile = false;
        ThumbGroup emuThumbs = null;
        bool allowChangedEvents = true;

        bool updateEmuPositions = false;
        bool thumbsLoaded = false;

        //add changed event handlers to controls
        //to determine whether to update selected Emulator 
        void addEventHandlers()
        {
            emulatorListView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(emulatorListView_ItemSelectionChanged);
            profileComboBox.SelectedIndexChanged += new EventHandler(profileComboBox_SelectedIndexChanged);

            txt_Title.TextChanged += new EventHandler(onItemChanged);
            platformComboBox.SelectedIndexChanged += new EventHandler(onItemChanged);
            romDirTextBox.TextChanged += new EventHandler(onItemChanged);
            filterTextBox.TextChanged += new EventHandler(onItemChanged);
            isArcadeCheckBox.CheckedChanged += new EventHandler(onItemChanged);
            txt_company.TextChanged += new EventHandler(onItemChanged);
            txt_yearmade.TextChanged += new EventHandler(onItemChanged);
            txt_description.TextChanged += new EventHandler(onItemChanged);
            gradeUpDown.ValueChanged += new EventHandler(onItemChanged);
            thumbAspectComboBox.TextChanged += new EventHandler(onItemChanged);

            txt_Manual.TextChanged += new EventHandler(onItemChanged);

            videoTextBox.TextChanged += new EventHandler(onItemChanged);

            pnlLogo.BackgroundImageChanged += new EventHandler(onThumbChanged);
            pnlFanart.BackgroundImageChanged += new EventHandler(onThumbChanged);

            emuPathTextBox.TextChanged += new EventHandler(onProfileChanged);
            emuPathTextBox.TextChanged += new EventHandler(emuPathTextBox_TextChanged);
            workingDirTextBox.TextChanged += new EventHandler(onProfileChanged);
            argumentsTextBox.TextChanged += new EventHandler(onProfileChanged);
            useQuotesCheckBox.CheckedChanged += new EventHandler(onProfileChanged);

            suspendMPCheckBox.CheckedChanged += new EventHandler(suspendMPCheckBox_CheckedChanged);
            delayResumeCheckBox.CheckedChanged += new EventHandler(onProfileChanged);
            resumeDelayUpDown.ValueChanged += new EventHandler(onProfileChanged);

            goodComboBox.TextChanged += new EventHandler(onProfileChanged);
            enableGoodCheckBox.CheckedChanged += new EventHandler(onProfileChanged);

            mountImagesCheckBox.CheckedChanged += new EventHandler(onProfileChanged);
            escExitCheckBox.CheckedChanged += new EventHandler(onProfileChanged);
            checkControllerCheckBox.CheckedChanged += new EventHandler(onProfileChanged);
            stopEmulationCheckBox.CheckStateChanged += new EventHandler(onProfileChanged);

            preCommandText.TextChanged += new EventHandler(onProfileChanged);
            preCommandWaitCheck.CheckedChanged += new EventHandler(onProfileChanged);
            preCommandWindowCheck.CheckedChanged += new EventHandler(onProfileChanged);
            postCommandText.TextChanged += new EventHandler(onProfileChanged);
            postCommandWaitCheck.CheckedChanged += new EventHandler(onProfileChanged);
            postCommandWindowCheck.CheckedChanged += new EventHandler(onProfileChanged);

            mainTabControl.SelectedIndexChanged += new EventHandler(mainTabControl_SelectedIndexChanged);
        }

        void suspendMPCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = suspendMPCheckBox.Checked;
            delayResumeCheckBox.Enabled = enabled;
            resumeDelayUpDown.Enabled = enabled;
            onProfileChanged(sender, e);
        }

        void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!thumbsLoaded && mainTabControl.SelectedTab == thumbsTabPage)
            {
                setGameArt(emuThumbs);
                thumbsLoaded = true;
            }
        }

        //Fired when a new profile is seleced from the profile dropdown.
        //Save any changes to current profile and update panel with new
        //profile details.
        void profileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProfile();

            selectedProfile = profileComboBox.SelectedItem as EmulatorProfile;
            if (selectedProfile == null)
                return;

            allowChangedEvents = false; //don't fire changed event when we are updating

            emuPathTextBox.Text = selectedProfile.EmulatorPath;
            workingDirTextBox.Text = selectedProfile.WorkingDirectory;
            argumentsTextBox.Text = selectedProfile.Arguments;
            useQuotesCheckBox.Checked = selectedProfile.UseQuotes != false;
            //suspend
            suspendMPCheckBox.Checked = selectedProfile.SuspendMP == true;
            delayResumeCheckBox.Checked = selectedProfile.DelayResume;
            resumeDelayUpDown.Value = selectedProfile.ResumeDelay;
            bool enabled = suspendMPCheckBox.Checked;
            delayResumeCheckBox.Enabled = enabled;
            resumeDelayUpDown.Enabled = enabled;

            enableGoodCheckBox.Checked = selectedProfile.EnableGoodmerge;
            goodComboBox.Text = selectedProfile.GoodmergeTags;

            mountImagesCheckBox.Checked = selectedProfile.MountImages;
            escExitCheckBox.Checked = selectedProfile.EscapeToExit;
            checkControllerCheckBox.Checked = selectedProfile.CheckController;
            stopEmulationCheckBox.CheckState = selectedProfile.StopEmulationOnKey.HasValue ? selectedProfile.StopEmulationOnKey.Value ? CheckState.Checked : CheckState.Unchecked : CheckState.Indeterminate;
            
            preCommandText.Text = selectedProfile.PreCommand;
            preCommandWaitCheck.Checked = selectedProfile.PreCommandWaitForExit;
            preCommandWindowCheck.Checked = selectedProfile.PreCommandShowWindow;
            postCommandText.Text = selectedProfile.PostCommand;
            postCommandWaitCheck.Checked = selectedProfile.PostCommandWaitForExit;
            postCommandWindowCheck.Checked = selectedProfile.PostCommandShowWindow;

            allowChangedEvents = true;

            updateButtons();
        }

        private void updateButtons()
        {
            //Default profile selected, don't allow rename or delete.
            if (selectedProfile == null || selectedProfile.IsDefault)
            {
                delProfileButton.Enabled = false;
                renameProfileButton.Enabled = false;
            }
            else
            {
                delProfileButton.Enabled = true;
                renameProfileButton.Enabled = true;
            }

            upButton.Enabled = true;
            downButton.Enabled = true;

            if (selectedListItem != null)
            {
                if (selectedListItem.Index == 0)
                {
                    upButton.Enabled = false; //top of list, don't allow up
                }
                if (selectedListItem.Index == emulatorListView.Items.Count - 1)
                {
                    downButton.Enabled = false; //bottom of list, don't allow down
                }
            }
        }

        private void updateProfile()
        {
            if (!saveProfile || selectedProfile == null)
                return; //no changes have been made or no profile is selected

            selectedProfile.EmulatorPath = emuPathTextBox.Text;
            selectedProfile.WorkingDirectory = workingDirTextBox.Text;
            selectedProfile.Arguments = argumentsTextBox.Text;
            selectedProfile.UseQuotes = useQuotesCheckBox.Checked;
            selectedProfile.SuspendMP = suspendMPCheckBox.Checked;
            selectedProfile.DelayResume = delayResumeCheckBox.Checked;
            selectedProfile.ResumeDelay = (int)resumeDelayUpDown.Value;

            selectedProfile.EnableGoodmerge = enableGoodCheckBox.Checked;
            selectedProfile.GoodmergeTags = goodComboBox.Text.Trim();
            selectedProfile.MountImages = mountImagesCheckBox.Checked;
            selectedProfile.EscapeToExit = escExitCheckBox.Checked;
            selectedProfile.CheckController = checkControllerCheckBox.Checked;
            selectedProfile.StopEmulationOnKey = boolFromCheckState(stopEmulationCheckBox.CheckState);
            
            selectedProfile.PreCommand = preCommandText.Text;
            selectedProfile.PreCommandWaitForExit = preCommandWaitCheck.Checked;
            selectedProfile.PreCommandShowWindow = preCommandWindowCheck.Checked;
            selectedProfile.PostCommand = postCommandText.Text;
            selectedProfile.PostCommandWaitForExit = postCommandWaitCheck.Checked;
            selectedProfile.PostCommandShowWindow = postCommandWindowCheck.Checked;

            selectedProfile.Save();
            saveProfile = false;
        }

        private bool? boolFromCheckState(CheckState c)
        {
            if (c == CheckState.Indeterminate)
                return null;
            return c == CheckState.Checked;
        }

        //Fired when the user changes emulator details
        void onItemChanged(object sender, EventArgs e)
        {
            if (allowChangedEvents)
                saveSelectedEmulator = true;
        }

        //Fired when the user changes emulator thumbs
        void onThumbChanged(object sender, EventArgs e)
        {
            if (allowChangedEvents)
            {
                saveSelectedEmulator = true;
                saveThumbs = true;
            }
        }

        //Fired when the user changes an emulator profile
        void onProfileChanged(object sender, EventArgs e)
        {
            if (allowChangedEvents)
                saveProfile = true;
        }

        #region Database ListView

        volatile bool listLoading = false;
        //populate the list view with all emulators
        void initListView()
        {
            if (listLoading)
                return;

            listLoading = true;
            dbEmus = new List<ListViewItem>();
            loadingEmusPanel.Visible = true; //display the loading panel
            emulatorListView.BeginUpdate();
            emulatorListView.Items.Clear();

            //load on seperate thread to ensure responsiveness
            Thread thread = new Thread(new ThreadStart(delegate()
            {
                List<Emulator> emus = DB.Instance.GetEmulators(true);
                foreach (Emulator emu in emus)
                {
                    ListViewItem item = new ListViewItem(emu.Title);
                    item.Tag = emu; //keep reference to emulator
                    try
                    {
                        //add the emulators to the list view on main thread
                        Invoke(new MethodInvoker(delegate
                        {
                            dbEmus.Add(item);
                            emulatorListView.Items.Add(item);
                        }
                        ));
                    }
                    catch (InvalidOperationException)
                    {
                        //form closed before complete
                        return;
                    }
                }
                try
                {
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        if (emulatorListView.Items.Count > 0)
                            emulatorListView.Items[0].Selected = true;
                        emulatorListView.EndUpdate();
                        loadingEmusPanel.Visible = false;
                        listLoading = false;
                    }
                        ));
                }
                catch { listLoading = false; } //form closed before complete
            }
            ));

            thread.Name = "Emulator browser populator";
            thread.Start();
        }

        //Fired when the user selectes an item in the listview
        void emulatorListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //This event is fired twice, once for the item losing selection
            //and once for the item being selected. Ensure we only process
            //one for performance.
            if (!e.IsSelected)
                return;

            updateEmulator();
            updateProfile();

            if (emulatorListView.SelectedItems.Count != 1)
            {
                //multiple items selected, shouldn't occur
                //with current setup but just in case.
                selectedListItem = null;
                selectedEmulator = null;
                selectedProfile = null;
                return;
            }

            selectedListItem = e.Item;
            //update panel with selected emu details
            setEmulatorToPanel(selectedListItem);
            //update panel enablings
            updatePanels();
        }

        //disables the profile panel when the PC Emulator is selected
        private void updatePanels()
        {
            if (selectedEmulator == null)
                return;

            if (selectedEmulator.UID == -1) //PC Emulator don't allow profile edit
            {
                profGroupBox.Enabled = false;
            }
            else
            {
                profGroupBox.Enabled = true;
            }
        }

        //Updates the panels with the selected Emulator's details.
        private void setEmulatorToPanel(ListViewItem listViewItem)
        {
            //reset status flags
            saveSelectedEmulator = false;
            saveThumbs = false;
            saveProfile = false;
            //get the selected Emulator
            Emulator dbEmu = listViewItem.Tag as Emulator;
            selectedEmulator = dbEmu;

            if (dbEmu == null)
                return;

            allowChangedEvents = false;

            int index = platformComboBox.FindStringExact(dbEmu.PlatformTitle);
            if (index < 0)
                index = 0;

            if (index < platformComboBox.Items.Count)
                platformComboBox.SelectedItem = platformComboBox.Items[index];
            
            txt_Title.Text = dbEmu.Title;
            romDirTextBox.Text = dbEmu.PathToRoms;
            filterTextBox.Text = dbEmu.Filter;
            isArcadeCheckBox.Checked = dbEmu.IsArcade;
            txt_company.Text = dbEmu.Company;
            txt_yearmade.Text = dbEmu.Year.ToString();
            txt_description.Text = dbEmu.Description;
            gradeUpDown.Value = dbEmu.Grade;

            EmuSettingsAutoFill.SetupAspectDropdown(thumbAspectComboBox, dbEmu.CaseAspect);

            videoTextBox.Text = dbEmu.VideoPreview;

            idLabel.Text = dbEmu.UID.ToString();

            if (emuThumbs != null)
                emuThumbs.Dispose();
            emuThumbs = new ThumbGroup(dbEmu);

            if (mainTabControl.SelectedTab == thumbsTabPage)
            {
                setGameArt(emuThumbs);
                thumbsLoaded = true;
            }
            else
            {
                setGameArt(null);
                thumbsLoaded = false;
            }

            txt_Manual.Text = emuThumbs.ManualPath;


            selectedProfile = null;
            profileComboBox.Items.Clear();
            foreach (EmulatorProfile profile in DB.Instance.GetProfiles(selectedEmulator))
            {
                profileComboBox.Items.Add(profile);
                if (profile.IsDefault)
                {
                    profileComboBox.SelectedItem = profile;
                    selectedProfile = profile;
                }
            }

            allowChangedEvents = true;
            profileComboBox_SelectedIndexChanged(profileComboBox, new EventArgs());
        }

        void clearForm()
        {
            allowChangedEvents = false;

            saveSelectedEmulator = false;
            saveProfile = false;
            saveThumbs = false;

            txt_Title.Text = "";
            romDirTextBox.Text = "";
            filterTextBox.Text = "";
            txt_company.Text = "";
            txt_yearmade.Text = "";
            txt_description.Text = "";
            gradeUpDown.Value = 0;

            thumbAspectComboBox.Text = "";

            txt_Manual.Text = "";
            idLabel.Text = "";

            enableGoodCheckBox.Checked = false;
            videoTextBox.Text = "";

            if (platformComboBox.Items.Count > 0)
                platformComboBox.SelectedIndex = 0;
            emuPathTextBox.Text = "";
            argumentsTextBox.Text = "";
            workingDirTextBox.Text = "";
            useQuotesCheckBox.Checked = true;
            suspendMPCheckBox.Checked = false;
            goodComboBox.Text = "";

            mountImagesCheckBox.Checked = false;
            escExitCheckBox.Checked = false;
            checkControllerCheckBox.Checked = false;

            if (emuThumbs != null)
            {
                emuThumbs.Dispose();
                emuThumbs = null;
            }
            pnlLogo.ThumbGroup = null;
            pnlFanart.ThumbGroup = null;

            allowChangedEvents = true;
        }

        private void updateEmulator()
        {
            if (!saveSelectedEmulator || selectedEmulator == null)
                return;

            selectedEmulator.Title = txt_Title.Text;
            if (selectedListItem != null)
            {
                selectedListItem.Text = selectedEmulator.Title;
            }
                        
            selectedEmulator.PlatformTitle = platformComboBox.Text;
            selectedEmulator.PathToRoms = romDirTextBox.Text;
            selectedEmulator.Filter = filterTextBox.Text;
            selectedEmulator.IsArcade = isArcadeCheckBox.Checked;
            selectedEmulator.Company = txt_company.Text;
            selectedEmulator.Description = txt_description.Text;
            selectedEmulator.Grade = (int)gradeUpDown.Value;

            int year;
            if (!int.TryParse(txt_yearmade.Text, out year))
                year = 0;
            selectedEmulator.Year = year;
            selectedEmulator.SetCaseAspect(thumbAspectComboBox.Text);

            selectedEmulator.VideoPreview = videoTextBox.Text;

            selectedEmulator.Save();

            if (emuThumbs != null)
            {
                emuThumbs.ManualPath = txt_Manual.Text;
                emuThumbs.SaveManual();
                if (saveThumbs)
                    emuThumbs.SaveAllThumbs();
            }

            saveSelectedEmulator = false;
        }

        void setGameArt(ThumbGroup thumbGroup)
        {
            bool lAllowEvents = allowChangedEvents;
            allowChangedEvents = false;
            pnlLogo.ThumbGroup = thumbGroup;
            pnlFanart.ThumbGroup = thumbGroup;
            allowChangedEvents = lAllowEvents;
        }

        #endregion

        #region Overrides

        public override void UpdatePanel()
        {
            initListView();
            base.UpdatePanel();
        }

        public override void SavePanel()
        {
            updateEmulator();
            updateProfile();
            updatePositions();
            clearForm();
            base.SavePanel();
        }

        public override void ClosePanel()
        {
            SavePanel();

            if (emuThumbs != null)
                emuThumbs.Dispose();

            if (thumbRetriever != null)
                thumbRetriever.close();

            base.ClosePanel();
        }

        void updatePositions()
        {
            if (!updateEmuPositions)
                return;

            List<Emulator> emus = new List<Emulator>();
            foreach (ListViewItem item in emulatorListView.Items)
            {
                Emulator emu = item.Tag as Emulator;
                if (emu.Position != item.Index)
                {
                    emu.Position = item.Index;
                    emus.Add(emu);
                }
            }
            var handler = new BackgroundTaskHandler<Emulator>() { Items = emus };
            handler.StatusDelegate = o => { return string.Format("Updating {0} position...", o.Title); };
            handler.ActionDelegate = o =>
                {
                    o.SavePosition();
                    return true;
                };

            using (Conf_ProgressDialog dlg = new Conf_ProgressDialog(handler))
                dlg.ShowDialog();
            updateEmuPositions = false;
        }

        #endregion

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            if (selectedEmulator == null)
                return;

            using (Conf_NewProfile profileDlg = new Conf_NewProfile(selectedEmulator, null))
            {
                if (profileDlg.ShowDialog() != DialogResult.OK || profileDlg.EmulatorProfile == null)
                    return;

                EmulatorProfile profile = profileDlg.EmulatorProfile;
                profile.Save();
                profileComboBox.Items.Add(profile);
                profileComboBox.SelectedItem = profile;
            }
        }

        private void delProfileButton_Click(object sender, EventArgs e)
        {
            if (selectedProfile.IsDefault)
            {
                Logger.LogDebug("Default profile cannot be deleted");
                return;
            }

            EmulatorProfile profile = selectedProfile;
            saveProfile = false;
            selectedProfile = null;
            profile.Delete();

            int index = profileComboBox.SelectedIndex;
            profileComboBox.Items.Remove(profile);

            if (index > 0)
                profileComboBox.SelectedIndex = index - 1;
            else
                profileComboBox.SelectedIndex = 0;
        }

        private void renameProfileButton_Click(object sender, EventArgs e)
        {
            updateProfile();

            if (selectedProfile == null)
                return;
            if (selectedProfile.IsDefault)
            {
                Logger.LogDebug("Default profile cannot be renamed");
                return;
            }

            using (Conf_NewProfile profileDlg = new Conf_NewProfile(selectedEmulator, selectedProfile))
            {
                if (profileDlg.ShowDialog() == DialogResult.OK)
                {
                    profileDlg.EmulatorProfile.Save();
                    int index = profileComboBox.SelectedIndex;
                    profileComboBox.Items.Remove(selectedProfile);
                    profileComboBox.Items.Insert(index, profileDlg.EmulatorProfile);
                    profileComboBox.SelectedIndex = index;
                }
            }
        }

        private void emuPathBrowseButton_Click(object sender, EventArgs e)
        {            
            string filter = "Executables (*.bat, *.exe) | *.bat;*.exe";

            string initialDirectory;
            int index = emuPathTextBox.Text.LastIndexOf("\\");

            if (index > -1)
                initialDirectory = emuPathTextBox.Text.Remove(index);
            else
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            using (OpenFileDialog dlg = Emulators2Settings.OpenFileDialog("Select path to emulator", filter, initialDirectory))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    emuPathTextBox.Text = dlg.FileName;
            }
        }

        void emuPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!allowChangedEvents || selectedEmulator == null)
                return;

            EmulatorProfile autoSettings = EmuSettingsAutoFill.Instance.CheckForSettings(emuPathTextBox.Text);
            if (autoSettings == null)
                return;
            
            if (MessageBox.Show(string.Format("Would you like to use the recommended settings for {0}?", autoSettings.Title), "Use recommended settings?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Options.Instance.GetBoolOption("autoconfemu"))
                    updateFilterBox(autoSettings.Filters);

                if (autoSettings.Platform != null && platformComboBox.Text == "Unspecified")
                {
                    int index = platformComboBox.FindStringExact(autoSettings.Platform);
                    if (index > -1)
                    {
                        platformComboBox.SelectedItem = platformComboBox.Items[index];
                        EmuSettingsAutoFill.SetupAspectDropdown(thumbAspectComboBox, EmuSettingsAutoFill.Instance.GetCaseAspect(platformComboBox.Text));
                    }
                }

                if (autoSettings.HasSettings)
                {
                    argumentsTextBox.Text = autoSettings.Arguments;
                    useQuotesCheckBox.Checked = autoSettings.UseQuotes != false; //default to true if null
                    suspendMPCheckBox.Checked = autoSettings.SuspendMP == true;
                    if (autoSettings.WorkingDirectory == EmuSettingsAutoFill.USE_EMULATOR_DIRECTORY)
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(emuPathTextBox.Text);
                        if (file.Exists)
                            workingDirTextBox.Text = file.Directory.FullName;
                    }
                    else
                        workingDirTextBox.Text = autoSettings.WorkingDirectory;

                    mountImagesCheckBox.Checked = autoSettings.MountImages;
                    escExitCheckBox.Checked = autoSettings.EscapeToExit;
                }
            }
        }

        private void updateFilterBox(string filters)
        {
            if (string.IsNullOrEmpty(filters) || selectedEmulator == null)
                return;

            if (filterTextBox.Text == "" || filterTextBox.Text == "*.*")
            {
                filterTextBox.Text = filters;
                return;
            }
            
            List<string> currentFilters = new List<string>();
            currentFilters.AddRange(filterTextBox.Text.ToLower().Split(';'));

            //remove generic filter otherwise all other filters pointless
            if (currentFilters.Contains("*.*"))
                currentFilters.Remove("*.*");

            int filterCount = currentFilters.Count;
            
            string[] newFilters = filters.ToLower().Split(';');

            for (int x = 0; x < newFilters.Length; x++)
            {
                if (!currentFilters.Contains(newFilters[x]))
                    currentFilters.Add(newFilters[x]);
            }

            string newFilterString = "";
            for (int x = 0; x < currentFilters.Count; x++)
            {
                if (x != 0)
                    newFilterString += ";";
                newFilterString += currentFilters[x];
            }

            filterTextBox.Text = newFilterString;
        }

        Emulator newEmu = null;
        private void newEmuButton_Click(object sender, EventArgs e)
        {
            newEmu = null;
            using (Wzd_NewEmu_Main wzd = new Wzd_NewEmu_Main())
            {
                if (wzd.ShowDialog() == DialogResult.OK)
                    newEmu = wzd.NewEmulator;

                if (newEmu != null)
                {
                    updateEmulator();
                    updateProfile();
                    newEmu.Save();
                    Emulators2Settings.Instance.Importer.Restart();
                    using (ThumbGroup thumbGroup = new ThumbGroup(newEmu))
                    {
                        if (wzd.Logo != null)
                        {
                            thumbGroup.FrontCover.Image = wzd.Logo;
                            thumbGroup.SaveThumb(ThumbType.FrontCover);
                            wzd.Logo.Dispose();
                        }
                        if (wzd.Fanart != null)
                        {
                            thumbGroup.Fanart.Image = wzd.Fanart;
                            thumbGroup.SaveThumb(ThumbType.Fanart);
                            wzd.Fanart.Dispose();
                        }
                    }

                    ListViewItem item = new ListViewItem(newEmu.Title) { Tag = newEmu };
                    emulatorListView.Items.Add(item);
                    selectedListItem = item;
                    emulatorListView.SelectedItems.Clear();
                    if (selectedListItem != null)
                        selectedListItem.Selected = true;
                    else if (emulatorListView.Items.Count > 0)
                        emulatorListView.Items[0].Selected = true;
                }
            }
        }

        private void romDirButton_Click(object sender, EventArgs e)
        {
            string title = "Select directory containing Roms";
            string initialDir;
            if (System.IO.Directory.Exists(romDirTextBox.Text))
                initialDir = romDirTextBox.Text;
            else
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (FolderBrowserDialog dlg = Emulators2Settings.OpenFolderDialog(title, initialDir))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    romDirTextBox.Text = dlg.SelectedPath;
            }
        }

        private void workingDirBrowseButton_Click(object sender, EventArgs e)
        {
            string title = "Select working directory";
            string initialDir;
            if (System.IO.Directory.Exists(workingDirTextBox.Text))
                initialDir = workingDirTextBox.Text;
            else if (emuPathTextBox.Text != "" && emuPathTextBox.Text.LastIndexOf("\\") > -1)
                initialDir = emuPathTextBox.Text.Remove(emuPathTextBox.Text.LastIndexOf("\\"));
            else
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            FolderBrowserDialog dlg = Emulators2Settings.OpenFolderDialog(title, initialDir);
            if (dlg.ShowDialog() == DialogResult.OK)
                workingDirTextBox.Text = dlg.SelectedPath;
        }

        private void delEmuButton_Click(object sender, EventArgs e)
        {
            if(selectedEmulator == null || selectedEmulator.UID < 0)
                return;
            
            if (MessageBox.Show(
                string.Format("Are you sure you want to remove {0} and\r\nall of it's associated games from the database?", selectedEmulator.Title), 
                "Delete Emulator?", 
                MessageBoxButtons.YesNo) 
                != DialogResult.Yes)
                return;

            saveSelectedEmulator = false;
            saveThumbs = false;
            saveProfile = false;

            List<Game> games = DB.Instance.GetGames(selectedEmulator);
            if (games.Count > 0)
            {
                Importer importer = Emulators2Settings.Instance.Importer;
                BackgroundTaskHandler<Game> handler = new BackgroundTaskHandler<Game>() { Items = games };
                handler.StatusDelegate = o => { return "removing " + o.Title; };
                handler.ActionDelegate = o =>
                {
                    importer.Remove(o.GameID);
                    using (ThumbGroup thumbGroup = new ThumbGroup(o))
                    {
                        try
                        {
                            if (System.IO.Directory.Exists(thumbGroup.ThumbPath))
                                System.IO.Directory.Delete(thumbGroup.ThumbPath, true);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Error deleting {0} thumb directory - {1}", o.Title, ex.Message);
                        }
                    }
                    return true;
                };

                using (Conf_ProgressDialog progressDlg = new Conf_ProgressDialog(handler))
                    progressDlg.ShowDialog();
            }
            selectedEmulator.Delete();

            if (selectedListItem != null)
            {
                int index = selectedListItem.Index;
                if (index > 0)
                    index--;
                emulatorListView.Items.Remove(selectedListItem);
                if (emulatorListView.Items.Count > index)
                {
                    emulatorListView.SelectedItems.Clear();
                    emulatorListView.Items[index].Selected = true;
                }
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (selectedListItem == null || selectedListItem.Index == 0 || selectedEmulator == null)
                return;

            int index = selectedListItem.Index;
            emulatorListView.Items.Remove(selectedListItem);
            emulatorListView.Items.Insert(index - 1, selectedListItem);
            
            updateButtons();
            updateEmuPositions = true;
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (selectedListItem == null || selectedListItem.Index == emulatorListView.Items.Count - 1 || selectedEmulator == null)
                return;

            int index = selectedListItem.Index;
            emulatorListView.Items.Remove(selectedListItem);
            emulatorListView.Items.Insert(index + 1, selectedListItem);

            updateButtons();
            updateEmuPositions = true;
        }

        #region ToolTip

        void setupToolTip()
        {
            emuToolTip.SetToolTip(newEmuButton, "Create new Emulator");
            emuToolTip.SetToolTip(delEmuButton, "Delete selected Emulator");
            emuToolTip.SetToolTip(upButton, "Move up");
            emuToolTip.SetToolTip(downButton, "Move down");

            emuToolTip.SetToolTip(addProfileButton, "Create new Profile");
            emuToolTip.SetToolTip(renameProfileButton, "Rename Profile");
            emuToolTip.SetToolTip(delProfileButton, "Delete Profile");

            emuToolTip.SetToolTip(romDirTextBox, "The directory where the Games are located");
            emuToolTip.SetToolTip(filterTextBox,
                @"The file filters used to determine games to import, Seperate filters with';'
e.g. '*.n64;*.z64;*.v64;*.rom'"
                );
            emuToolTip.SetToolTip(emuPathTextBox, "(Required) The path to the emulator exe/bat");
            emuToolTip.SetToolTip(workingDirTextBox, "(Optional) The Working Directory of the exe/bat when launched");
            emuToolTip.SetToolTip(argumentsTextBox,
                @"(Optional) Additional arguments to pass to the exe/bat.
The Game path will be appended to the end or will replace
%ROM% if present"
                );
            emuToolTip.SetToolTip(mountImagesCheckBox,
                @"If enabled the Plugin will mount games that are disk images using MediaPortal's VirtualDrive.
The VirtualDrive must be configured and enabled in MediaPortal's Configuration."
                );

        }

        #endregion

        private void btnNewManual_Click(object sender, EventArgs e)
        {
            string filter = "PDF | *.pdf";

            string initialDirectory;
            if (txt_Manual.Text != "" && txt_Manual.Text.LastIndexOf("\\") > -1)
                initialDirectory = txt_Manual.Text.Remove(txt_Manual.Text.LastIndexOf("\\"));
            else
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (OpenFileDialog dlg = Emulators2Settings.OpenFileDialog("Select manual", filter, initialDirectory))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    txt_Manual.Text = dlg.FileName;
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Manual.Text))
                return;

            if (!txt_Manual.Text.ToLower().EndsWith(".pdf") || !System.IO.File.Exists(txt_Manual.Text))
                return;

            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo = new System.Diagnostics.ProcessStartInfo(txt_Manual.Text);
                proc.Start();
            }
        }

        private void videoButton_Click(object sender, EventArgs e)
        {
            if (selectedEmulator == null)
                return;

            string filter = "All files (*.*) | *.*";
            string initialDirectory;

            if (System.IO.File.Exists(videoTextBox.Text))
                initialDirectory = System.IO.Directory.GetParent(videoTextBox.Text).FullName;
            else
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (OpenFileDialog dlg = Emulators2Settings.OpenFileDialog("Select path to preview video", filter, initialDirectory))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    videoTextBox.Text = dlg.FileName;
            }
        }

        private void updateInfoButton_Click(object sender, EventArgs e)
        {
            updateEmuInfo();            
        }

        void updateEmuInfo()
        {
            if (selectedEmulator == null)
                return;

            updateEmulator();
            updateProfile();

            EmulatorInfo lEmuInfo = new EmulatorScraperHandler().UpdateEmuInfo(selectedEmulator.PlatformTitle, (o) => 
            {
                EmulatorInfo emuInfo = (EmulatorInfo)o;
                if (emuInfo == null)
                    return false;

                if (!string.IsNullOrEmpty(emuInfo.Title))
                    selectedEmulator.Title = emuInfo.Title;

                if (!string.IsNullOrEmpty(emuInfo.Developer))
                    selectedEmulator.Company = emuInfo.Developer;

                int grade;
                if (!string.IsNullOrEmpty(emuInfo.Grade) && int.TryParse(emuInfo.Grade, out grade))
                    selectedEmulator.Grade = grade;

                string description = emuInfo.GetDescription();
                if (!string.IsNullOrEmpty(description))
                    selectedEmulator.Description = description;

                using (ThumbGroup thumbGroup = new ThumbGroup(selectedEmulator))
                {
                    if (!string.IsNullOrEmpty(emuInfo.LogoUrl))
                    {
                        thumbGroup.FrontCover.Path = emuInfo.LogoUrl;
                        thumbGroup.SaveThumb(ThumbType.FrontCover);
                    }
                    if (!string.IsNullOrEmpty(emuInfo.FanartUrl))
                    {
                        thumbGroup.Fanart.Path = emuInfo.FanartUrl;
                        thumbGroup.SaveThumb(ThumbType.Fanart);
                    }
                }

                selectedEmulator.Save();
                return true;
            });

            if (lEmuInfo != null)
                setEmulatorToPanel(selectedListItem);
        }

        Conf_EmuThumbRetriever thumbRetriever = null;
        void getEmuArtButton_Click(object sender, EventArgs e)
        {
            if (selectedEmulator == null)
                return;

            updateEmulator();
            updateProfile();

            if (thumbRetriever == null)
                thumbRetriever = new Conf_EmuThumbRetriever(selectedEmulator);
            else
                thumbRetriever.Reset(selectedEmulator);

            if (thumbRetriever.Visible)
                thumbRetriever.Activate();
            else
                thumbRetriever.Show();
        }
    }
}
