using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyEmulators2
{
    /// <summary>
    /// The underlying thumbs used by ThumbGroup. Setting Path will dispose of Image
    /// and reload it (when next referenced) using the new path. Setting Image will set
    /// Path to "" (we don't know the path). Setting Path to "" or either Path or Image to null
    /// will dispose and remove all references to the Image. 
    /// </summary>
    public class Thumb : IDisposable
    {
        string friendlyName = "";

        public Thumb(ThumbType thumbType)
        {
            switch (thumbType)
            {
                case ThumbType.FrontCover:
                    friendlyName = "Front Cover";
                    break;
                case ThumbType.BackCover:
                    friendlyName = "Back Cover";
                    break;
                case ThumbType.TitleScreen:
                    friendlyName = "Title Screen";
                    break;
                case ThumbType.InGameScreen:
                    friendlyName = "Ingame Screen";
                    break;
                case ThumbType.Fanart:
                    friendlyName = "Fanart";
                    break;
            }
        }

        string path = "";
        public string Path
        {
            get { return path; }
            set
            {
                clearThumb();
                path = value == null ? "" : value;
            }
        }

        Image image = null;
        public Image Image
        {
            get
            {
                if (image == null && !string.IsNullOrEmpty(path))
                {
                    loadImage();
                }
                return image;
            }
            set
            {
                clearThumb(); //dispose of old image

                path = ""; //remove reference to old image

                if (value == null)
                    return;

                try
                {
                    image = ImageHandler.NewImage(value);
                }
                catch (Exception ex)
                {
                    Logger.LogError("ThumbGroup - Error loading {0} - {1}", friendlyName, ex.Message);
                    clearThumb();
                }
            }
        }

        double thumbaspect = 0;
        public double ThumbAspect
        {
            get { return thumbaspect; }
            set { thumbaspect = value; }
        }

        void loadImage()
        {
            try
            {
                //attempt to load specified file
                if (path.ToLower().StartsWith("http://"))
                {
                    using (Image newImage = ImageHandler.BitmapFromWeb(path))
                    {
                        //set thumb to new image
                        image = ImageHandler.NewImage(newImage);
                    }
                }
                else
                {
                    using (Image newImage = Image.FromFile(path))
                    {
                        //set thumb to new image
                        image = ImageHandler.NewImage(newImage);
                    }
                }
            }
            catch(Exception ex)
            {
                //reset fields on error
                Logger.LogError("ThumbGroup - Error loading {0} - {1}", friendlyName, ex.Message);
                clearThumb();
            }
        }

        void clearThumb()
        {
            if (image != null)
            {
                try
                {
                    image.Dispose();
                }
                catch { }
                image = null;
            }
        }

        /// <summary>
        /// Releases underlying image resources,
        /// if a path is specified, it will be kept
        /// and the image will be reloaded if required
        /// </summary>
        public void Dispose()
        {
            clearThumb();
        }
    }
}
