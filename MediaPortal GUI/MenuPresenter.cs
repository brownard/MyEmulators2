
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace MyEmulators2
{
    class MenuPresenter
    {
        //Does all the logic behind the different context menus

        public static ListItemProperty ShowSortDialog(int windowID, bool sortEnabled)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.sortby);
                dlg.Add(new GUIListItem(Translator.Instance.defaultsort));
                if (sortEnabled)
                {
                    dlg.Add(new GUIListItem(Translator.Instance.title));
                    dlg.Add(new GUIListItem(Translator.Instance.grade));
                    dlg.Add(new GUIListItem(Translator.Instance.lastplayed));
                    dlg.Add(new GUIListItem(Translator.Instance.playcount));
                    dlg.Add(new GUIListItem(Translator.Instance.year));
                }
                dlg.DoModal(windowID);

                //Sort accordingly
                switch (dlg.SelectedId)
                {
                    case 1:
                        return ListItemProperty.DEFAULT;
                    case 2:
                        return ListItemProperty.TITLE;
                    case 3:
                        return ListItemProperty.GRADE;
                    case 4:
                        return ListItemProperty.LASTPLAYED;
                    case 5:
                        return ListItemProperty.PLAYCOUNT;
                    case 6:
                        return ListItemProperty.YEAR;
                }
            }

            return ListItemProperty.NONE;
        }

        public static bool ShowContext(ExtendedGUIListItem item, GUIPresenter presenter)
        {
            if (item == null || item.AssociatedGame == null)
                return showSimpleContext(presenter);
            else
                return ShowGameDialog(item.AssociatedGame, presenter);
        }

        static bool showSimpleContext(GUIPresenter presenter, int selectedLabel = 0)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Options.Instance.GetStringOption("shownname"));

                dlg.Add(new GUIListItem(Translator.Instance.switchview));
                dlg.Add(new GUIListItem(Translator.Instance.runimport));
                dlg.Add(new GUIListItem(Translator.Instance.options));
                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(Plugin.WINDOW_ID);
                selectedLabel = dlg.SelectedLabel;
                switch (dlg.SelectedId)
                {
                    case 1:
                        presenter.SwitchView();
                        break;
                    case 2:
                        presenter.RestartImporter();
                        break;
                    case 3:
                        if (ShowSettingsDialog())
                            presenter.ReloadOptions();
                        showSimpleContext(presenter, selectedLabel);
                        break;
                }
            }
            return false;
        }

        public static bool ShowGameDialog(Game game, GUIPresenter presenter, int selectedLabel = 0)
        {
            if (game == null)
                return false;

            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                int windowID = Plugin.WINDOW_ID;
                dlg.Reset();
                dlg.SetHeading(Options.Instance.GetStringOption("shownname"));

                GUIListItem playItem = new GUIListItem("Play");
                dlg.Add(playItem);

                GUIListItem profileItem = new GUIListItem(Translator.Instance.profile)
                    {
                        Label2 = game.CurrentProfile.Title
                    };
                dlg.Add(profileItem);

                GUIListItem favouriteItem = new GUIListItem(Translator.Instance.favourite)
                    {
                        Label2 = game.Favourite.ToString()
                    };
                dlg.Add(favouriteItem);

                GUIListItem gradeItem = new GUIListItem(Translator.Instance.grade)
                    {
                        Label2 = game.Grade.ToString()
                    };
                dlg.Add(gradeItem);

                GUIListItem discItem = new GUIListItem(Translator.Instance.disc);
                if (game.Discs.Count > 1)
                {
                    discItem.Label2 = game.CurrentDiscNum.ToString();
                    dlg.Add(discItem);
                }

                GUIListItem goodmergeItem = new GUIListItem(Translator.Instance.goodmerge);
                if (game.IsGoodmerge)
                {
                    goodmergeItem.Label2 = game.CurrentDisc.LaunchFile.Replace(game.Filename, "").Trim();
                    dlg.Add(goodmergeItem);
                }

                GUIListItem viewItem = new GUIListItem(Translator.Instance.switchview);
                dlg.Add(viewItem);                                
                GUIListItem importItem = new GUIListItem(Translator.Instance.retrieveonlineinfo);
                dlg.Add(importItem);
                GUIListItem runImportItem = new GUIListItem(Translator.Instance.runimport);
                dlg.Add(runImportItem);
                GUIListItem settingsItem = new GUIListItem(Translator.Instance.options);
                dlg.Add(settingsItem);

                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(windowID);
                selectedLabel = dlg.SelectedLabel;
                int id = dlg.SelectedId;

                bool result = false;
                bool reload = true;

                if (id < 1)
                    return false;

                if (id == playItem.ItemId)
                {
                    LaunchHandler.Instance.StartLaunch(game);
                    reload = false;
                }
                else if (id == profileItem.ItemId)
                {
                    int profileId = game.SelectedProfileId;
                    if (showProfileDialog(ref profileId, game.GetProfiles(), windowID))
                    {
                        game.SelectedProfileId = profileId;
                        game.SaveGamePlayInfo();
                        result = true;
                    }
                }
                else if (id == favouriteItem.ItemId)
                {
                    bool favourite = game.Favourite;
                    if (ShowBoolDialog(ref favourite, windowID))
                    {
                        game.Favourite = favourite;
                        game.SaveGamePlayInfo();
                        result = true;
                    }
                }
                else if (id == gradeItem.ItemId)
                {
                    int grade = game.Grade;
                    if (showGradeDialog(ref grade, windowID))
                    {
                        game.Grade = grade;
                        game.SaveGamePlayInfo();
                        result = true;
                    }
                }
                else if (id == discItem.ItemId)
                {
                    int disc = game.CurrentDiscNum;
                    if (showDiscSelect(ref disc, game.Discs, windowID))
                    {
                        game.CurrentDiscNum = disc;
                        game.SaveGamePlayInfo();
                        result = true;
                    }
                }
                else if (id == goodmergeItem.ItemId)
                {
                    try
                    {
                        int selectedGoodmergeIndex;
                        List<string> games = Extractor.Instance.ViewFiles(game, out selectedGoodmergeIndex);
                        if (games != null)
                        {
                            string launchFile = games[selectedGoodmergeIndex];
                            if (ShowGoodmergeSelect(ref launchFile, games, game.Filename, windowID))
                            {
                                game.CurrentDisc.LaunchFile = launchFile;
                                game.CurrentDisc.Save();
                                result = true;
                            }
                        }
                    }
                    catch (ExtractException ex)
                    {
                        Emulators2Settings.Instance.ShowMPDialog("{0} - {1}", Translator.Instance.goodmergeerror, ex.Message);
                    }
                }
                else if (id == viewItem.ItemId)
                {
                    presenter.SwitchView();
                    reload = false;
                }                
                else if (id == importItem.ItemId)
                {
                    presenter.AddToImporter(game);
                    reload = false;
                }
                else if (id == runImportItem.ItemId)
                {
                    presenter.RestartImporter();
                    reload = false;
                }
                else if (id == settingsItem.ItemId)
                {
                    if (ShowSettingsDialog())
                    {
                        presenter.ReloadOptions();
                        result = true;
                    }
                }

                if (reload)
                    result = ShowGameDialog(game, presenter, selectedLabel) || result;
                return result;
            }

            return false;
        }

        static bool showProfileDialog(ref int selectedProfileId, List<EmulatorProfile> profiles, int windowID)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.selectprofile);

                int selectedLabel = 0;
                for (int x = 0; x < profiles.Count; x++)
                {
                    dlg.Add(new GUIListItem(profiles[x].Title));
                    if (profiles[x].ID == selectedProfileId)
                        selectedLabel = x;
                }

                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(windowID);
                selectedLabel = dlg.SelectedLabel;

                if (selectedLabel > -1)
                {
                    selectedProfileId = profiles[selectedLabel].ID;
                    return true;
                }
            }
            return false;
        }

        static bool showGradeDialog(ref int grade, int windowID)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.selectgrade);
                for (int i = 0; i < 11; i++)
                {
                    GUIListItem item = new GUIListItem();
                    dlg.Add(item);
                    item.Label = i.ToString(); //set label after adding so we overwrite the default numbering 
                }

                if (grade < 0)
                    grade = 0;
                else if (grade > 10)
                    grade = 10;

                dlg.SelectedLabel = grade;
                dlg.DoModal(windowID);

                if (dlg.SelectedLabel > -1)
                {
                    grade = dlg.SelectedLabel;
                    return true;
                }
            }
            return false;
        }

        public static int ShowLayoutDialog(int windowID)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.layoutheader);
                dlg.Add(new GUIListItem(Translator.Instance.layoutlist));
                dlg.Add(new GUIListItem(Translator.Instance.layouticons));
                dlg.Add(new GUIListItem(Translator.Instance.layoutlargeicons));
                dlg.Add(new GUIListItem(Translator.Instance.layoutfilmstrip));
                dlg.Add(new GUIListItem(Translator.Instance.layoutcoverflow));
                dlg.DoModal(windowID);
                return dlg.SelectedLabel;
            }
            return -1;
        }

        static bool showDiscSelect(ref int selectedDisc, List<GameDisc> discs, int windowID)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.discselect);

                int selectedLabel = 0;
                for (int x = 0; x < discs.Count; x++ )
                {
                    GameDisc disc = discs[x];
                    dlg.Add(new GUIListItem(disc.Name));
                    if (disc.Number == selectedDisc)
                        selectedLabel = x;
                }

                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(windowID);

                if (dlg.SelectedId > 0)
                {
                    selectedDisc = dlg.SelectedId;
                    return true;
                }
            }
            return false;
        }

        public static bool ShowGoodmergeSelect(ref string selectedFile, List<string> files, string prefix, int windowID)
        {
            if (files == null)
                return false;

            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.goodmergeselect);

                bool empty = false;
                bool stripPrefix = !string.IsNullOrEmpty(prefix);
                int selectedLabel = 0;
                if (files.Count < 1)
                {
                    dlg.Add(new GUIListItem(Translator.Instance.goodmergeempty));
                    empty = true;
                }
                else
                    for (int x = 0; x < files.Count; x++)
                    {
                        dlg.Add(new GUIListItem(stripPrefix ? files[x].Replace(prefix, "").Trim() : files[x]));
                        if (files[x] == selectedFile)
                            selectedLabel = x;
                    }

                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(windowID);
                selectedLabel = dlg.SelectedLabel;

                if (empty)
                    return false;

                if (selectedLabel > -1 && selectedLabel < files.Count)
                {
                    selectedFile = files[selectedLabel];
                    return true;
                }
            }
            return false;
        }

        public static StartupState ShowViewsDialog(StartupState currentState, bool showPC)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.switchview);

                int index = (int)currentState;

                dlg.Add(new GUIListItem(Translator.Instance.viewemulators));
                dlg.Add(new GUIListItem(Translator.Instance.viewgroups));
                dlg.Add(new GUIListItem(Translator.Instance.favourites));

                if (showPC)
                    dlg.Add(new GUIListItem(Translator.Instance.pcgames));
                else if (currentState == StartupState.PCGAMES)
                    index = 0;
                
                dlg.SelectedLabel = index;
                dlg.DoModal(Plugin.WINDOW_ID);

                index = dlg.SelectedLabel;
                if (index > -1)
                    return (StartupState)index;
            }
            return currentState;
        }

        public static bool ShowSettingsDialog(int selectedLabel = 0)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.options);

                int startup;
                var startupStates = Options.Instance.GetStartupOptions(out startup);
                string startupValue = startupStates.Single(s => s.Value == startup).Name;
                dlg.Add(new GUIListItem(Translator.Instance.startupview) { Label2 = startupValue });

                bool clickToDetails = Options.Instance.GetBoolOption("clicktodetails");
                dlg.Add(new GUIListItem(Translator.Instance.clicktodetails) { Label2 = clickToDetails.ToString() });

                bool stopMedia = Options.Instance.GetBoolOption("stopmediaplayback");
                dlg.Add(new GUIListItem(Translator.Instance.stopmediaplayback) { Label2 = stopMedia.ToString() });

                bool showSortValue = Options.Instance.GetBoolOption("showsortvalue");
                dlg.Add(new GUIListItem(Translator.Instance.showsortvalue) { Label2 = showSortValue.ToString() });

                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(Plugin.WINDOW_ID);
                selectedLabel = dlg.SelectedLabel;

                if (dlg.SelectedId > 0)
                {
                    switch (dlg.SelectedId)
                    {
                        case 1:
                            if (ShowStringSelect(ref startupValue, startupStates.Select(s => s.Name).ToArray(), Plugin.WINDOW_ID))
                                Options.Instance.UpdateOption("startupstate", startupStates.Single(s => s.Name == startupValue).Value);
                            break;
                        case 2:
                            if (ShowBoolDialog(ref clickToDetails, Plugin.WINDOW_ID))
                                Options.Instance.UpdateOption("clicktodetails", clickToDetails);
                            break;
                        case 3:
                            if (ShowBoolDialog(ref stopMedia, Plugin.WINDOW_ID))
                                Options.Instance.UpdateOption("stopmediaplayback", stopMedia);
                            break;
                        case 4:
                            if (ShowBoolDialog(ref showSortValue, Plugin.WINDOW_ID))
                                Options.Instance.UpdateOption("showsortvalue", showSortValue);
                            break;
                    }
                    ShowSettingsDialog(selectedLabel);
                    return true;
                }
            }
            return false;
        }

        public static bool ShowStringSelect(ref string selectedString, string[] strings, int windowID)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.options);

                int selectedLabel = 0;
                for (int x = 0; x < strings.Length; x++)
                {
                    dlg.Add(new GUIListItem(strings[x]));
                    if (strings[x] == selectedString)
                        selectedLabel = x;
                }

                dlg.SelectedLabel = selectedLabel;
                dlg.DoModal(windowID);
                selectedLabel = dlg.SelectedLabel;

                if (selectedLabel > -1 && selectedLabel < strings.Length)
                {
                    selectedString = strings[selectedLabel];
                    return true;
                }
            }
            return false;
        }

        public static bool ShowBoolDialog(ref bool value, int windowId)
        {
            GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(Translator.Instance.options);
                dlg.Add(new GUIListItem(true.ToString()));
                dlg.Add(new GUIListItem(false.ToString()));
                dlg.SelectedLabel = value ? 0 : 1;
                dlg.DoModal(windowId);
                if (dlg.SelectedLabel > -1)
                {
                    value = dlg.SelectedLabel == 0;
                    return true;
                }
            }
            return false;
        }

        public static bool ShowIntDialog(ref int value, int windowId)
        {
            string input = value.ToString();
            if (GetUserInputString(ref input))
            {
                return int.TryParse(input, out value);
            }
            return false;
        }

        public static bool GetUserInputString(ref string sString, bool password = false)
        {
            VirtualKeyboard keyBoard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
            if (keyBoard == null) return false;
            keyBoard.Reset();
            keyBoard.Text = sString;
            keyBoard.Password = password;
            keyBoard.DoModal(GUIWindowManager.ActiveWindow); // show it...
            if (keyBoard.IsConfirmed) sString = keyBoard.Text;
            return keyBoard.IsConfirmed;
        }
    }
}
