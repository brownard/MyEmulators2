
using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using System.IO;

namespace myEmulators
{
    class ThumbsHandler
    {
        //Does all the logic behind which thumbnails to show

        public ThumbsHandler()
        {
            string thumbsFolder = getThumbsFolder();
            thumb_main = thumbsFolder + @"\myEmulators";
            thumb_emulators = thumbsFolder + @"\myEmulators\emulators";
            thumb_previews = thumbsFolder + @"\myEmulators\previews";
            thumb_games = thumbsFolder + @"\myEmulators\games";
        }

        #region Singleton
        protected static ThumbsHandler instance = null;
        static object padlock = new object();
        public static ThumbsHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                            instance = new ThumbsHandler();
                    }
                }
                return instance;
            }
        }
        #endregion

        //Static options, initiated in constructor
        public String thumb_main = "";
        public String thumb_emulators = "";
        public String thumb_previews = "";
        public String thumb_games = "";

        //Folder location
        private String getThumbsFolder()
        {
            string location = Options.Instance.GetStringOption("thumblocation");
            if (location == "")
                location = Config.GetFolder(Config.Dir.Thumbs);
            if (!Directory.Exists(location))
            {
                Logger.LogError("Unable to locate thumb folder '{0}', reverting to default thumb location", location);
                location = Config.GetFolder(Config.Dir.Thumbs); //default to MP thumb directory
                Options.Instance.UpdateOption("thumblocation", location);
            }
            if (location.EndsWith("\\"))
                location = location.Substring(0, location.Length - 1); //remove any trailing '\'

            return location;
        }

        //Create folders
        public void initFolders()
        {
            if (!Directory.Exists(thumb_main))
            {
                Directory.CreateDirectory(thumb_main);
            }
            if (!Directory.Exists(thumb_emulators))
            {
                Directory.CreateDirectory(thumb_emulators);
            }
            if (!Directory.Exists(thumb_games))
            {
                Directory.CreateDirectory(thumb_games);
            }
        }

        public String createEmulatorArt(Emulator item, String ArtType)
        {
            string ArtPath = "";

            if (ArtType.ToLower() == "manual")
            {
                ArtPath = setManual(thumb_emulators + @"\" + item.Title + @"\" + @"\" + ArtType);
            }
            else
            {
                ArtPath = setArt(thumb_emulators + @"\" + item.Title + @"\" + ArtType);
            }

            return ArtPath;
        }

        public String createGameArt(Game item, String ArtType, bool GetDefault)
        {
            string ArtPath = "";

            if (ArtType.ToLower() == "manual")
            {
                ArtPath = setManual(thumb_games + @"\" + item.ParentEmulator.Title + @"\" + item.GameID.ToString() + @"\" + ArtType);
            }
            else
            {
                ArtPath = setArt(thumb_games + @"\" + item.ParentEmulator.Title + @"\" + item.GameID.ToString() + @"\" + ArtType);

                if (ArtPath.Trim() == "" && GetDefault)
                {
                    ArtPath = setArt(thumb_emulators + @"\" + item.ParentEmulator.Title + @"\" + ArtType);
                }
            }
            
            return ArtPath;
        }

        public ExtendedGUIListItem createEmulatorFacadeItem(Emulator item)
        {
            ExtendedGUIListItem listItem = new ExtendedGUIListItem(item.Title);
            listItem.AssociatedEmulator = item;
            setThumbnail(listItem, thumb_emulators + @"\" + item.Title + @"\Logo");
            return listItem;
        }
        
        public ExtendedGUIListItem createGameFacadeItem(Game item)
        {
            ExtendedGUIListItem listItem = new ExtendedGUIListItem(item.Title);
            listItem.AssociatedGame = item;

            setThumbnail(listItem, thumb_games + @"\" + item.ParentEmulator.Title + @"\" + item.GameID.ToString() + @"\BoxFront");

            if (listItem.ThumbnailImage.Trim() == "")
            {
                String artfile = createGameArt(item, "TitleScreenshot", true);

                if (artfile != "")
                {
                    listItem.ThumbnailImage = artfile;
                }
                else
                {
                    setThumbnail(listItem, thumb_emulators + @"\" + item.ParentEmulator.Title + @"\Logo");
                }
            }

            return listItem;
        }

        public ExtendedGUIListItem createGameRomFacadeItem(Game item)
        {
            ExtendedGUIListItem listItem = new ExtendedGUIListItem(item.LaunchFile);
            listItem.AssociatedGame = item;
            setThumbnail(listItem, thumb_games + @"\" + item.ParentEmulator.Title + @"\" + item.GameID.ToString() + @"\BoxFront");

            if (listItem.ThumbnailImage.Trim() == "")
            {
                String artfile = createGameArt(item, "TitleScreenshot", true);

                if (artfile != "")
                {
                    listItem.ThumbnailImage = artfile;
                }
                else
                {
                    setThumbnail(listItem, thumb_emulators + @"\" + item.ParentEmulator.Title + @"\Logo");
                }
            }

            return listItem;
        }

        private void setThumbnail(ExtendedGUIListItem item, String thumbpath)
        {
            if (File.Exists(thumbpath + ".png"))
            {
                item.ThumbnailImage = thumbpath + ".png";
            }
            else if (File.Exists(thumbpath + ".jpg"))
            {
                item.ThumbnailImage = thumbpath + ".jpg";
            }
            else if (File.Exists(thumbpath + ".gif"))
            {
                item.ThumbnailImage = thumbpath + ".gif";
            }
            else
                item.ThumbnailImage = GUIGraphicsContext.Skin + "\\media\\defaultMyEmulators.png";
        }

        private String setArt(String thumbpath)
        {
            string ArtPath = "";

            if (File.Exists(thumbpath + ".png"))
            {
                ArtPath = thumbpath + ".png";
            }
            else if (File.Exists(thumbpath + ".jpg"))
            {
                ArtPath = thumbpath + ".jpg";
            }
            else if (File.Exists(thumbpath + ".gif"))
            {
                ArtPath = thumbpath + ".gif";
            }

            return ArtPath;
        }

        private String setManual(String manualpath)
        {
            string ManualPath = "";

            if (File.Exists(manualpath + ".pdf"))
            {
                ManualPath = manualpath + ".pdf";
            }

            return ManualPath;
        }

        public void setFilmstripImage(GUIFacadeControl facade)
        {
#if MP11
            facade.FilmstripView.InfoImageFileName = facade.SelectedListItem.ThumbnailImage;
#else
            facade.FilmstripLayout.InfoImageFileName = facade.SelectedListItem.ThumbnailImage;
#endif
        }

        public ExtendedGUIListItem createBackDots(string PrevSelected)
        {
            ExtendedGUIListItem dots = new ExtendedGUIListItem("..");
            dots.IsBackDots = true;
            dots.PrevSelected = PrevSelected;
            dots.ThumbnailImage = GUIGraphicsContext.Skin + @"\Media\DefaultFolderBackBig.png";
            dots.IconImage = GUIGraphicsContext.Skin + @"\Media\defaultFolderBack.png";
            return dots;
        }

        public ExtendedGUIListItem createSubDir(String path)
        {
            ExtendedGUIListItem dir = new ExtendedGUIListItem(path.Substring(path.LastIndexOf("\\") + 1));
            dir.AssociatedDirectory = path;
            dir.ThumbnailImage = GUIGraphicsContext.Skin + @"\Media\DefaultFolderBig.png";
            setThumbnail(dir, path + @"\folder");
            dir.IconImage = GUIGraphicsContext.Skin + @"\Media\DefaultFolder.png";
            return dir;
        }

        public bool NeedThumbUpdate = false;

        public void updateThumbs(Emulator emu, string oldTitle)
        {
            if (emu.Title == oldTitle || !NeedThumbUpdate)
                return;
            bool updateArt = false;
            DirectoryInfo source = new DirectoryInfo(thumb_games + "\\" + oldTitle);
            FileInfo[] filesToCopy = null;
            if (source.Exists)
            {
                filesToCopy = source.GetFiles("*.*", SearchOption.AllDirectories);
                if (filesToCopy.Length > 0)
                    updateArt = true;
            }
            if (updateArt)
            {
                DirectoryInfo dest = new DirectoryInfo(thumb_games + "\\" + emu.Title);
                if (!dest.Exists)
                {
                    dest.Create();
                }
                foreach (FileInfo file in filesToCopy)
                {
                    FileInfo newFile = new FileInfo(Path.Combine(dest.FullName, file.FullName.Substring(source.FullName.Length + 1)));
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();
                    try
                    {
                        file.CopyTo(newFile.FullName, true);
                    }
                    catch { }
                }

            }
        }

        public void removeUnusedThumbs()
        {
            List<Emulator> emus = new List<Emulator>(DB.Instance.GetEmulatorsAndPC());
            //emulator thumbs
            List<DirectoryInfo> thumbDirs = new List<DirectoryInfo>(new DirectoryInfo(thumb_emulators).GetDirectories());
            //emulator folder in game thumbs
            thumbDirs.AddRange(new DirectoryInfo(thumb_games).GetDirectories());

            foreach(DirectoryInfo dir in thumbDirs)
            {
                bool delete = true;
                //match each emulator title against folder 
                foreach (Emulator emu in emus)
                {
                    if (dir.Name == emu.Title)
                    {
                        delete = false;
                        //no need to keep checking
                        break;
                    }
                }
                if (delete)
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch { }
                }
            }
            emus = null;
            thumbDirs = null;

            DirectoryInfo[] emuDirs = new DirectoryInfo(thumb_games).GetDirectories();
            List<DirectoryInfo> gameDirs = new List<DirectoryInfo>();
            foreach (DirectoryInfo emuDir in emuDirs)
            {
                try
                {
                    gameDirs.AddRange(emuDir.GetDirectories());
                }
                catch { }
            }

            foreach (DirectoryInfo dir in gameDirs)
            {
                bool delete = true;
                foreach (Game game in DB.Instance.GetGames())
                {
                    if (dir.Name == game.GameID.ToString())
                    {
                        delete = false;
                        break;
                    }
                }
                if (delete)
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch { }
                }
            }
        }


    }
}
