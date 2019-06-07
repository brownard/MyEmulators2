
using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;
using System.IO;
using SevenZip;

namespace myEmulators
{
    public enum gameProperty
    {
        none,
        latestplay,
        grade,
        playcount,
        year,
        genre,
        company
    }

    class MainPresenter
    {
        //Does all the logic behind what to show in the facadeview etc

        private GUIFacadeControl facade;
        private GUIImage[] stars;
        private GUIImage bg_description;
         private GUITextScrollUpControl text_description;
        private GUIButtonControl button_view;
        private GUILabelControl gamelabel_year = null;
        private GUILabelControl gamelabel_genre = null;
        private GUILabelControl gamelabel_company = null;
        private GUILabelControl gamelabel_latestplay = null;
        private GUILabelControl gamelabel_playcount = null;

        public MainPresenter(GUIFacadeControl facade, GUIImage[] stars, GUIImage bg_description, GUITextScrollUpControl text_description, GUIButtonControl button_view, GUILabelControl gamelabel_year, GUILabelControl gamelabel_genre, GUILabelControl gamelabel_company, GUILabelControl gamelabel_latestplay, GUILabelControl gamelabel_playcount)
        {
            this.facade = facade;
            this.stars = stars;
            this.bg_description = bg_description;
            this.text_description = text_description;
            this.button_view = button_view;
            this.gamelabel_year = gamelabel_year;
            this.gamelabel_genre = gamelabel_genre;
            this.gamelabel_company = gamelabel_company;
            this.gamelabel_latestplay = gamelabel_latestplay;
            this.gamelabel_playcount = gamelabel_playcount;
        }        

        private Emulator currentEmulator = null;
        private String currentFolder = null;

        public Emulator CurrentEmulator()
        {
            return currentEmulator;
        }

        public void onFacadeClicked(MediaPortal.GUI.Library.Action.ActionType ButtonPressed)
        {
            ExtendedGUIListItem currentItem = (ExtendedGUIListItem)facade.SelectedListItem;
            
            //Show games for selected emulator
            if (currentEmulator == null && ButtonPressed != MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU)
            {
                if (ButtonPressed == MediaPortal.GUI.Library.Action.ActionType.ACTION_NEXT_ITEM)
                {
                    Executor.launchDocument(currentItem.AssociatedEmulator);
                }
                else
                {
                    currentEmulator = currentItem.AssociatedEmulator;
                    if (currentEmulator.isPc())
                    {
                        fillPCGames();
                    }
                    else
                    {
                        currentFolder = currentEmulator.PathToRoms;
                        fillGames();
                        facade.SelectedListItemIndex = 1;
                        onFacadeAction();
                    }
                }
            }

            //Dive into subdir
            else if (currentItem.AssociatedDirectory != null && ButtonPressed != MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU)
            {
                currentFolder = currentItem.AssociatedDirectory;
                fillGames();
                facade.SelectedListItemIndex = 1;
                onFacadeAction();
            }
            //Execute game
            else if (currentItem.AssociatedGame != null && ButtonPressed != MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU)
            {
                if (ButtonPressed == MediaPortal.GUI.Library.Action.ActionType.ACTION_NEXT_ITEM)
                {
                    Executor.launchDocument(currentItem.AssociatedGame);
                }
                else
                {
                    if (currentItem.AssociatedGame.ParentEmulator.EnableGoodmerge)
                    {
                        if (ButtonPressed == MediaPortal.GUI.Library.Action.ActionType.ACTION_MUSIC_PLAY)
                        {
                            currentItem.AssociatedGame.LaunchFile = "";
                            Executor.launchGame(currentItem.AssociatedGame);
                        }
                        else
                        {
                            if (ButtonPressed == MediaPortal.GUI.Library.Action.ActionType.ACTION_PAUSE)
                            {
                                currentItem.AssociatedGame.LaunchFile = "";
                            }

                            if (currentItem.AssociatedGame.LaunchFile.Trim() != "")
                            {
                                Executor.launchGame(currentItem.AssociatedGame);
                                fillGames();

                                for (int i = 0; i < facade.Count; i++)
                                {
                                    try
                                    {
                                        if (((ExtendedGUIListItem)facade[i]).AssociatedGame.Path == currentItem.AssociatedGame.Path)
                                        {
                                            facade.SelectedListItemIndex = i;
                                            onFacadeAction();
                                        }
                                    }
                                    catch { }
                                }
                            }
                            else
                            {
                                SevenZipCompressor.SetLibraryPath(Options.getStringOption("7zdllpath"));
                                using (SevenZipExtractor tmp = new SevenZipExtractor(currentItem.AssociatedGame.Path))
                                {
                                    string GoodmergeTempPath = "";

                                    if (currentItem.AssociatedGame.ParentEmulator.GoodmergeTempPath.EndsWith("\\"))
                                    {
                                        GoodmergeTempPath = currentItem.AssociatedGame.ParentEmulator.GoodmergeTempPath;
                                    }
                                    else
                                    {
                                        GoodmergeTempPath = currentItem.AssociatedGame.ParentEmulator.GoodmergeTempPath + "\\";
                                    }

                                    if (tmp.ArchiveFileNames.Count == 1)
                                    {
                                        Executor.launchGame(currentItem.AssociatedGame);
                                    }
                                    else
                                    {
                                        setView(0);
                                        facade.Clear();

                                        //prev selected
                                        facade.Add(ThumbsHandler.Instance.createBackDots(currentItem.AssociatedGame.Path));

                                        for (int i = 0; i < tmp.ArchiveFileNames.Count; i++)
                                        {
                                            Game RomListGame = DB.getGame(currentItem.AssociatedGame.Path, currentItem.AssociatedGame.ParentEmulator);
                                            RomListGame.LaunchFile = tmp.ArchiveFileNames[i];
                                            facade.Add(ThumbsHandler.Instance.createGameRomFacadeItem(RomListGame));

                                            Log.Error(new Exception("launch" + RomListGame.LaunchFile));
                                        }

                                        facade.SelectedListItemIndex = 1;
                                        onFacadeAction();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Executor.launchGame(currentItem.AssociatedGame);
                    }
                }
            }
            else if ((currentItem.IsBackDots || ButtonPressed == MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU) && !string.IsNullOrEmpty(currentItem.PrevSelected))
            {
                fillGames();

                for (int i = 0; i < facade.Count; i++)
                {
                    try
                    {
                        if (((ExtendedGUIListItem)facade[i]).AssociatedGame.Path == currentItem.PrevSelected)
                        {
                            facade.SelectedListItemIndex = i;
                            onFacadeAction();
                        }
                    }
                    catch { }
                }
            }
            //Go up one level
            else if ((currentItem.IsBackDots || ButtonPressed == MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU))
            {
                //Go back to all emulators
                if (currentEmulator.isManyEmulators() || currentEmulator.isPc() || currentFolder.Equals(currentEmulator.PathToRoms))
                {
                    currentEmulator = null;
                    currentFolder = null;
                    currentSqlTail = null;
                    currentGameProperty = gameProperty.none;
                    fillEmulators();
                    facade.SelectedListItemIndex = 0;
                    onFacadeAction();
                }
                //Go back to parent directory
                else
                {
                    currentFolder = currentFolder.Remove(currentFolder.LastIndexOf("\\"));
                    fillGames();
                    facade.SelectedListItemIndex = 1;
                }
            }
        }

        public void launch()
        {
            if (!Options.getBoolOption("startwithfavourites"))
            {
                if (Options.getBoolOption("onlyshowpcgames"))
                {
                    fillPCGames();
                }
                else
                {
                    fillEmulators();
                    facade.SelectedListItemIndex = 0;
                }
            }
            else
            {
                fillSomeGames("WHERE favourite='True' ORDER BY title ASC", gameProperty.none);
            }
        }

        public void fillEmulators()
        {
            setView(Options.getIntOption("viewemus"));
            facade.Clear();
            Emulator[] emulators = DB.getEmulators();

            if (DB.getPCGames().Length > 0)
            {
                facade.Add(ThumbsHandler.Instance.createEmulatorFacadeItem(DB.getPCDetails()));
            }

            foreach (Emulator emu in emulators)
            {
                facade.Add(ThumbsHandler.Instance.createEmulatorFacadeItem(emu));
            }
            facade.Data = emulators;
        }

        private void fillGames()
        {
            setView(currentEmulator.View);
            facade.Clear();
            facade.Add(ThumbsHandler.Instance.createBackDots(""));
            /*
            string[] directories = Directory.GetDirectories(currentFolder);
            Array.Sort(directories);

            foreach (String dir in directories)
            {
                FileAttributes att = new DirectoryInfo(dir).Attributes;
                if ((att | FileAttributes.Hidden) != att)
                {
                    facade.Add(ThumbsHandler.Instance.createSubDir(dir));
                }
            }*/
            foreach (Game game in DB.getGamesFromDB(currentEmulator))
            {
                facade.Add(ThumbsHandler.Instance.createGameFacadeItem(game));
            }
        }

        private void fillPCGames()
        {
            setView(Options.getIntOption("viewpcgames"));
            facade.Clear();

            if (!Options.getBoolOption("onlyshowpcgames"))
            {
                facade.Add(ThumbsHandler.Instance.createBackDots(""));
            }

            foreach (Game game in DB.getPCGames())
            {
                facade.Add(ThumbsHandler.Instance.createGameFacadeItem(game));
            }
        }

        public void displayStars(int grade)
        {
            try
            {
                if (grade < 1 || grade > 10)
                {
                    foreach (GUIImage star in stars)
                    {
                        star.SetFileName("");
                    }
                }
                else
                {
                    for (int i = 0; i < stars.Length; i++)
                    {
                        stars[i].SetFileName(GUIGraphicsContext.Skin + @"\Media\stargood.png");
                        if (i < grade)
                        {
                            stars[i].Dimmed = false;
                        }
                        else
                        {
                            stars[i].Dimmed = true;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                //Skin does not have the stars images
            }
        }

        public void displayDescription(String description)
        {
            try
            {
                text_description.Label = description;
            }
            catch (NullReferenceException)
            {
                //Skin does not have the description label
            }
        }

        public void displayDescriptionBackground(bool visible)
        {
            try
            {
                if (visible)
                {
                    bg_description.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    bg_description.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            catch (NullReferenceException)
            {
                //Skin does not have the description background image
            }
        }

        public void displayYear(int year)
        {
            try
            {
                if (year > 0)
                {
                    gamelabel_year.Label = "" + year;
                }
                else
                {
                    gamelabel_year.Label = "";
                }
            }
            catch (NullReferenceException)
            {
                //Skin does not have the year label
            }
        }

        public void displayGenre(String genre)
        {
            try
            {
                gamelabel_genre.Label = genre;
            }
            catch (NullReferenceException)
            {
                //Skin does not have the genre label
            }
        }

        public void displayCompany(String company)
        {
            try
            {
                gamelabel_company.Label = company;
            }
            catch (NullReferenceException)
            {
                //Skin does not have the company label
            }
        }

        public void displayLatestPlay(DateTime latestplay)
        {
            try
            {
                String x = Translator.getString(TranslatorString.never);
                if (latestplay > DateTime.MinValue)
                {
                    x = latestplay.ToShortDateString();
                }
                gamelabel_latestplay.Label = x;
            }
            catch (NullReferenceException)
            {
                //Skin does not have the latestplay label
            }
        }

        public void displayPlayCount(int playcount)
        {
            try
            {
                gamelabel_playcount.Label = "" + playcount;
            }
            catch (NullReferenceException)
            {
                //Skin does not have the playcount label
            }
        }

        public bool isInRoot()
        {
            return currentEmulator == null;
        }
        
        public void goBackOneLevel()
        {
            //facade.SelectedListItemIndex = 0;
            onFacadeClicked(MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU);
        }

        public void onFacadeAction()
        {
            Game item = null;
            Emulator emu = null;
            if (facade.Count > 0)
            {
                item = ((ExtendedGUIListItem)facade.SelectedListItem).AssociatedGame;
                emu = ((ExtendedGUIListItem)facade.SelectedListItem).AssociatedEmulator;
            }

            if (item != null)
            {
                GUIPropertyManager.SetProperty("#emulator_title", item.ParentEmulator.ToString());
                GUIPropertyManager.SetProperty("#game_grade", item.Grade.ToString());
                
                GUIPropertyManager.SetProperty("#fanartpath", ThumbsHandler.Instance.createGameArt(item, "Fanart", true));
                GUIPropertyManager.SetProperty("#backcoverpath", ThumbsHandler.Instance.createGameArt(item, "BoxBack", true));
                GUIPropertyManager.SetProperty("#titlescreenpath", ThumbsHandler.Instance.createGameArt(item, "TitleScreenshot", true));
                GUIPropertyManager.SetProperty("#ingamescreenpath", ThumbsHandler.Instance.createGameArt(item, "IngameScreenshot", true));
                
                displayStars(item.Grade);
                if (item.Description.Length > 0)
                {
                    displayDescriptionBackground(true);
                    displayDescription(item.Description);
                    GUIPropertyManager.SetProperty("#game_description", item.Description);
                }
                else
                {
                    displayDescriptionBackground(false);
                    displayDescription("");
                    GUIPropertyManager.SetProperty("#game_description", "");
                }

                if (item.Yearmade > 0)
                {
                    displayYear(item.Yearmade);
                    GUIPropertyManager.SetProperty("#game_yearmade", "" + item.Yearmade);
                }
                else
                {
                    displayYear(0);
                    GUIPropertyManager.SetProperty("#game_yearmade", "");
                }

                displayGenre(item.Genre);
                GUIPropertyManager.SetProperty("#game_genre", item.Genre);

                displayCompany(item.Company);
                GUIPropertyManager.SetProperty("#game_company", item.Company);

                if (item.Latestplay.Year > 2000)
                {
                    displayLatestPlay(item.Latestplay);
                    GUIPropertyManager.SetProperty("#game_latestplay", (item.Latestplay.CompareTo(DateTime.MinValue) == 0) ? Translator.getString(TranslatorString.never) : item.Latestplay.ToShortDateString());
                }
                else
                {
                    displayLatestPlay(DateTime.MinValue);
                    GUIPropertyManager.SetProperty("#game_latestplay", "");
                }

                if (item.Playcount > 0)
                {
                    displayPlayCount(item.Playcount);
                    GUIPropertyManager.SetProperty("#game_playcount", "" + item.Playcount);
                }
                else
                {
                    displayPlayCount(0);
                    GUIPropertyManager.SetProperty("#game_playcount", "");
                }
            }
            else if (emu != null)
            {
                GUIPropertyManager.SetProperty("#emulator_title", " ");
                GUIPropertyManager.SetProperty("#game_grade", emu.Grade.ToString());
                GUIPropertyManager.SetProperty("#fanartpath", ThumbsHandler.Instance.createEmulatorArt(emu, "fanart"));
                GUIPropertyManager.SetProperty("#backcoverpath", "");
                GUIPropertyManager.SetProperty("#titlescreenpath", "");
                GUIPropertyManager.SetProperty("#ingamescreenpath", "");
                GUIPropertyManager.SetProperty("#game_yearmade", "");

                displayStars(emu.Grade);
                
                displayDescriptionBackground(true);
                
                displayDescription(emu.Description);
                if (emu.Yearmade > 0)
                {
                    displayYear(emu.Yearmade);
                    GUIPropertyManager.SetProperty("#game_yearmade", emu.Yearmade.ToString());
                }
                //displayGenre(item.Genre);
                displayCompany(emu.Company);
                

                //AJP
                GUIPropertyManager.SetProperty("#game_description", emu.Description);
                
                GUIPropertyManager.SetProperty("#game_genre", "");
                GUIPropertyManager.SetProperty("#game_company", emu.Company);
            }
            else
            {
                GUIPropertyManager.SetProperty("#emulator_title", " ");

                
                displayStars(0);
                displayDescription("");
                displayDescriptionBackground(false);
                displayYear(0);
                displayGenre("");
                displayCompany("");
                displayLatestPlay(DateTime.MinValue);
                displayPlayCount(0);
                

                //AJP
                GUIPropertyManager.SetProperty("#game_description", "");
                GUIPropertyManager.SetProperty("#game_yearmade", "");
                GUIPropertyManager.SetProperty("#game_genre", "");
                GUIPropertyManager.SetProperty("#game_company", "");
                GUIPropertyManager.SetProperty("#game_latestplay", "");
                GUIPropertyManager.SetProperty("#game_playcount", "");
                GUIPropertyManager.SetProperty("#game_grade", "0");
            }

            if (facade.Count > 0)
            {
#if MP11
                if (facade.View == GUIFacadeControl.ViewMode.Filmstrip)
#else
                if (facade.CurrentLayout == GUIFacadeControl.Layout.Filmstrip)
#endif
                {
                    ThumbsHandler.Instance.setFilmstripImage(facade);
                }
            }

        }

        public void setView(int view)
        {
            switch (view)
            {
#if MP11
                case 0:
                    {
                        facade.View = GUIFacadeControl.ViewMode.List;
                        button_view.Label = Translator.getString(TranslatorString.viewlist);
                        break;
                    }
                case 1:
                    {
                        facade.View = GUIFacadeControl.ViewMode.SmallIcons;
                        button_view.Label = Translator.getString(TranslatorString.viewicons);
                        break;
                    }
                case 2:
                    {
                        facade.View = GUIFacadeControl.ViewMode.LargeIcons;
                        button_view.Label = Translator.getString(TranslatorString.viewlargeicons);
                        break;
                    }
                case 3:
                    {
                        facade.View = GUIFacadeControl.ViewMode.Filmstrip;
                        button_view.Label = Translator.getString(TranslatorString.viewfilmstrip);
                        break;
                    }
#else
                case 0:
                    {
                        facade.CurrentLayout = GUIFacadeControl.Layout.List;
                        button_view.Label = Translator.getString(TranslatorString.viewlist);
                        break;
                    }
                case 1:
                    {
                        facade.CurrentLayout = GUIFacadeControl.Layout.SmallIcons;
                        button_view.Label = Translator.getString(TranslatorString.viewicons);
                        break;
                    }
                case 2:
                    {
                        facade.CurrentLayout = GUIFacadeControl.Layout.LargeIcons;
                        button_view.Label = Translator.getString(TranslatorString.viewlargeicons);
                        break;
                    }
                case 3:
                    {
                        facade.CurrentLayout = GUIFacadeControl.Layout.Filmstrip;
                        button_view.Label = Translator.getString(TranslatorString.viewfilmstrip);
                        break;
                    }
                case 4:
                    facade.CurrentLayout = GUIFacadeControl.Layout.CoverFlow;
                    button_view.Label = "CoverFlow";
                    break;
#endif
            }
            if (currentEmulator == null)
            {
                Options.updateIntOption("viewemus", view);
            }
            else if (currentEmulator.UID >= 0)
            {
                currentEmulator.View = view;
                DB.saveEmulator(currentEmulator);
            }
            else if (currentEmulator.isPc())
            {
                Options.updateIntOption("viewpcgames", view);
            }
        }

        public void switchView()
        {
            //Copy all items
            GUIListItem[] itemsBefore = new GUIListItem[facade.Count];
            for (int i = 0; i < facade.Count; i++)
            {
                itemsBefore[i] = facade[i];
            }
            //Switch view
#if MP11
            switch (facade.View)
            {
                case GUIFacadeControl.ViewMode.List: { setView(1); break; }
                case GUIFacadeControl.ViewMode.SmallIcons: { setView(2); break; }
                case GUIFacadeControl.ViewMode.LargeIcons: { setView(3); break; }
                case GUIFacadeControl.ViewMode.Filmstrip: { setView(0); break; }
            }
#else
            switch (facade.CurrentLayout)
            {
                case GUIFacadeControl.Layout.List: { setView(1); break; }
                case GUIFacadeControl.Layout.SmallIcons: { setView(2); break; }
                case GUIFacadeControl.Layout.LargeIcons: { setView(3); break; }
                case GUIFacadeControl.Layout.Filmstrip: { setView(4); break; }
                case GUIFacadeControl.Layout.CoverFlow: { setView(0); break; }
            }
#endif

            //Restore all items
            facade.Clear();
            for (int i = 0; i < itemsBefore.Length; i++)
            {
                facade.Add(itemsBefore[i]);
            }
        }

        string currentSqlTail = null;
        gameProperty currentGameProperty = gameProperty.none;

        public void fillSomeGames(String sqlTail, gameProperty secondLabel)
        {
            if (currentEmulator == null)
            {
                currentEmulator = new Emulator(EmulatorType.manyemulators);
            }
            currentSqlTail = sqlTail;
            currentGameProperty = secondLabel;
            setView(0);
            facade.Clear();
            facade.Add(ThumbsHandler.Instance.createBackDots(""));

            List<GUIListItem> firstPart = new List<GUIListItem>();
            List<GUIListItem> secondPart = new List<GUIListItem>();

            foreach (Game game in DB.getSomeGames(sqlTail))
            {

                ExtendedGUIListItem item = ThumbsHandler.Instance.createGameFacadeItem(game);
                switch (secondLabel)
                {
                    case gameProperty.grade: { item.Label2 = "(" + game.Grade + "/10)"; break; }
                    case gameProperty.latestplay: { item.Label2 = "(" + game.Latestplay.ToShortDateString() + ")"; break; }
                    case gameProperty.playcount: { item.Label2 = "(" + game.Playcount + ")"; break; }
                    case gameProperty.year: { item.Label2 = "(" + game.Yearmade + ")"; break; }
                    case gameProperty.genre: { item.Label2 = "(" + game.Genre + ")"; break; }
                    case gameProperty.company: { item.Label2 = "(" + game.Company + ")"; break; }
                }
                if (item.Label2.Equals("(0)") || item.Label2.Equals("(0/10)") || item.Label2.Equals("(" + DateTime.MinValue.ToShortDateString() + ")") || item.Label2.Equals("()"))
                {
                    item.Label2 = "n/a";
                }
                if (Options.getBoolOption("hidelabeldecorations"))
                {
                    item.Label2 = item.Label2.Replace("(", "").Replace(")", "");
                }
                if (currentEmulator.isManyEmulators() || (!currentEmulator.isManyEmulators() && game.ParentEmulator.UID == currentEmulator.UID))
                {
                    if (!item.Label2.Equals("n/a"))
                    {
                        firstPart.Add(item);
                    }
                    else
                    {
                        secondPart.Add(item);
                    }
                }
            }
            
            //Put all the ones with empty Label2 in the end
            foreach (GUIListItem item in firstPart)
            {
                facade.Add(item);
            }
            foreach (GUIListItem item in secondPart)
            {
                facade.Add(item);
            }
        }

        public void onShowContextMenu(int windowID, MenuPresenter menuHandler)
        {
            Game item = ((ExtendedGUIListItem)facade.SelectedListItem).AssociatedGame;
            if (item != null)
            {
                menuHandler.showGameDialog(windowID, item);
                displayStars(item.Grade);
            }
        }

        public void Refresh()
        {
            int index = facade.SelectedListItemIndex;
            if (currentEmulator != null)
            {
                if (currentEmulator.isManyEmulators() && currentSqlTail != null)
                    fillSomeGames(currentSqlTail, currentGameProperty);
                else if (currentEmulator.isPc())
                    fillPCGames();
                else
                    fillGames();
            }
            else
                fillEmulators();

            if (index >= facade.Count)
                index = facade.Count - 1;
            facade.SelectedListItemIndex = index;
        }
    }
}
