using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Cornerstone.MP;
using MediaPortal.Player;
using MediaPortal.Dialogs;

namespace MyEmulators2
{
    partial class GUIPresenter : IDisposable
    {
        #region Events
        public delegate void SortAscendingChanged(bool sortAscending);
        public event SortAscendingChanged OnSortAscendingChanged;
        public delegate void PreviewVideoStatusChanged(string filename, bool loop);
        public event PreviewVideoStatusChanged OnPreviewVideoStatusChanged;
        #endregion

        #region Private variables
        int imageLoadingDelay = 0;
        ImageSwapper backdrop = null;
        AsyncImageResource cover = null;
        AsyncImageResource backCover = null;
        AsyncImageResource titleScreen = null;
        AsyncImageResource inGameScreen = null;

        string defaultFanart = "";
        string defaultCover = "";

        GUIListControl goodmergeList = null;
        GUIButtonControl detailsPlayButton = null;

        GUIFacadeControl facade = null;
        List<ExtendedGUIListItem> facadeItems = null;
        object gameItemLock = new object();
        Dictionary<int, ExtendedGUIListItem> gameItems = null;

        int lastitemIndex = 0;
        ExtendedGUIListItem lastItem = null;

        ViewState currentView = ViewState.Items;
        int facadeIndex = 0;
        bool allowLayoutChange = true;
        int currentLayout = -1;
        int groupsLayout = 0;
        bool showSortValue = false;

        StartupState startupState = StartupState.EMULATORS;

        DBItem startupItem = null;
        Emulator startupEmu = null;
        Game startupGame = null;
        RomGroup startupGroup = null;

        bool launchStartupItem = false;
        bool showGoodmergeDialogOnce = true;
        bool alwaysShowGoodmergeDialog = false;

        object videoPreviewSync = new object();
        bool previewVidEnabled = false;
        bool loopVideoPreview = false;
        int videoPreviewDelay = 2000;
        System.Threading.Timer videoPreviewTimer = null;
        string currentVideoId = null;
        string currentVideoPath = null;

        bool previewVidPlaying = false;

        bool clickToDetails = true;
        int detailsDefaultControl = 6;

        GUILabelControl showVideoPreviewControl = null;
        bool showVideoPreview()
        {
            return showVideoPreviewControl != null && showVideoPreviewControl.Visible;
        }
        #endregion

        public GUIPresenter()
        {
            imageLoadingDelay = Options.Instance.GetIntOption("gameartdelay");

            cover = new AsyncImageResource();
            cover.Property = "#Emulators2.CurrentItem.coverpath";
            backCover = new AsyncImageResource();
            backCover.Property = "#Emulators2.CurrentItem.backcoverpath";
            titleScreen = new AsyncImageResource();
            titleScreen.Property = "#Emulators2.CurrentItem.titlescreenpath";
            inGameScreen = new AsyncImageResource();
            inGameScreen.Property = "#Emulators2.CurrentItem.ingamescreenpath";

            setImageDelay(imageLoadingDelay);                        
            defaultFanart = Emulators2Settings.Instance.DefaultFanart;
            defaultCover = Emulators2Settings.Instance.DefaultLogo;

            ReloadOptions();
            initImporter();
        }

        public void Load(GUIFacadeControl facade, ImageSwapper backdrop, DBItem startupItem, bool launch, GUILabelControl showVideoPreviewControl, GUIListControl goodmergeList, GUIButtonControl detailsPlayButton)
        {
            this.facade = facade;
            this.goodmergeList = goodmergeList;
            this.backdrop = backdrop;
            this.showVideoPreviewControl = showVideoPreviewControl;
            this.detailsPlayButton = detailsPlayButton;

            clearGUIProperties();
            currentLayout = -1;

            int prevWindow = GUIWindowManager.GetPreviousActiveWindow();
            if (GUIWindowManager.ActiveWindow == prevWindow || prevWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_VIDEO)
            {
                launchStartupItem = false;
                Refresh(true); //Catch when MP is refreshing plugin after resizing/restoring and maintain current view
            }
            else
            {
                resetStartupItem(startupItem);
                this.launchStartupItem = launch;
                SortProperty = ListItemProperty.DEFAULT; //set skin property
                GUIPropertyManager.SetProperty("#Emulators2.sortenabled", "no");
                currentView = ViewState.Items;
                setFacadeVisibility(true);
                loadStartupItems(0);
            }
            //resume import if previously paused
            resumeImporter();
        }

        public void ReloadOptions()
        {
            clickToDetails = Options.Instance.GetBoolOption("clicktodetails");
            startupState = Options.Instance.GetStartupState();
            showSortValue = Options.Instance.GetBoolOption("showsortvalue");
            previewVidEnabled = Options.Instance.GetBoolOption("showvideopreview");

            showGoodmergeDialogOnce = Options.Instance.GetBoolOption("showgmdialogonce");
            alwaysShowGoodmergeDialog = !showGoodmergeDialogOnce && Options.Instance.GetBoolOption("showgmdialog");

            loopVideoPreview = Options.Instance.GetBoolOption("loopvideopreview");
            videoPreviewDelay = Options.Instance.GetIntOption("videopreviewdelay");
        }

        #region Item Selected Handlers

        public void ItemSelected()
        {
            ExtendedGUIListItem selectedListItem = facade.SelectedListItem as ExtendedGUIListItem;
            int parentIndex = facade.SelectedListItemIndex;
            itemSelected(selectedListItem, 0, parentIndex);
        }

        void itemSelected(ExtendedGUIListItem selectedListItem, int index, int parentIndex, bool reverse = false, bool changeLayout = true, bool allowSortChange = true)
        {
            bool handled = false;
            allowLayoutChange = changeLayout;
            setImageDelay(0);

            if (selectedListItem == null)
            {
                handled = loadStartupItems(index);
            }
            else if (selectedListItem.IsGroup)
            {
                handled = groupSelected(selectedListItem, index, parentIndex, allowSortChange);
            }
            else if (selectedListItem.AssociatedEmulator != null)
            {
                handled = emulatorSelected(selectedListItem, index, parentIndex);
            }
            else if (selectedListItem.AssociatedGame != null)
            {
                gameSelected(selectedListItem);
                handled = true;
            }

            allowLayoutChange = true;

            if (!handled) //if no items to display,
            {
                if (reverse && selectedListItem != null) //if we're going back and not already on top level
                {
                    itemSelected(selectedListItem.Parent, parentIndex, selectedListItem.ParentIndex, true, changeLayout); //go back another level
                    return;
                }
                else
                    Emulators2Settings.Instance.ShowMPDialog(Translator.Instance.noitemstodisplay); //else show message to user
            }
            setFacadeVisibility(currentView != ViewState.Details);
            setImageDelay(imageLoadingDelay);
        }               

        bool loadStartupItems(int index)
        {
            lastItem = null;
            lastitemIndex = 0;

            if (startupItem != null)
            {
                return handleStartupItem(index);
            }

            List<DBItem> items = new List<DBItem>();
            int layout;
            if (startupState == StartupState.FAVOURITES)
            {
                items.AddRange(DB.Instance.GetGames("WHERE favourite='True' ORDER BY title ASC", false));
                layout = Options.Instance.GetIntOption("viewfavourites");
            }
            else if (startupState == StartupState.PCGAMES)
            {
                items.AddRange(DB.Instance.GetGames(Emulator.GetPC()));
                layout = Options.Instance.GetIntOption("viewpcgames");
            }
            else if (startupState == StartupState.GROUPS)
            {
                foreach (RomGroup group in GroupHandler.Instance.Groups)
                {
                    group.RefreshThumbs();
                    items.Add(group);
                }
                layout = groupsLayout;
            }
            else
            {
                foreach (Emulator emu in DB.Instance.GetEmulators())
                    items.Add(emu);
                layout = Options.Instance.GetIntOption("viewemus");
            }

            if (!setItemsToFacade(items.ToArray(), null, 0, index))
                return false;

            setLayout(layout);
            return true;
        }

        void resetStartupItem(DBItem startupItem)
        {
            startupEmu = null;
            startupGame = null;
            startupGroup = null;
            this.startupItem = startupItem;

            if (startupItem == null)
                return;

            startupEmu = startupItem as Emulator;
            if (startupEmu != null)
                return;

            startupGame = startupItem as Game;
            if (startupGame != null)
                return;

            startupGroup = startupItem as RomGroup;
        }

        bool handleStartupItem(int index)
        {
            if (startupGame != null)
            {
                setItemsToFacade(new[] { startupGame }, null, 0, 0);
                gameSelected(facade.SelectedListItem as ExtendedGUIListItem, true);
                if (launchStartupItem)
                {
                    launchStartupItem = false; //otherwise continuous loop of rom launch
                    LaunchHandler.Instance.StartLaunch(startupGame);
                }
                return true;
            }

            if (startupEmu != null)
            {
                setItemsToFacade(DB.Instance.GetGames(startupEmu), null, 0, index);
                setLayout(startupEmu.View);
                return true;
            }

            if (startupGroup != null)
            {
                startupGroup.Refresh();
                SortProperty = startupGroup.SortProperty;
                if (sortAsc == startupGroup.SortDescending)
                {
                    sortAsc = !startupGroup.SortDescending;
                    if (OnSortAscendingChanged != null)
                        OnSortAscendingChanged(sortAsc);
                }
                setItemsToFacade(startupGroup.GroupItems, null, 0, index);
                if (startupGroup.Layout < 0)
                    startupGroup.Layout = currentLayout;
                else
                    setLayout(startupGroup.Layout);
                return true;
            }

            return false;
        }

        bool emulatorSelected(ExtendedGUIListItem selectedListItem, int index, int parentIndex)
        {
            if (selectedListItem.AssociatedEmulator == null)
                return false;

            if (!setItemsToFacade(DB.Instance.GetGames(selectedListItem.AssociatedEmulator), selectedListItem, parentIndex, index))
                return false;
            lastItem = selectedListItem;
            lastitemIndex = parentIndex;

            //setLayout must be called after setting facade items and lastItem info
            setLayout(selectedListItem.AssociatedEmulator.View);
            return true;
        }

        bool groupSelected(ExtendedGUIListItem selectedListItem, int index, int parentIndex, bool allowSortChange)
        {
            RomGroup romGroup = selectedListItem.RomGroup;
            if (romGroup == null)
                return false;

            romGroup.Refresh();
            if (romGroup.GroupItems.Count < 1)
                return false;

            if (allowSortChange)
            {
                SortProperty = romGroup.SortProperty;
                if (sortAsc == romGroup.SortDescending)
                {
                    sortAsc = !romGroup.SortDescending;
                    if (OnSortAscendingChanged != null)
                        OnSortAscendingChanged(sortAsc);
                }
            }

            setItemsToFacade(romGroup.GroupItems, selectedListItem, parentIndex, index);

            lastItem = selectedListItem;
            lastitemIndex = parentIndex;
            if (romGroup.Layout < 0)
                romGroup.Layout = currentLayout;
            else
                setLayout(romGroup.Layout);
            return true;
        }

        void gameSelected(ExtendedGUIListItem selectedItem)
        {
            gameSelected(selectedItem, clickToDetails);
        }
        void gameSelected(ExtendedGUIListItem selectedItem, bool lClickToDetails)
        {
            if (selectedItem == null)
                return;
            Game selectedGame = selectedItem.AssociatedGame;
            if (selectedGame == null)
                return;

            int itemCount = 0;
            int selectedGoodmerge = -1;
            List<string> goodMergeGames = null;

            if (selectedGame.IsGoodmerge)
            {
                try
                {
                    goodMergeGames = Extractor.Instance.ViewFiles(selectedGame, out selectedGoodmerge);
                }
                catch (ExtractException) { }

                if (goodMergeGames != null)
                {
                    if (goodMergeGames.Count < 1)
                        goodMergeGames = null;
                    else
                        itemCount = goodMergeGames.Count;
                }

                selectedGame.GoodmergeFiles = goodMergeGames;
            }


            if (goodmergeList != null)
                GUIControl.ClearControl(Plugin.WINDOW_ID, goodmergeList.GetID);

            if (goodMergeGames != null)
            {
                if (selectedGoodmerge < 0 || selectedGoodmerge >= goodMergeGames.Count)
                    selectedGoodmerge = 0;

                if (lClickToDetails && goodmergeList != null)
                {
                    selectedGame.CurrentDisc.LaunchFile = goodMergeGames[selectedGoodmerge];
                    bool selected = false;
                    for (int x = 0; x < goodMergeGames.Count; x++)
                    {
                        GUIListItem item = new GUIListItem(goodMergeGames[x].Replace(selectedGame.Filename, "").Trim()) { DVDLabel = goodMergeGames[x] };
                        GUIControl.AddListItemControl(Plugin.WINDOW_ID, goodmergeList.GetID, item);
                        if (!selected && x == selectedGoodmerge)
                        {
                            selected = true;
                            item.Selected = true;
                            goodmergeList.SelectedListItemIndex = x;                            
                        }
                    }
                }
                else if (selectedGame.CurrentDisc.LaunchFile != goodMergeGames[selectedGoodmerge])
                    selectedGame.CurrentDisc.LaunchFile = "";
            }

            GUIPropertyManager.SetProperty("#Emulators2.CurrentItem.goodmergecount", itemCount.ToString());
            if (lClickToDetails)
                toggleDetails(selectedItem);
            else
                launchGame(selectedGame);
        }

        #endregion

        #region Facade

        bool setItemsToFacade<T>(IEnumerable<T> items, ExtendedGUIListItem parent, int parentIndex, int selectedIndex) where T : DBItem
        {
            bool sortable = false;
            facadeItems = new List<ExtendedGUIListItem>();
            lock (gameItemLock)
            {
                gameItems = new Dictionary<int, ExtendedGUIListItem>();
                int listPosition = 0;
                foreach(DBItem item in items)
                {
                    ExtendedGUIListItem facadeItem = item.CreateGUIListItem();
                    if (facadeItem.AssociatedGame != null)
                        gameItems[facadeItem.AssociatedGame.GameID] = facadeItem;
                    sortable = sortable || facadeItem.Sortable;
                    facadeItem.OnItemSelected += new GUIListItem.ItemSelectedHandler(onFacadeItemSelected);
                    facadeItem.Parent = parent;
                    facadeItem.ParentIndex = parentIndex;
                    facadeItem.ListPosition = listPosition;
                    listPosition++;
                    facadeItems.Add(facadeItem);
                }
            }

            if (facadeItems.Count < 1)
                return false;

            if (sortable)
            {
                SortEnabled = true;
                if (sortProperty != ListItemProperty.DEFAULT)
                    facadeItems.Sort(new ListItemComparer(sortProperty, !sortAsc));
                else
                    sortable = false;
            }
            else
            {
                SortProperty = ListItemProperty.DEFAULT;
                SortEnabled = false;
            }

            GUIPropertyManager.SetProperty("#Emulators2.currentfilter", parent != null ? parent.Label : startupState.Translate());
            sortable = sortable && showSortValue;

            GUIControl.ClearControl(Plugin.WINDOW_ID, facade.GetID);
            for (int i = 0; i < facadeItems.Count; i++)
            {
                if (sortable)
                    facadeItems[i].SetLabel2(sortProperty);
                GUIControl.AddListItemControl(Plugin.WINDOW_ID, facade.GetID, facadeItems[i]);
            }

            setFacadeIndex(selectedIndex);

            if (currentView != ViewState.Details)
            {
                onFacadeItemSelected(facade.SelectedListItem, facade);
            }

            return true;
        }

        void onFacadeItemSelected(GUIListItem item, GUIControl parent)
        {
            onFacadeItemSelected(item, parent, false);
        }

        void onFacadeItemSelected(GUIListItem item, GUIControl parent, bool detailsUpdate)
        {
            if (currentView == ViewState.Details)
            {
                if (!detailsUpdate)
                    return;
            }
            
            ExtendedGUIListItem selectedItem = item as ExtendedGUIListItem;
            if (selectedItem == null)
                return;

            ThumbGroup thumbs;
            string lVideoId;
            string lVideoPath;
            lock (selectedItem.SyncRoot)
            {
                selectedItem.SetGUIProperties();
                thumbs = selectedItem.ThumbGroup;
                lVideoId = selectedItem.VideoPreviewId;
                lVideoPath = selectedItem.VideoPreview;
            }

            if (thumbs != null)
            {
                cover.Filename = thumbs.FrontCoverDefaultPath;
                backdrop.Filename = thumbs.FanartDefaultPath;
                backCover.Filename = thumbs.BackCover.Path;
                titleScreen.Filename = thumbs.TitleScreen.Path;
                inGameScreen.Filename = thumbs.InGame.Path;
            }
            else
            {
                cover.Filename = defaultCover;
                backdrop.Filename = defaultFanart;
                backCover.Filename = string.Empty;
                titleScreen.Filename = string.Empty;
                inGameScreen.Filename = string.Empty;
            }

            if (!previewVidEnabled)
                return;

            //preview videos
            lock (videoPreviewSync)
            {
                if (!string.IsNullOrEmpty(currentVideoId) && currentVideoId == lVideoId)
                    return;
                //keep reference to current list item
                currentVideoId = lVideoId;
                currentVideoPath = lVideoPath;

                //stop any playing media
                if (previewVidPlaying)
                {
                    previewVidPlaying = false;
                    if (OnPreviewVideoStatusChanged != null)
                        OnPreviewVideoStatusChanged(null, false);
                }
                
                if (videoPreviewTimer != null)
                    videoPreviewTimer.Dispose();

                if (!string.IsNullOrEmpty(currentVideoPath))
                {
                    videoPreviewTimer = new System.Threading.Timer(videoPreviewTimer_Elapsed, lVideoId, videoPreviewDelay, System.Threading.Timeout.Infinite);
                }
            }
        }

        //called when videoPreviewTimer has elapsed
        void videoPreviewTimer_Elapsed(object state)
        {
            lock (videoPreviewSync)
            {
                //if video item is no longer selected in facade, return
                if (!showVideoPreview() || state.ToString() != currentVideoId || !System.IO.File.Exists(currentVideoPath))
                    return;

                //else play preview video
                previewVidPlaying = true;
                if (OnPreviewVideoStatusChanged != null)
                    OnPreviewVideoStatusChanged(currentVideoPath, loopVideoPreview);
            }
        }

        void clearGUIProperties()
        {
            //Image handlers
            if (backdrop != null)
                backdrop.Filename = string.Empty;

            cover.Filename = string.Empty;
            backCover.Filename = string.Empty;
            titleScreen.Filename = string.Empty;
            inGameScreen.Filename = string.Empty;

            GUIPropertyManager.SetProperty("#Emulators2.currentfilter", "");
            ExtendedGUIListItem.ClearGUIProperties();
        }
        
        void setFacadeIndex(int index)
        {
            facade.SelectedListItemIndex = index;
            if (facade.CurrentLayout == GUIFacadeControl.Layout.Filmstrip)
            {
                //Workaround for filmstrip selected index
                GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_ITEM_SELECT, facade.WindowId, 0, facade.FilmstripLayout.GetID, index, 0, null);
                GUIGraphicsContext.SendMessage(msg);
            }
            else if (facade.CurrentLayout == GUIFacadeControl.Layout.CoverFlow)
            {
                facade.CoverFlowLayout.SelectCard(index); //this is dizzying on large lists, is there a better way??
            }
        }

        void setFacadeVisibility(bool visible)
        {
            if (facade == null || facade.Visible == visible)
                return;
            facade.Visible = visible;
            if (facade.ListLayout != null)
                facade.ListLayout.Visible = visible;
            if (facade.ThumbnailLayout != null)
                facade.ThumbnailLayout.Visible = visible;
            if (facade.FilmstripLayout != null)
                facade.FilmstripLayout.Visible = visible;
            if (facade.CoverFlowLayout != null)
                facade.CoverFlowLayout.Visible = visible;
            if (detailsPlayButton != null)
                detailsPlayButton.Visible = !visible;
        }

        #endregion

        #region Layout

        public void setLayout(int view, bool update = false)
        {
            if (!allowLayoutChange || view < 0 || view == currentLayout)
                return;

            int currentIndex = facade.SelectedListItemIndex;
            string layout = Translator.Instance.layout + ": ";
            switch (view)
            {
                case 0:
                    facade.CurrentLayout = GUIFacadeControl.Layout.List;
                    layout += Translator.Instance.layoutlist;
                    break;
                case 1:
                    facade.CurrentLayout = GUIFacadeControl.Layout.SmallIcons;
                   layout += Translator.Instance.layouticons;
                    break;
                case 2:
                    facade.CurrentLayout = GUIFacadeControl.Layout.LargeIcons;
                    layout += Translator.Instance.layoutlargeicons;
                    break;
                case 3:
                    facade.CurrentLayout = GUIFacadeControl.Layout.Filmstrip;
                    layout += Translator.Instance.layoutfilmstrip;
                    break;
                case 4:
                    facade.CurrentLayout = GUIFacadeControl.Layout.CoverFlow;
                    layout += Translator.Instance.layoutcoverflow;
                    break;
                default:
                    return;
            }
            GUIPropertyManager.SetProperty("#Emulators2.Label.currentlayout", layout);
            setFacadeIndex(currentIndex);
            currentLayout = view;
            //only save layout state when user has changed layout
            if (!update)
                return;

            if (lastItem == null)
            {
                if (startupItem != null)
                {
                    if (startupEmu != null)
                    {
                        startupEmu.View = view;
                        startupEmu.Save();
                    }
                    else if (startupGroup != null)
                    {
                        if (startupGroup.Favourite)
                        {
                            Options.Instance.UpdateOption("viewfavourites", view);
                            startupGroup.Layout = view;
                        }
                    }
                }
                else
                    switch (startupState)
                    {
                        case StartupState.EMULATORS:
                            Options.Instance.UpdateOption("viewemus", view);
                            break;
                        case StartupState.FAVOURITES:
                            Options.Instance.UpdateOption("viewfavourites", view);
                            break;
                        case StartupState.PCGAMES:
                            Options.Instance.UpdateOption("viewpcgames", view);
                            break;
                        case StartupState.GROUPS:
                            groupsLayout = view;
                            break;
                    }
            }
            else
            {
                if (lastItem.IsGroup)
                {
                    if (lastItem.IsFavourites)
                        Options.Instance.UpdateOption("viewfavourites", view);
                    lastItem.RomGroup.Layout = view;
                }
                else if (lastItem.AssociatedEmulator != null)
                {
                    lastItem.AssociatedEmulator.View = view;
                    lastItem.AssociatedEmulator.Save();
                }
            }

            GUIControl.FocusControl(Plugin.WINDOW_ID, facade.GetID);
        }

        #endregion

        #region Public Methods

        //Updates the GUI with latest info whilst maintaining current state
        public void Refresh(bool pageLoad = false)
        {
            if (facade == null)
                return;

            clearGUIProperties();
            int index = pageLoad ? facadeIndex : facade.SelectedListItemIndex;
            itemSelected(lastItem, index, lastitemIndex, true, pageLoad, false);
            if (currentView == ViewState.Details)
            {
                ExtendedGUIListItem item = facade.SelectedListItem as ExtendedGUIListItem;
                if (item != null && item.AssociatedGame != null && detailsItem != null && detailsItem.AssociatedGame.GameID == item.AssociatedGame.GameID)
                    detailsItem = item;

                if (pageLoad)
                    gameSelected(detailsItem, true);
                else
                    onFacadeItemSelected(detailsItem, facade, true);
            }
        }

        public bool GoBack()
        {
            if (currentView == ViewState.Details)
            {
                toggleDetails(null, true);
                if (lastItem != null || startupGame == null)
                    return true;

            }
            else if (lastItem != null)
            {
                itemSelected(lastItem.Parent, lastitemIndex, lastItem.ParentIndex, true);
                return true;
            }
            return false;
        }

        public void ShowContextMenu()
        {
            ExtendedGUIListItem item = null;
            bool detailsUpdate = false;
            if (currentView == ViewState.Details)
            {
                item = detailsItem;
                detailsUpdate = true;
            }
            else if (facade.Focus)
                item = (ExtendedGUIListItem)facade.SelectedListItem;

            if (MenuPresenter.ShowContext(item, this))
            {
                if (detailsUpdate)
                    gameSelected(item, true);
                else
                    onFacadeItemSelected(item, facade, detailsUpdate); //Update GUI
            }
        }

        public void SwitchView()
        {
            bool showPC = DB.Instance.GetGames(Emulator.GetPC()).Count > 0;
            StartupState newState = MenuPresenter.ShowViewsDialog(startupState, showPC);
            if (startupItem != null || newState != startupState)
            {
                startupState = newState;
                startupItem = null;
                toggleDetails(null, true);
                loadStartupItems(0);
            }
        }

        public void LaunchDocument()
        {
            if (facade == null)
                return;
            if (facade.SelectedListItem == null)
                return;

            ExtendedGUIListItem item = (ExtendedGUIListItem)facade.SelectedListItem;
            Logger.LogDebug("Opening {0} manual", item.Label);
            if (item.AssociatedEmulator != null)
                Executor.LaunchDocument(item.AssociatedEmulator);
            else if (item.AssociatedGame != null)
                Executor.LaunchDocument(item.AssociatedGame);
        }

        public void UpdateGame(Game game)
        {
            if (game == null)
                return;

            ExtendedGUIListItem item = null;
            lock (gameItemLock)
                if (gameItems == null || !gameItems.TryGetValue(game.GameID, out item))
                    return;

            GUIWindowManager.SendThreadCallback((a, b, c) =>
            {
                item.UpdateGameInfo(game);
                if (facade != null && facade.SelectedListItem == item)
                {
                    if (currentView == ViewState.Details)
                    {
                        if (detailsItem != null && detailsItem.AssociatedGame != null && item.AssociatedGame != null && detailsItem.AssociatedGame.GameID == item.AssociatedGame.GameID)
                            toggleDetails(item);
                    }
                    else
                        item.ItemSelected(facade);
                }

                return 0;
            }, 0, 0, null);
        }

        object refreshSync = new object();
        System.Threading.Timer refreshTimer = null;
        bool facadeNeedsUpdate = false;

        public void UpdateFacade()
        {
            lock (refreshSync)
            {
                facadeNeedsUpdate = true;
                if (refreshTimer == null)
                    refreshTimer = new System.Threading.Timer(updateFacade, null, 500, System.Threading.Timeout.Infinite);
                else
                    refreshTimer.Change(500, System.Threading.Timeout.Infinite);
            }
        }

        void updateFacade(object state)
        {
            lock (refreshSync)
            {
                if (!facadeNeedsUpdate)
                    return;

                facadeNeedsUpdate = false;
                GUIWindowManager.SendThreadCallback(refreshCallback, 0, 0, null);
            }
        }

        int refreshCallback(int param1, int param2, object data)
        {
            Refresh();
            return 0;
        }

        public bool LaunchGame(bool auto = false)
        {
            Game game = null;
            if (currentView == ViewState.Details && detailsItem != null)
            {
                game = detailsItem.AssociatedGame;
            }
            else if (facade.Focus)
            {
                ExtendedGUIListItem item = facade.SelectedListItem as ExtendedGUIListItem;
                if (item != null)
                    game = item.AssociatedGame;
            }
            if (game != null)
            {
                if (auto)
                    game.GoodmergeFiles = null;
                launchGame(game);
                return true;
            }

            return false;
        }

        void launchGame(Game game)
        {
            if (game.GoodmergeFiles != null && game.GoodmergeFiles.Count > 0)
            {
                if (alwaysShowGoodmergeDialog || (showGoodmergeDialogOnce && string.IsNullOrEmpty(game.CurrentDisc.LaunchFile)))
                {
                    string launchFile = "";
                    if (!MenuPresenter.ShowGoodmergeSelect(ref launchFile, game.GoodmergeFiles, game.Filename, Plugin.WINDOW_ID))
                        return;
                    game.CurrentDisc.LaunchFile = launchFile;
                    game.CurrentDisc.Save();
                }
            }
            
            if (previewVidPlaying)
            {
                previewVidPlaying = false;
                if (OnPreviewVideoStatusChanged != null)
                    OnPreviewVideoStatusChanged(null, false);
            }

            LaunchHandler.Instance.StartLaunch(game);
        }

        ExtendedGUIListItem detailsItem = null;
        public bool ToggleDetails()
        {
            if (currentView == ViewState.Details)
            {
                toggleDetails(null, true);
                return true;
            }
            else if (facade.Focus)
            {
                ExtendedGUIListItem item = facade.SelectedListItem as ExtendedGUIListItem;
                if (item != null)
                {
                    gameSelected(item, true);
                    return true;
                }
            }
            return false;
        }

        void toggleDetails(ExtendedGUIListItem detailsItem, bool hide = false)
        {
            int controlId;
            setFacadeVisibility(hide);
            if (hide)
            {
                currentView = ViewState.Items;
                controlId = facade.GetID;
                ExtendedGUIListItem item = (ExtendedGUIListItem)facade.SelectedListItem;
                if (this.detailsItem != null && item != null && item.AssociatedGame != null && this.detailsItem.AssociatedGame.GameID == item.AssociatedGame.GameID)
                    item.UpdateGameInfo(this.detailsItem.AssociatedGame);
                this.detailsItem = null;
                onFacadeItemSelected(facade.SelectedListItem, facade);
            }
            else
            {
                currentView = ViewState.Details;
                controlId = detailsDefaultControl;
                this.detailsItem = detailsItem;
                if (detailsItem != null)
                    onFacadeItemSelected(detailsItem, facade, true);
            }
            GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_SETFOCUS, Plugin.WINDOW_ID, 0, controlId, 0, 0, null);
            GUIGraphicsContext.SendMessage(msg);

        }

        #endregion

        #region Sort

        bool sortEnabled = false;
        public bool SortEnabled
        {
            get { return sortEnabled; }
            set
            {
                if (sortEnabled == value)
                    return;
                sortEnabled = value;
                GUIPropertyManager.SetProperty("#Emulators2.sortenabled", sortEnabled ? "yes" : "no");
            }
        }

        bool sortAsc = true;
        public bool SortAscending
        {
            get { return sortAsc; }
        }

        ListItemProperty sortProperty = ListItemProperty.DEFAULT;
        public ListItemProperty SortProperty
        {
            get { return sortProperty; }
            set 
            {
                if (value == ListItemProperty.NONE)
                    return;

                sortProperty = value;
                sortAsc = false;
                string sortLabel = Translator.Instance.sortby + ": ";
                switch (sortProperty)
                {
                    case ListItemProperty.COMPANY:
                        sortLabel += Translator.Instance.developer;
                        sortAsc = true;
                        break;
                    case ListItemProperty.DEFAULT:
                        sortLabel += Translator.Instance.defaultsort;
                        break;
                    case ListItemProperty.GRADE:
                        sortLabel += Translator.Instance.grade;
                        break;
                    case ListItemProperty.LASTPLAYED:
                        sortLabel += Translator.Instance.lastplayed;
                        break;
                    case ListItemProperty.PLAYCOUNT:
                        sortLabel += Translator.Instance.playcount;
                        break;
                    case ListItemProperty.TITLE:
                        sortLabel += Translator.Instance.title;
                        sortAsc = true;
                        break;
                    case ListItemProperty.YEAR:
                        sortLabel += Translator.Instance.year;
                        break;
                }
                GUIPropertyManager.SetProperty("#Emulators2.sortlabel", sortLabel);
                if (OnSortAscendingChanged != null)
                    OnSortAscendingChanged(sortAsc);
            }
        }

        public void Sort()
        {
            if (facadeItems == null || facadeItems.Count < 1)
                return;

            facadeItems.Sort(new ListItemComparer(sortProperty, !sortAsc));

            GUIControl.ClearControl(Plugin.WINDOW_ID, facade.GetID);
            for (int i = 0; i < facadeItems.Count; i++)
            {
                if (showSortValue)
                    facadeItems[i].SetLabel2(sortProperty);
                GUIControl.AddListItemControl(Plugin.WINDOW_ID, facade.GetID, facadeItems[i]);
            }
            setFacadeIndex(0);
        }

        public void OnSort(object sender, SortEventArgs e)
        {
            sortAsc = e.Order != System.Windows.Forms.SortOrder.Descending;
            Sort();
        }

        #endregion

        #region Goodmerge

        public void GoodMergeClicked(MediaPortal.GUI.Library.Action.ActionType actionType)
        {
            switch (actionType)
            {
                case MediaPortal.GUI.Library.Action.ActionType.ACTION_SELECT_ITEM:
                    goodmergeSelected();
                    break;
            }
        }

        void goodmergeSelected()
        {
            GUIListItem selectedItem = goodmergeList.SelectedListItem;
            if (selectedItem == null || selectedItem.Selected)
                return;
            foreach (GUIListItem item in goodmergeList.ListItems)
            {
                item.Selected = false;
            }
            selectedItem.Selected = true;
            if (detailsItem.AssociatedGame != null && !string.IsNullOrEmpty(selectedItem.DVDLabel))
            {
                detailsItem.AssociatedGame.CurrentDisc.LaunchFile = selectedItem.DVDLabel;
                detailsItem.AssociatedGame.CurrentDisc.Save();
            }
        }

        #endregion

        void setImageDelay(int delay)
        {
            if (backdrop != null)
                backdrop.ImageResource.Delay = delay;
            cover.Delay = delay;
            backCover.Delay = delay;
            titleScreen.Delay = delay;
            inGameScreen.Delay = delay;
        }

        #region IDisposable Members

        public void Dispose()
        {
            lock(videoPreviewSync)
                if (videoPreviewTimer != null)
                {
                    videoPreviewTimer.Dispose();
                    videoPreviewTimer = null;
                }
            lock (refreshSync)
                if (refreshTimer != null)
                {
                    refreshTimer.Dispose();
                    refreshTimer = null;
                }
            importer.Stop();
        }

        #endregion

        internal void OnPageDestroy()
        {
            //exiting plugin so stop any preview vids
            if (previewVidPlaying)
            {
                previewVidPlaying = false;
                if (OnPreviewVideoStatusChanged != null)
                    OnPreviewVideoStatusChanged(null, false);
            }
            facadeIndex = facade.SelectedListItemIndex;
            Options.Instance.UpdateOption("laststartupstate", (int)startupState);
            importer.Pause();
        }
    }
}
