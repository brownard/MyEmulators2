using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;

namespace MyEmulators2
{
    public class ExtendedGUIListItem: GUIListItem
    {
        public ExtendedGUIListItem(String strLabel) : base(strLabel)
        {
            AssociatedEmulator = null;
            AssociatedGame = null;
            Label = strLabel;
        }

        private Emulator associatedEmulator = null;
        public Emulator AssociatedEmulator
        {
            get { return associatedEmulator; }
            set 
            { 
                associatedEmulator = value;
                if (associatedEmulator != null)
                {
                    videoPreview = associatedEmulator.VideoPreview;
                    if (!string.IsNullOrEmpty(videoPreview))
                        VideoPreviewId = "emu" + associatedEmulator.UID.ToString();
                }
            }
        }

        private Game associatedGame = null;
        public Game AssociatedGame
        {
            get { return associatedGame; }
            set 
            {
                thumbGroup = null;
                associatedGame = value;
                if (associatedGame != null)
                {
                    ReleaseYear = associatedGame.Yearmade;
                    PlayCount = associatedGame.Playcount;
                    LastPlayed = associatedGame.Latestplay;
                    Company = associatedGame.Company;
                    Grade = associatedGame.Grade;
                    videoPreview = associatedGame.VideoPreview;
                    if (string.IsNullOrEmpty(videoPreview) && Options.Instance.GetBoolOption("defaultvideopreview"))
                        videoPreview = associatedGame.ParentEmulator.VideoPreview;
                    if (!string.IsNullOrEmpty(videoPreview))
                        VideoPreviewId = "game" + associatedGame.GameID.ToString();
                }                    
            }
        }

        bool isFavourites = false;
        public bool IsFavourites
        {
            get { return isFavourites; }
            set { isFavourites = value; }
        }

        ExtendedGUIListItem parent = null;
        public ExtendedGUIListItem Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        int parentIndex = 0;
        public int ParentIndex
        {
            get { return parentIndex; }
            set { parentIndex = value; }
        }

        object syncRoot = new object();
        public object SyncRoot { get { return syncRoot; } }

        ThumbGroup thumbGroup = null;
        public ThumbGroup ThumbGroup
        {
            get
            {
                if (thumbGroup == null)
                {
                    if (associatedEmulator != null)
                        thumbGroup = new ThumbGroup(associatedEmulator);
                    else if (associatedGame != null)
                        thumbGroup = new ThumbGroup(associatedGame);
                    else if (RomGroup != null)
                        thumbGroup = RomGroup.ThumbGroup;
                }
                return thumbGroup;
            }
        }        

        string videoPreview = null;
        public string VideoPreview
        {
            get 
            {
                return videoPreview;
            }
        }
        public string VideoPreviewId { get; private set; }

        public RomGroup RomGroup { get; set; }
        public bool IsGroup { get; set; }

        public bool Sortable
        {
            get { return associatedGame != null; }
        }

        public int ReleaseYear { get; set; }
        public int PlayCount { get; set; }
        public DateTime LastPlayed { get; set; }
        public string Company { get; set; }
        public int Grade { get; set; }

        public void SetLabel2(ListItemProperty property)
        {
            if (associatedGame == null)
                return;

            switch (property)
            {
                case ListItemProperty.COMPANY:
                    Label2 = associatedGame.Company;
                    break;
                case ListItemProperty.GRADE:
                    Label2 = associatedGame.Grade.ToString();
                    break;
                case ListItemProperty.LASTPLAYED:
                    if (associatedGame.Latestplay != DateTime.MinValue)
                        Label2 = associatedGame.Latestplay.ToShortDateString();
                    else
                        Label2 = Translator.Instance.never;
                    break;
                case ListItemProperty.PLAYCOUNT:
                    Label2 = associatedGame.Playcount.ToString();
                    break;
                case ListItemProperty.YEAR:
                    if (ReleaseYear > 0)
                        Label2 = ReleaseYear.ToString();
                    else
                        Label2 = "";
                    break;
                default:
                    Label2 = "";
                    break;
            }
        }

        static Dictionary<string, string> getProps()
        {
            Dictionary<string, string> guiProperties = new Dictionary<string, string>();
            guiProperties.Add("#Emulators2.CurrentItem.isemulator", "no");
            guiProperties.Add("#Emulators2.CurrentItem.isgame", "no");

            guiProperties.Add("#Emulators2.CurrentItem.title", "");
            guiProperties.Add("#Emulators2.CurrentItem.emulatortitle", "");
            guiProperties.Add("#Emulators2.CurrentItem.coverflowlabel", "");
            guiProperties.Add("#Emulators2.CurrentItem.description", "");
            guiProperties.Add("#Emulators2.CurrentItem.year", "");
            guiProperties.Add("#Emulators2.CurrentItem.genre", "");
            guiProperties.Add("#Emulators2.CurrentItem.company", "");
            guiProperties.Add("#Emulators2.CurrentItem.latestplaydate", "");
            guiProperties.Add("#Emulators2.CurrentItem.latestplaytime", "");
            guiProperties.Add("#Emulators2.CurrentItem.playcount", "");
            guiProperties.Add("#Emulators2.CurrentItem.grade", "");
            guiProperties.Add("#Emulators2.CurrentItem.caseaspect", "0");

            guiProperties.Add("#Emulators2.CurrentItem.goodmerge", "no");
            guiProperties.Add("#Emulators2.CurrentItem.favourite", "no");
            guiProperties.Add("#Emulators2.CurrentItem.isgoodmerge", "False");
            guiProperties.Add("#Emulators2.CurrentItem.isfavourite", "False");

            guiProperties.Add("#Emulators2.CurrentItem.currentdisc", "0");
            guiProperties.Add("#Emulators2.CurrentItem.totaldiscs", "0");

            guiProperties.Add("#Emulators2.CurrentItem.path", "");
            guiProperties.Add("#Emulators2.CurrentItem.selectedgoodmerge", "");

            guiProperties.Add("#Emulators2.CurrentItem.Profile.title", "");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.emulatorpath", "");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.arguments", "");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.workingdirectory", "");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.suspendmp", "False");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.usequotes", "False");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.mountimages", "False");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.escapetoexit", "False");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.checkcontroller", "False");
            guiProperties.Add("#Emulators2.CurrentItem.Profile.launchedexe", "");

            return guiProperties;
        }

        public static void ClearGUIProperties()
        {
            foreach (KeyValuePair<string, string> prop in getProps())
                GUIPropertyManager.SetProperty(prop.Key, prop.Value);
        }

        public void SetGUIProperties()
        {
            Dictionary<string, string> guiProperties = getProps();
            Emulator emu = associatedEmulator;
            Game game = associatedGame;
            
            if (isFavourites || IsGroup)
            {
                guiProperties["#Emulators2.CurrentItem.title"] = Label;
            }
            else if (emu != null)
            {
                guiProperties["#Emulators2.CurrentItem.isemulator"] = "yes";
                guiProperties["#Emulators2.CurrentItem.isgame"] = "no";

                guiProperties["#Emulators2.CurrentItem.title"] = emu.Title;
                guiProperties["#Emulators2.CurrentItem.grade"] = emu.Grade.ToString();
                guiProperties["#Emulators2.CurrentItem.description"] = emu.Description;
                guiProperties["#Emulators2.CurrentItem.company"] = emu.Company;
                guiProperties["#Emulators2.CurrentItem.year"] = emu.Year > 0 ? emu.Year.ToString() : "";
                guiProperties["#Emulators2.CurrentItem.caseaspect"] = emu.CaseAspect.ToString(System.Globalization.CultureInfo.InvariantCulture);
                guiProperties["#Emulators2.CurrentItem.isarcade"] = emu.IsArcade.ToString();
            }
            else if (game != null)
            {
                guiProperties["#Emulators2.CurrentItem.isemulator"] = "no";
                guiProperties["#Emulators2.CurrentItem.isgame"] = "yes";
                guiProperties["#Emulators2.CurrentItem.goodmerge"] = game.IsGoodmerge ? "yes" : "no";
                guiProperties["#Emulators2.CurrentItem.isgoodmerge"] = game.IsGoodmerge.ToString();
                guiProperties["#Emulators2.CurrentItem.title"] = game.Title;

                guiProperties["#Emulators2.CurrentItem.coverflowlabel"] = string.IsNullOrEmpty(Label2) ? game.ParentEmulator.ToString() : Label2;
                guiProperties["#Emulators2.CurrentItem.emulatortitle"] = game.ParentEmulator.Title;
                guiProperties["#Emulators2.CurrentItem.grade"] = game.Grade.ToString();
                guiProperties["#Emulators2.CurrentItem.description"] = game.Description;
                guiProperties["#Emulators2.CurrentItem.year"] = game.Yearmade > 0 ? game.Yearmade.ToString() : "";
                guiProperties["#Emulators2.CurrentItem.genre"] = game.Genre;
                guiProperties["#Emulators2.CurrentItem.company"] = game.Company;
                guiProperties["#Emulators2.CurrentItem.caseaspect"] = game.ParentEmulator.CaseAspect.ToString(System.Globalization.CultureInfo.InvariantCulture);
                guiProperties["#Emulators2.CurrentItem.isarcade"] = game.ParentEmulator.IsArcade.ToString();

                if (game.Latestplay.CompareTo(DateTime.MinValue) == 0)
                {
                    guiProperties["#Emulators2.CurrentItem.latestplaydate"] = Translator.Instance.never;
                    guiProperties["#Emulators2.CurrentItem.latestplaytime"] = "";
                }
                else
                {
                    guiProperties["#Emulators2.CurrentItem.latestplaydate"] = game.Latestplay.ToShortDateString();
                    guiProperties["#Emulators2.CurrentItem.latestplaytime"] = game.Latestplay.ToShortTimeString();
                }

                guiProperties["#Emulators2.CurrentItem.playcount"] = game.Playcount.ToString();
                guiProperties["#Emulators2.CurrentItem.favourite"] = game.Favourite ? "yes" : "no";
                guiProperties["#Emulators2.CurrentItem.isfavourite"] = game.Favourite.ToString();

                if (game.CurrentDiscNum < 2)
                    guiProperties["#Emulators2.CurrentItem.currentdisc"] = "1";
                else
                    guiProperties["#Emulators2.CurrentItem.currentdisc"] = game.CurrentDiscNum.ToString();
                guiProperties["#Emulators2.CurrentItem.totaldiscs"] = game.Discs.Count.ToString();

                guiProperties["#Emulators2.CurrentItem.path"] = game.Path;
                guiProperties["#Emulators2.CurrentItem.selectedgoodmerge"] = game.CurrentDisc.LaunchFile;
                guiProperties["#Emulators2.CurrentItem.Profile.title"] = game.CurrentProfile.Title;
                guiProperties["#Emulators2.CurrentItem.Profile.emulatorpath"] = game.CurrentProfile.EmulatorPath;
                guiProperties["#Emulators2.CurrentItem.Profile.arguments"] = game.CurrentProfile.Arguments;
                guiProperties["#Emulators2.CurrentItem.Profile.workingdirectory"] = game.CurrentProfile.WorkingDirectory;
                guiProperties["#Emulators2.CurrentItem.Profile.suspendmp"] = (game.CurrentProfile.SuspendMP == true).ToString();
                guiProperties["#Emulators2.CurrentItem.Profile.usequotes"] = (game.CurrentProfile.UseQuotes == true).ToString();
                guiProperties["#Emulators2.CurrentItem.Profile.mountimages"] = game.CurrentProfile.MountImages.ToString();
                guiProperties["#Emulators2.CurrentItem.Profile.escapetoexit"] = game.CurrentProfile.EscapeToExit.ToString();
                guiProperties["#Emulators2.CurrentItem.Profile.checkcontroller"] = game.CurrentProfile.CheckController.ToString();
                guiProperties["#Emulators2.CurrentItem.Profile.launchedexe"] = game.CurrentProfile.LaunchedExe;
            }

            foreach (KeyValuePair<string, string> prop in guiProperties)
                GUIPropertyManager.SetProperty(prop.Key, prop.Value);
        }

        internal void UpdateGameInfo(Game game)
        {
            lock (syncRoot)
            {
                Label = game.Title;
                AssociatedGame = game;
                thumbGroup = null;
                ThumbnailImage = ThumbGroup.FrontCoverDefaultPath;
            }
        }

        public int ListPosition { get; set; }
    }

    
}
