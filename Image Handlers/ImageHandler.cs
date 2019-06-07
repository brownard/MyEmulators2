using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using MediaPortal.Configuration;
using System.Net;

namespace MyEmulators2
{
    class ImageHandler
    {
        public static Image NewImage(Image input)
        {
            if (input == null)
                return null;

            Image output = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppPArgb);

            using (Graphics graphics = Graphics.FromImage(output))
                graphics.DrawImage(input, 0, 0, input.Width, input.Height);

            return output;
        }

        public static Image ResizeImage(Image input, double ratio, double maxThumbDimension = 0)
        {
            if (input == null)
                return null;

            bool resize = ratio > 0 && Options.Instance.GetBoolOption("resizethumbs");

            int newWidth = input.Width;
            int newHeight = input.Height;

            if (input.Width > input.Height)
            {
                if (resize)
                {
                    newWidth = input.Width;
                    newHeight = Convert.ToInt32(input.Width / ratio);
                }
                if (maxThumbDimension > 0 && newWidth > maxThumbDimension)
                {
                    double factor = maxThumbDimension / newWidth;
                    newWidth = Convert.ToInt32(maxThumbDimension);
                    newHeight = Convert.ToInt32(newHeight * factor);
                }
            }
            else
            {
                if (resize)
                {
                    newWidth = Convert.ToInt32(input.Height * ratio);
                    newHeight = input.Height;
                }
                if (maxThumbDimension > 0 && newHeight > maxThumbDimension)
                {
                    double factor = maxThumbDimension / newHeight;
                    newHeight = Convert.ToInt32(maxThumbDimension);
                    newWidth = Convert.ToInt32(newWidth * factor);
                }
            }

            return getNewImage(input, newWidth, newHeight);
        }

        public static Bitmap BitmapFromWeb(string url)
        {
            try
            {
                // create a web request to the url of the image
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                // set the method to GET to get the image
                myRequest.Method = "GET";
                myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:6.0.1) Gecko/20100101 Firefox/6.0.1";
                // get the response from the webpage
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                // create a bitmap from the stream of the response
                Bitmap bmp = new Bitmap(myResponse.GetResponseStream());
                // close off the stream and the response
                myResponse.Close();
                // return the Bitmap of the image
                return bmp;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error downloading thumb from {0} - {1}", url, ex.Message);
                return null; // if for some reason we couldn't get to image, we return null
            }
        }

        public static BitmapDownloadResult BeginBitmapFromWeb(string url)
        {
            BitmapDownloadResult result = new BitmapDownloadResult();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:6.0.1) Gecko/20100101 Firefox/6.0.1";
                request.BeginGetResponse((asyncRes) =>
                {
                    Bitmap bitmap = null;
                    try
                    {
                        using (HttpWebResponse response = request.EndGetResponse(asyncRes) as HttpWebResponse)
                        {
                            bitmap = new Bitmap(response.GetResponseStream());
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Error downloading thumb from {0} - {1}", url, ex.Message);
                    }
                    finally
                    {
                        result.Complete(bitmap);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error downloading thumb from {0} - {1}", url, ex.Message);
                return null;
            }
            return result;
        }

        public static bool CheckImageUrl(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:6.0.1) Gecko/20100101 Firefox/6.0.1";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    bool result = response.ContentType.ToLower().StartsWith("image");
                    if (!result)
                        Logger.LogDebug("Ignoring invalid thumb url {0}", url);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error checking thumb url {0} - {1}", url, ex.Message);
                return false;
            }
        }

        static Image getNewImage(Image input, int width, int height)
        {
            Image output = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            using (Graphics graphics = Graphics.FromImage(output))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(input, 0, 0, width, height);
            }
            return output;
        }
    }

    public class BitmapDownloadResult : IDisposable
    {
        object syncRoot = new object();
        volatile bool cancelled = false;
        Bitmap bitmap = null;
        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        volatile bool isCompleted = false;
        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        internal void Complete(Bitmap bitmap)
        {
            lock (syncRoot)
            {
                if (!cancelled)
                {
                    this.bitmap = bitmap;
                }
                else if (bitmap != null)
                {
                    try 
                    { 
                        bitmap.Dispose();
                    }
                    catch { }
                }
                isCompleted = true;
            }
        }

        public void Cancel()
        {
            lock (syncRoot)
            {
                Dispose();
                cancelled = true;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            lock (syncRoot)
            {
                if (bitmap != null)
                {
                    try { bitmap.Dispose(); }
                    catch { }
                }
            }
        }

        #endregion
    }

}
