using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using Cornerstone.MP;
using System.Text.RegularExpressions;
using MediaPortal.Player;

namespace MyEmulators2
{
    [MediaPortal.Configuration.PluginIcons("MyEmulators2.Resources.Emulators2Icon.png", "MyEmulators2.Resources.Emulators2Icon_faded.png")]
    public class Plugin : GUIWindow, ISetupForm
    {
        public static readonly int WINDOW_ID = 7942;

        GUIPresenter newGUIHandler = null;
        bool firstLoad = true;
        ImageSwapper backdrop = null;

        [SkinControlAttribute(1230)] protected GUIImage fanartControl1 = null;
        [SkinControlAttribute(1231)] protected GUIImage fanartControl2 = null;

        [SkinControlAttribute(1232)] protected GUILabelControl fanArtEnabled = null;
        [SkinControlAttribute(1233)] protected GUILabelControl gameArtEnabled = null;
        [SkinControlAttribute(1235)] protected GUILabelControl videoPreviewEnabled = null;
        [SkinControlAttribute(1234)] protected GUILabelControl showVideoPreviewControl = null;

        //The facade list with emulators and games
        [SkinControlAttribute(50)]
        protected GUIFacadeControl facade = null;
        [SkinControlAttribute(51)]
        protected GUIListControl goodmergeList = null;
        
        //The buttons in the menu to the left
        [SkinControlAttribute(10)] protected GUIButtonControl buttonLayout = null;
        [SkinControlAttribute(11)] protected GUISortButtonControl buttonSort = null;
        [SkinControlAttribute(12)] protected GUIButtonControl buttonViews = null;
        [SkinControlAttribute(13)] protected GUIButtonControl buttonImport = null;

        [SkinControlAttribute(6)] protected GUIButtonControl details_play = null;

        g_Player.StoppedHandler onVideoStopped = null;
        g_Player.EndedHandler onVideoEnded = null;

        public string PluginName()
        {
            return "Emulators 2";
        }

        public string Description()
        {
            return "Emulators 2\r\rEmulators 2 is a plugin for MediaPortal that allows you to browse and play your emulated games. It supports any number of emulators with all their games. Finding the game you want to play is made easy, and will launch the emulator with the game for you when you want to play it.\r\rMy Emulators 2 also supports a number of extra features. It will show you all kinds of information for the system and games you have. It has images like the box art for the front and back, ingame screen shots, title screen, and fanart. Ratings can be set as well.";
        }
        
        public string Author()
        {
            return "Brownard, Craige1";
        }
        
        public bool CanEnable()
        {
            return true;
        }
        
        public bool DefaultEnabled()
        {
            return true;
        }
        
        public bool HasSetup()
        {
            return true;
        }

        public int GetWindowId()
        {
            return WINDOW_ID;
        }
        
        public override int GetID
        {
            get
            {
                return GetWindowId();
            }
        }

        //Show Configuration
        public void ShowPlugin()
        {
            startUp();
            //Translator.Instance.createEngXml();
            Emulators2Settings.Instance.IsConfig = true;
            new Conf_Main().ShowDialog();
            Options.Instance.Save();
            Emulators2Settings.Instance.IsConfig = false;
        }

        //Show Plugin
        public override bool Init()
        {
            Logger.LogDebug("Init()");
            startUp();
            Translator.Instance.TranslateSkin();
            return Load(GUIGraphicsContext.Skin + "\\" + Emulators2Settings.SKIN_FILE);
        }

        public override void DeInit()
        {
            Logger.LogDebug("DeInit()");
            if (newGUIHandler != null)
                newGUIHandler.Dispose();
            Executor.Instance.Dispose();
            Options.Instance.Save();
            DB.Instance.Dispose();
            base.DeInit();
        }

        void startUp()
        {
            GroupHandler.Instance.Init();
            Emulators2Settings.Instance.Init();
        }

        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = Options.Instance.GetStringOption("shownname");
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = Emulators2Settings.HOME_HOVER;
            return true;
        }

        public override string GetModuleName()
        {
            return Options.Instance.GetStringOption("shownname");
        }

        protected override void OnPageLoad()
        {
            if (firstLoad)
            {
                firstLoad = false;

                GUIPropertyManager.SetProperty("#Emulators2.PreviewVideo.playing", "no");
                //Image Handlers
                backdrop = new ImageSwapper();
                backdrop.ImageResource.Delay = Options.Instance.GetIntOption("fanartdelay");
                backdrop.PropertyOne = "#Emulators2.CurrentItem.fanartpath";
                backdrop.PropertyTwo = "#Emulators2.CurrentItem.fanartpath2";
                
                newGUIHandler = new GUIPresenter();
                newGUIHandler.OnSortAscendingChanged += new GUIPresenter.SortAscendingChanged(newGUIHandler_OnSortAscendingChanged);
                newGUIHandler.OnPreviewVideoStatusChanged += new GUIPresenter.PreviewVideoStatusChanged(newGUIHandler_OnPreviewVideoStatusChanged);
                GUIPropertyManager.SetProperty("#Emulators2.plugintitle", Options.Instance.GetStringOption("shownname"));

                onVideoStopped = new g_Player.StoppedHandler(g_Player_PlayBackStopped);
                onVideoEnded = new g_Player.EndedHandler(g_Player_PlayBackEnded);
            }

            base.OnPageLoad();
            DBItem startupItem = null;
            bool launch = false;
            getStartupSettings(ref startupItem, ref launch);

            if (buttonSort != null)
            {
                buttonSort.IsAscending = newGUIHandler.SortAscending;
                buttonSort.SortChanged += new SortEventHandler(newGUIHandler.OnSort);
            }

            if (Options.Instance.GetBoolOption("showfanart"))
            {
                backdrop.GUIImageOne = fanartControl1;
                backdrop.GUIImageTwo = fanartControl2;
            }

            if (gameArtEnabled != null)
                gameArtEnabled.Visible = Options.Instance.GetBoolOption("showgameart"); //update gameart dummy control visibility

            if (Options.Instance.GetBoolOption("showvideopreview"))
            {
                if (videoPreviewEnabled != null)
                    videoPreviewEnabled.Visible = true; //videoPreview dummy
            }

            g_Player.PlayBackStopped += onVideoStopped;
            g_Player.PlayBackEnded += onVideoEnded;

            newGUIHandler.Load(facade, backdrop, startupItem, launch, showVideoPreviewControl, goodmergeList, details_play);
        }

        string previewVidFilename = null;
        bool previewVidPlaying = false;
        bool previewVidLoop = false;

        void g_Player_PlayBackEnded(g_Player.MediaType type, string filename)
        {
            if (previewVidPlaying && filename == previewVidFilename)
            {
                string playing;
                g_Player.Stop();
                if (previewVidLoop)
                {
                    previewVidPlaying = true;
                    playing = "yes";
                    GUIGraphicsContext.ShowBackground = false;
                    g_Player.Play(filename, MediaPortal.Player.g_Player.MediaType.Video);
                }
                else
                {
                    playing = "no";
                    previewVidPlaying = false;
                }
                GUIPropertyManager.SetProperty("#Emulators2.PreviewVideo.playing", playing);
            }
        }

        void g_Player_PlayBackStopped(g_Player.MediaType type, int stoptime, string filename)
        {
            if (previewVidPlaying && filename == previewVidFilename)
            {
                previewVidPlaying = false;
                GUIPropertyManager.SetProperty("#Emulators2.PreviewVideo.playing", "no");
            }
        }

        void newGUIHandler_OnPreviewVideoStatusChanged(string filename, bool loop)
        {
            GUIWindowManager.SendThreadCallback((p1, p2, o) => 
            {
                string playing;
                previewVidFilename = filename;
                if (string.IsNullOrEmpty(filename))
                {
                    if (g_Player.Playing)
                        g_Player.Stop();
                    GUIGraphicsContext.ShowBackground = true;
                    playing = "no";

                }
                else
                {
                    previewVidLoop = loop;
                    previewVidPlaying = true;
                    GUIGraphicsContext.ShowBackground = false;
                    g_Player.Play(filename, MediaPortal.Player.g_Player.MediaType.Video);
                    playing = "yes";
                }
                GUIPropertyManager.SetProperty("#Emulators2.PreviewVideo.playing", playing);
                return 0;
            }
            , 0, 0, null);
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            base.OnClicked(controlId, control, actionType);

            if (facade != null && controlId == facade.GetID)
            {
                switch (actionType)
                {
                    case Action.ActionType.ACTION_SELECT_ITEM:
                        newGUIHandler.ItemSelected();
                        break;
                    case Action.ActionType.ACTION_NEXT_ITEM:
                        newGUIHandler.LaunchDocument();
                        break;
                }
            }
            else if (control == goodmergeList)
            {
                newGUIHandler.GoodMergeClicked(actionType);
            }
            //Layout button
            else if (buttonLayout != null && controlId == buttonLayout.GetID)
            {
                int view = MenuPresenter.ShowLayoutDialog(GetID);
                if (view < 0)
                    return;

                newGUIHandler.setLayout(view, true);
            }
            //Most played button
            else if (buttonSort == control)
            {
                ListItemProperty sortProperty = MenuPresenter.ShowSortDialog(GetID, newGUIHandler.SortEnabled);
                if (sortProperty != ListItemProperty.NONE && sortProperty != newGUIHandler.SortProperty)
                {
                    newGUIHandler.SortProperty = sortProperty;
                    newGUIHandler.Sort();
                }
            }
            else if (buttonViews == control)
            {
                newGUIHandler.SwitchView();
            }
            else if (details_play == control)
            {
                newGUIHandler.LaunchGame();
            }
            else if (buttonImport == control)
            {
                newGUIHandler.RestartImporter();
            }
        }

        public override void OnAction(MediaPortal.GUI.Library.Action action)
        {
            //To allow Esc to go back one level instead of exiting
            if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU)
            {
                if (newGUIHandler.GoBack()) //if true back handled by plugin
                    return;
            }
            else if (action.wID == Action.ActionType.ACTION_PAUSE)
            {
                if (newGUIHandler.ToggleDetails())
                    return;
            }
            else if (action.wID == Action.ActionType.ACTION_PLAY)
            {
                if (newGUIHandler.LaunchGame(true))
                    return;
            }
            base.OnAction(action); //else exit
        }

        protected override void OnShowContextMenu()
        {
            base.OnShowContextMenu();
            newGUIHandler.ShowContextMenu();
        }

        protected override void OnPageDestroy(int new_windowId)
        {
            newGUIHandler.OnPageDestroy();
            GUIGraphicsContext.ShowBackground = true;
            g_Player.PlayBackStopped -= onVideoStopped;
            g_Player.PlayBackEnded -= onVideoEnded;
        }

        void newGUIHandler_OnSortAscendingChanged(bool sortAscending)
        {
            if (buttonSort != null)
                buttonSort.IsAscending = sortAscending;
        }

        void getStartupSettings(ref DBItem startupItem, ref bool launch)
        {
            //startupItem = DB.Instance.GetGame(5359);
            //launch = false;
            //return;

            if (string.IsNullOrEmpty(_loadParameter))
                return;

            //Check and load startup parameters
            Regex paramReg = new Regex(@"([A-z]+)\s*:\s*([-]?[A-z0-9]+)");
            List<string> loadParams = new List<string>(_loadParameter.Split(';'));
            foreach (string param in loadParams)
            {
                Match m = paramReg.Match(param);
                if (!m.Success)
                    continue;

                switch (m.Groups[1].Value.ToLower())
                {
                    case "emulator":
                        int id;
                        if (int.TryParse(m.Groups[2].Value, out id))
                            startupItem = DB.Instance.GetEmulator(id);
                        break;
                    case "rom":
                        //int id;
                        if (int.TryParse(m.Groups[2].Value, out id))
                        {
                            startupItem = DB.Instance.GetGame(id);
                        }
                        break;
                    case "group":
                        RomGroup group;
                        if (GroupHandler.Instance.GroupNames.TryGetValue(m.Groups[2].Value.ToLower(), out group))
                            startupItem = group;
                        break;
                    case "launch":
                        bool tryLaunch;
                        if (bool.TryParse(m.Groups[2].Value, out tryLaunch))
                            launch = tryLaunch;
                        break;
                }
            }
        }        
    }
}
