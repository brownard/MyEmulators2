using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace MyEmulators2
{
    /// <summary>
    /// Holds all the Thumb info for an Emulator or Game and provides methods
    /// for updating/viewing or deleting
    /// </summary>
    public class ThumbGroup : IDisposable
    {
        public const string THUMB_DIR_NAME = "Emulators 2";
        public const string EMULATOR_DIR_NAME = "Emulators";
        public const string GAME_DIR_NAME = "Games";

        public const string LOGO_NAME = "Logo";
        public const string BOX_FRONT_NAME = "BoxFront";
        public const string BOX_BACK_NAME = "BoxBack";
        public const string TITLESCREEN_NAME = "TitleScreenshot";
        public const string INGAME_NAME = "IngameScreenshot";
        public const string FANART_NAME = "Fanart";
        public const string MANUAL_NAME = "Manual";

        ImageCodecInfo jpegCodec = null;
        EncoderParameters encoderParams = null;
        double thumbaspect = 0;
                
        /// <summary>
        /// Initialises a new ThumbGroup with the thumbs of the specified parent
        /// </summary>
        /// <param name="parent">An Emulator or Game</param>
        public ThumbGroup(DBItem parent)
        {
            //init Thumbs
            frontCover = new Thumb(ThumbType.FrontCover);
            backCover = new Thumb(ThumbType.BackCover);
            titleScreen = new Thumb(ThumbType.TitleScreen);
            inGame = new Thumb(ThumbType.InGameScreen);
            fanart = new Thumb(ThumbType.Fanart);

            //set parent info and thumbaspect
            updateParent(parent);

            //load the paths/images
            loadThumbs();
        }

        #region Properties
        
        //int id;
        /// <summary>
        /// The ID of the DB item
        /// </summary>
        public int ID
        {
            get 
            {
                if (parentItemIsGame)
                    return ((Game)parentItem).GameID;
                else
                    return ((Emulator)parentItem).UID;
            }
        }
        
        public string ParentTitle
        {
            get
            {
                if (parentItemIsGame)
                    return ((Game)parentItem).Title;
                else
                    return ((Emulator)parentItem).Title;
            }

        }
        
        DBItem parentItem = null;

        /// <summary>
        /// True: ParentItem is Game
        /// False: ParentItem is Emulator
        /// </summary>
        bool parentItemIsGame = false;

        /// <summary>
        /// The location where the thumbs will be saved
        /// </summary>
        public string ThumbPath
        {
            get
            {
                string thumbFolder = EMULATOR_DIR_NAME;
                if (parentItemIsGame)
                    thumbFolder = GAME_DIR_NAME;

                return string.Format(@"{0}\{1}\{2}\{3}\", Emulators2Settings.Instance.ThumbDirectory, THUMB_DIR_NAME, thumbFolder, ID);
            }
        }

        Thumb frontCover = null;
        public Thumb FrontCover
        {
            get { return frontCover; }
        }

        Thumb backCover = null;
        public Thumb BackCover
        {
            get { return backCover; }
        }

        Thumb titleScreen = null;
        public Thumb TitleScreen
        {
            get { return titleScreen; }
        }

        Thumb inGame = null;
        public Thumb InGame
        {
            get { return inGame; }
        }

        Thumb fanart = null;
        public Thumb Fanart
        {
            get { return fanart; }
        }

        string manualPath = null;
        public string ManualPath
        {
            get 
            {
                if (manualPath == null) //we haven't tried loading existing manual yet
                {
                    string lPath = ThumbPath + MANUAL_NAME + ".pdf";
                    if (System.IO.File.Exists(lPath))
                        manualPath = lPath;
                    else
                        manualPath = "";
                }

                return manualPath; 
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    value = "";
                else if (!value.ToLower().EndsWith(".pdf"))
                {
                    Logger.LogError("Unable to update {0} Manual, file must be a pdf.");
                    return;
                }
                manualPath = value; 
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the Thumb Directory in Windows Explorer, 
        /// highlighting the selected thumb if it exists
        /// </summary>
        /// <param name="thumbType">The thumb to select</param>
        public void BrowseThumbs(ThumbType thumbType)
        {
            if (ID < -1)
            {
                Logger.LogDebug("Unable to browse thumb directory, the emulator is not in the database");
                return;
            }
            string file = "";

            //get file name
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    if (parentItemIsGame)
                        file = BOX_FRONT_NAME;
                    else
                        file = LOGO_NAME;
                    break;
                case ThumbType.BackCover:
                    file = BOX_BACK_NAME;
                    break;
                case ThumbType.TitleScreen:
                    file = TITLESCREEN_NAME;
                    break;
                case ThumbType.InGameScreen:
                    file = INGAME_NAME;
                    break;
                case ThumbType.Fanart:
                    file = FANART_NAME;
                    break;
            }
            
            string args = "";
            string path = ThumbPath;

            if (System.IO.File.Exists(path + file + ".jpg"))
            {
                //selected thumb exists, set arguments to highlight file
                args = "/select," + path + file + ".jpg";
            }
            else if (System.IO.File.Exists(path + file + ".png"))
            {
                //selected thumb exists, set arguments to highlight file
                args = "/select," + path + file + ".png";
            }
            else
            {
                //selected thumb doesn't exist, 
                //check if directory exists and create if necessary
                if (!System.IO.Directory.Exists(ThumbPath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(ThumbPath);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Error creating thumb directory for {0} - {1}", ParentTitle, ex.Message);
                        return;
                    }
                }
                //set args to just open directory
                args = ThumbPath;
            }

            // launch Explorer with selected args
            using (Process proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = "explorer.exe";
                proc.StartInfo.Arguments = args;
                proc.Start();
            }
        }

        /// <summary>
        /// Returns the path to the specified thumb
        /// </summary>
        /// <param name="thumbType"></param>
        /// <returns></returns>
        public string GetThumbPath(ThumbType thumbType)
        {
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    return frontCover.Path;
                case ThumbType.BackCover:
                    return backCover.Path;
                case ThumbType.TitleScreen:
                    return titleScreen.Path;
                case ThumbType.InGameScreen:
                    return inGame.Path;
                case ThumbType.Fanart:
                    return fanart.Path;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the Image for the specified thumb
        /// </summary>
        /// <param name="thumbType"></param>
        /// <returns></returns>
        public Image GetThumb(ThumbType thumbType)
        {
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    return frontCover.Image;
                case ThumbType.BackCover:
                    return backCover.Image;
                case ThumbType.TitleScreen:
                    return titleScreen.Image;
                case ThumbType.InGameScreen:
                    return inGame.Image;
                case ThumbType.Fanart:
                    return fanart.Image;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Updates the specified thumb with the specified path
        /// </summary>
        /// <param name="thumbType"></param>
        /// <param name="thumbPath"></param>
        public void UpdateThumb(ThumbType thumbType, string thumbPath)
        {
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    frontCover.Path = thumbPath;
                    break;
                case ThumbType.BackCover:
                    backCover.Path = thumbPath;
                    break;
                case ThumbType.TitleScreen:
                    titleScreen.Path = thumbPath;
                    break;
                case ThumbType.InGameScreen:
                    inGame.Path = thumbPath;
                    break;
                case ThumbType.Fanart:
                    fanart.Path = thumbPath;
                    break;
            }
        }

        /// <summary>
        /// Updates the specified thumb with the specified image
        /// </summary>
        /// <param name="thumbType"></param>
        /// <param name="thumb"></param>
        public void UpdateThumb(ThumbType thumbType, Image thumb)
        {
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    frontCover.Image = thumb;
                    break;
                case ThumbType.BackCover:
                    backCover.Image = thumb;
                    break;
                case ThumbType.TitleScreen:
                    titleScreen.Image = thumb;
                    break;
                case ThumbType.InGameScreen:
                    inGame.Image = thumb;
                    break;
                case ThumbType.Fanart:
                    fanart.Image = thumb;
                    break;
            }
        }

        /// <summary>
        /// Saves all configured thumbs to thumb directory
        /// </summary>
        public void SaveAllThumbs()
        {
            if (ID < -1)
            {
                return;
            }

            SaveThumb(ThumbType.FrontCover);
            SaveThumb(ThumbType.BackCover);
            SaveThumb(ThumbType.TitleScreen);
            SaveThumb(ThumbType.InGameScreen);
            SaveThumb(ThumbType.Fanart);
        }

        /// <summary>
        /// Save specified thumb
        /// </summary>
        public void SaveThumb(ThumbType thumbType)
        {
            if (ID < -1)
            {
                //new Emulator, no save directory
                return;
            }

            string friendlyName = null;
            string fileName = null;
            Image thumb = null;

            bool resize = false;

            string savePath = ThumbPath;

            //check and create thumb directory
            if (!System.IO.Directory.Exists(savePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(savePath);
                }
                catch(Exception ex)
                {
                    Logger.LogError("Error creating thumb directory for {0} - {1}", ParentTitle, ex.Message);
                    return;
                }
            }

            //get file/friendly name and set resize option
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    if (parentItemIsGame)
                    {
                        friendlyName = "Front Cover";
                        savePath += BOX_FRONT_NAME;
                        resize = Options.Instance.GetBoolOption("resizethumbs");
                    }
                    else
                    {
                        //Emulator so set front cover to Logo
                        friendlyName = "Logo";
                        savePath += LOGO_NAME;
                    }

                    fileName = frontCover.Path;
                    thumb = frontCover.Image;
                    break;
                case ThumbType.BackCover:
                    friendlyName = "Back Cover";
                    savePath += BOX_BACK_NAME;
                    resize = Options.Instance.GetBoolOption("resizethumbs");

                    fileName = backCover.Path;
                    thumb = backCover.Image;
                    break;
                case ThumbType.TitleScreen:
                    friendlyName = "Title Screen";
                    savePath += TITLESCREEN_NAME;

                    fileName = titleScreen.Path;
                    thumb = titleScreen.Image;
                    break;
                case ThumbType.InGameScreen:
                    friendlyName = "In Game Screen";
                    savePath += INGAME_NAME;

                    fileName = inGame.Path;
                    thumb = inGame.Image;
                    break;
                case ThumbType.Fanart:
                    friendlyName = "Fanart";
                    savePath += FANART_NAME;

                    fileName = fanart.Path;
                    thumb = fanart.Image;
                    break;
            }

            if (thumb == null)
            {
                //only delete if filename is also empty, else there was a problem
                //loading the image but path may still be valid
                if (fileName == "")
                {
                    //Delete thumb
                    try
                    {
                        System.IO.File.Delete(savePath + ".jpg");
                    }
                    catch { }
                    try
                    {
                        System.IO.File.Delete(savePath + ".png");
                    }
                    catch { }
                }
                else
                    Logger.LogError("Unable to save {0} for {1} - error loading path '{2}'", friendlyName, ParentTitle, fileName);
                return;
            }

            bool shrinkThumb = false;
            int maxThumbDimension = 0;
            if (thumbType != ThumbType.Fanart)
            {
                maxThumbDimension = Options.Instance.GetIntOption("maxthumbdimension");
                if (maxThumbDimension < 0)
                    maxThumbDimension = 0;

                shrinkThumb = maxThumbDimension > 0 && (thumb.Width > maxThumbDimension || thumb.Height > maxThumbDimension);
            }


            string ext = ".png";

            //if we are not resizing and we have a reference to an image in a valid format
            //on the local file system, copy the file to the thumb location
            if (!resize && !shrinkThumb && isValidThumbPath(fileName, ref ext))
            {
                if (fileName != savePath + ext) //check that image actually needs updating
                {
                    try
                    {
                        System.IO.File.Copy(fileName, savePath + ext, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Error copying new {0} for {1} - {2}", friendlyName, ParentTitle, ex.Message);
                        return;
                    }
                }
            }
            else
            {
                //If first save set image encoder parameters
                if (jpegCodec == null)
                    initImageEncoder();
                try
                {
                    if (resize || shrinkThumb)
                        thumb = ImageHandler.ResizeImage(thumb, resize ? thumbaspect : 0, shrinkThumb ? maxThumbDimension : 0);
                    //save image to thumb location
                    thumb.Save(savePath + ext, jpegCodec, encoderParams);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error saving new {0} for {1} - {2}", friendlyName, ParentTitle, ex.Message);
                    return;
                }
            }

            //if image is jpg, remove any png's and vice versa
            RemoveAlternateThumb(savePath + ext); 

            //update thumb path to save location
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    frontCover.Path = savePath + ext;
                    break;
                case ThumbType.BackCover:
                    backCover.Path = savePath + ext;
                    break;
                case ThumbType.TitleScreen:
                    titleScreen.Path = savePath + ext;
                    break;
                case ThumbType.InGameScreen:
                    inGame.Path = savePath + ext;
                    break;
                case ThumbType.Fanart:
                    fanart.Path = savePath + ext;
                    break;
            }
        }

        public void SaveManual()
        {
            string savePath = ThumbPath + MANUAL_NAME + ".pdf"; //destination dir

            string lPath = ManualPath; //initialise property and get configured manual path
            if (lPath == savePath) //configured manual is already in destination dir
                return;

            if (lPath == "") //delete manual
            {
                try { System.IO.File.Delete(savePath); }
                catch { }

                return;
            }

            //if configured path exists and is a pdf, copy to destination dir
            if (lPath.ToLower().EndsWith(".pdf") && System.IO.File.Exists(lPath))
            {
                try
                {
                    System.IO.File.Copy(lPath, savePath, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error copying new Manual for {1} - {2}", ParentTitle, ex.Message);
                    return;
                }
            }

        }

        /// <summary>
        /// Opens the specified thumb in the default image viewer
        /// </summary>
        /// <param name="thumbType"></param>
        public void LaunchThumb(ThumbType thumbType)
        {
            Thumb thumb = null;
            //get specified thumb
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    thumb = frontCover;
                    break;
                case ThumbType.BackCover:
                    thumb = backCover;
                    break;
                case ThumbType.TitleScreen:
                    thumb = titleScreen;
                    break;
                case ThumbType.InGameScreen:
                    thumb = inGame;
                    break;
                case ThumbType.Fanart:
                    thumb = fanart;
                    break;
            }

            if (thumb == null)
                return;

            string thumbPath = "";

            //path is in local file system use that
            if (thumb.Path != "" && !thumb.Path.ToLower().StartsWith("http://"))
                thumbPath = thumb.Path;
            //else if we have an image, save a temp image and use temp path
            else if (thumb.Image != null)
                thumbPath = getTempThumbPath(thumb.Image);
            else
                return; //no path or image

            if (thumbPath == "")
            {
                return;
            }

            //open selected path
            using (Process proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo(thumbPath);
                proc.Start();
            }
        }
        
        #endregion

        #region Private Methods

        //initialise image encoder settings, called on first image save
        void initImageEncoder()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();            
            for (int x = 0; x < codecs.Length; x++)
            {
                if (codecs[x].MimeType == "image/png")
                    jpegCodec = codecs[x];
            }
            if (jpegCodec == null)
            {
                Logger.LogError("Error saving images - Unable to locate the PNG codec");
            }
            encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
        }

        //check if parent is an Emulator or Game and update thumbaspect
        void updateParent(DBItem parent)
        {
            if (parent is Game)
                parentItemIsGame = true;
            else if (!(parent is Emulator))
                throw new ArgumentException("Parent must be an Emulator or Game", "parent");

            parentItem = parent;
            if (parentItemIsGame)
            {
                //parent is a Game
                Game game = parent as Game;
                if (game.ParentEmulator != null)
                    thumbaspect = game.ParentEmulator.CaseAspect;
                else
                    thumbaspect = 0;
                frontCover.ThumbAspect = thumbaspect;
                backCover.ThumbAspect = thumbaspect;
            }
            else //Emulator
            {
                thumbaspect = 0;
                frontCover.ThumbAspect = 0;
                backCover.ThumbAspect = 0;
            }
        }

        //loads all configured thumbs for the specified parent
        void loadThumbs()
        {
            if (ID < -1)
                return; //New emulator, there won't be any thumbs

            frontCover.Path = getThumbPath(ThumbType.FrontCover);
            fanart.Path = getThumbPath(ThumbType.Fanart);

            if (parentItemIsGame)
            {
                backCover.Path = getThumbPath(ThumbType.BackCover);
                titleScreen.Path = getThumbPath(ThumbType.TitleScreen);
                inGame.Path = getThumbPath(ThumbType.InGameScreen);
            }
        }

        /// <summary>
        /// Checks if the specified image is in the local file system
        /// and is a supported image type.
        /// </summary>
        /// <param name="path">The full path to check</param>
        /// <param name="ext">If true this will be updated with the image extension</param>
        /// <returns></returns>
        static bool isValidThumbPath(string path, ref string ext)
        {
            if (path.ToLower().StartsWith("http://") ||
                !System.IO.File.Exists(path)
                )
                return false;

            if (path.ToLower().EndsWith(".jpg"))
            {
                ext = ".jpg";
                return true;
            }
            if (path.ToLower().EndsWith(".png"))
            {
                ext = ".png";
                return true;
            }

            return false;
        }

        string getThumbPath(ThumbType thumbType)
        {
            string path = ThumbPath;

            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    if (parentItemIsGame)
                        path += BOX_FRONT_NAME;
                    else
                        path += LOGO_NAME;
                    break;
                case ThumbType.BackCover:
                    path += BOX_BACK_NAME;
                    break;
                case ThumbType.TitleScreen:
                    path += TITLESCREEN_NAME;
                    break;
                case ThumbType.InGameScreen:
                    path += INGAME_NAME;
                    break;
                case ThumbType.Fanart:
                    path += FANART_NAME;
                    break;
            }

            if (System.IO.File.Exists(path + ".jpg"))
                return path + ".jpg";
            
            if (System.IO.File.Exists(path + ".png"))
                return path + ".png";

            return "";
        }

        //save the image to temp directory and return temp image path
        static string getTempThumbPath(Image thumb)
        {
            if (thumb == null)
                return "";

            //create unique temp save path
            string savePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "myEmulators2." + Guid.NewGuid().ToString() + ".bmp");
            try
            {
                thumb.Save(savePath, ImageFormat.Bmp);
            }
            catch(Exception ex)
            {
                Logger.LogError("Error saving temp image - {0}", ex.Message);
                return "";
            }
            return savePath;
        }

        //if savePath is jpg, remove png and vica versa
        public static void RemoveAlternateThumb(string savePath)
        {            
            if(savePath == null || savePath.Length <= 4) //invalid path
                return;

            //get path without extension
            string pathWithoutExtension = savePath.Substring(0, savePath.Length - 4);
            //get extension
            string extension = savePath.Substring(savePath.Length - 4);

            //get alternate path
            string delPath = null;
            if (extension.ToLower() == ".jpg")
            {
                delPath = pathWithoutExtension + ".png";
            }
            else if (extension.ToLower() == ".png")
            {
                delPath = pathWithoutExtension + ".jpg";
            }

            if (delPath == null) //image is neither jpg or png
                return;

            try
            {
                System.IO.File.Delete(delPath);
            }
            catch { }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (frontCover != null)
                frontCover.Dispose();
            if (backCover != null)
                backCover.Dispose();
            if (titleScreen != null)
                titleScreen.Dispose();
            if (inGame != null)
                inGame.Dispose();
            if (fanart != null)
                fanart.Dispose();

            if (parentThumbs != null)
            {
                parentThumbs.Dispose();
                parentThumbs = null;
            }
        }

        #endregion

        ThumbGroup parentThumbs = null;

        public string FrontCoverDefaultPath
        {
            get
            {
                string path = "";
                //check if we have a local reference
                if (!frontCover.Path.ToLower().StartsWith("http://"))
                    path = frontCover.Path;
                //if not, load parent emulator ThumbGroup
                if (string.IsNullOrEmpty(path))
                {
                    if (!parentItemIsGame)
                    {
                        //Parent is an Emulator, load skin default
                        return Emulators2Settings.Instance.DefaultLogo;
                    }

                    using (parentThumbs = new ThumbGroup(((Game)parentItem).ParentEmulator)) //ensure latest emu settings are loaded
                        path = parentThumbs.FrontCoverDefaultPath;
                }

                return path;
            }
        }
        /// <summary>
        /// Returns the fanart path. If Parent is
        /// a Game and no fanart is configured this will
        /// return the parent Emulator fanart path.
        /// </summary>
        public string FanartDefaultPath 
        {
            get
            {
                string path = "";
                //check if we have a local reference
                if (!fanart.Path.ToLower().StartsWith("http://"))
                    path = fanart.Path;
                //if not, load parent emulator ThumbGroup
                if (string.IsNullOrEmpty(path))
                {
                    if (!parentItemIsGame)
                    {
                        //Parent is an Emulator, return default
                        return Emulators2Settings.Instance.DefaultFanart;
                    }
                    using (parentThumbs = new ThumbGroup(((Game)parentItem).ParentEmulator)) //ensure latest emu settings are loaded
                        path = parentThumbs.FanartDefaultPath;
                }

                return path;
            }
        }

        /// <summary>
        /// Returns the fanart image. If Parent is
        /// a Game and no fanart is configured this will
        /// return the parent Emulator fanart image.
        /// </summary>
        public Image FanartDefault
        {
            get
            {
                if (!parentItemIsGame || fanart.Image != null)
                {
                    return fanart.Image; //emulator or we have an image
                }

                //if not, load parent emulator ThumbGroup and return parent fanart image
                if (parentThumbs != null) //ensure latest emu settings are loaded
                    parentThumbs.Dispose();

                parentThumbs = new ThumbGroup(((Game)parentItem).ParentEmulator);

                return parentThumbs.fanart.Image;
            }
        }

    }

}
