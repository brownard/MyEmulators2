using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    public partial class Conf_EmuThumbRetriever : Form
    {
        ThumbRetriever thumbRetriever = null;
        Emulator emu = null;
        Dictionary<string, string> platforms = null;
        string currentPlatform = null;
        bool retrieverStopping = false;
        List<Bitmap> currentImages = null;

        public Conf_EmuThumbRetriever(Emulator emu)
        {
            InitializeComponent();

            this.emu = emu;
            //configure thumb retriever events
            thumbRetriever = new ThumbRetriever();
            thumbRetriever.OnThumbDownloaded += new ThumbDownloadedHandler(thumbRetriever_OnThumbDownloaded);
            thumbRetriever.OnDownloadComplete += new ThumbDownloadCompleteHandler(thumbRetriever_OnDownloadComplete);
            thumbRetriever.OnPlatformsFound += new PlatformsFoundHandler(thumbRetriever_OnPlatformsFound);
            thumbRetriever.OnStatusChanged += new ThumbRetrieverStatusChangedHandler(thumbRetriever_OnStatusChanged);
            currentImages = new List<Bitmap>();

            //reset status info
            progressBar.Visible = false;
            statusLabel.Text = "";
        }

        void thumbRetriever_OnStatusChanged(ThumbRetrieverStatus status)
        {
            //Ensure we're on main thread
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    thumbRetriever_OnStatusChanged(status);
                }));
                return;
            }

            if (retrieverStopping)
            {
                if (status == ThumbRetrieverStatus.Stopped)
                    retrieverStopping = false;
                else
                    return;
            }
            else if (status == ThumbRetrieverStatus.Stopping)
            {
                retrieverStopping = true;
                stopAndClear();
            } 
        }

        void thumbRetriever_OnPlatformsFound(Dictionary<string, string> platforms, string selectedKey)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    thumbRetriever_OnPlatformsFound(platforms, selectedKey);
                }
                ));
                return;
            }

            //reset status
            progressBar.Visible = false;
            statusLabel.Text = "";
            resultsComboBox.Items.Clear();
            
            this.platforms = platforms;
            if (platforms == null)
                return;

            foreach (KeyValuePair<string, string> keyVal in platforms)
                resultsComboBox.Items.Add(keyVal.Key);

            currentPlatform = selectedKey;
            if (selectedKey != null)
            {
                resultsComboBox.SelectedItem = selectedKey;
                getThumbs(platforms[selectedKey]);
            }

        }
        
        //On first load set search term and start searching
        private void Conf_ThumbRetriever_Load(object sender, EventArgs e)
        {
            //configure results comboBox
            resultsComboBox.DropDownClosed += new EventHandler(resultsComboBox_DropDownClosed);
            getPlatforms();
        }

        //resets the form, updates the game to search against and starts searching
        internal void Reset(Emulator emu)
        {
            //stop running threads and clear form
            stopAndClear();
            resultsComboBox.Items.Clear();
            this.emu = emu;
            getPlatforms();
        }

        void getPlatforms()
        {
            progressBar.Visible = true;
            statusLabel.Text = "Searching...";
            thumbRetriever.GetPlatforms(emu.PlatformTitle);
        }
        
        //called when an item is selected in the results box, update the romMatch
        //with the selected result and start downloading thumbs
        void resultsComboBox_DropDownClosed(object sender, EventArgs e)
        {
            //toolTip1.Hide(resultsComboBox);

            string selectedItem = resultsComboBox.SelectedItem.ToString();

            string selectedId;
            if (platforms != null && selectedItem != null && selectedItem != currentPlatform && platforms.TryGetValue(selectedItem, out selectedId))
            {
                currentPlatform = selectedItem;
                stopAndClear();
                getThumbs(selectedId);
            }
        }

        void getThumbs(string selectedId)
        {
            progressBar.Visible = true;
            statusLabel.Text = "Downloading thumbs...";
            thumbRetriever.RetrieveThumbs(selectedId);
        }
        
        //called when the thumbRetriever has finished downloading a thumb
        void thumbRetriever_OnThumbDownloaded(Bitmap image, bool isCovers)
        {
            if (image == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    thumbRetriever_OnThumbDownloaded(image, isCovers);
                }
                ));
                return;
            }

            if (retrieverStopping)
            {
                try { image.Dispose(); }
                catch { }
                return;
            }

            //create new panel for the image
            Panel imagePnl = new Panel();
            imagePnl.BackgroundImage = image;
            imagePnl.Height = 88;
            imagePnl.Width = 107;
            
            imagePnl.BackgroundImageLayout = ImageLayout.Zoom;
            imagePnl.MouseDown += new MouseEventHandler(imagePnl_MouseDown); //add drag event handler
            currentImages.Add(image);
            //add panel to relevant flowLayoutControl
            if (isCovers)
                coversPanel.Controls.Add(imagePnl);
            else
                screensPanel.Controls.Add(imagePnl);
        }

        void thumbRetriever_OnDownloadComplete()
        {
            //Ensure we're on main thread
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    thumbRetriever_OnDownloadComplete();
                }
                ));
                return;
            }
            if (retrieverStopping)
                return;

            //hide status info
            progressBar.Visible = false;
            statusLabel.Text = "";
        }


        //allows drag n drop to be used on doenloaded images
        void imagePnl_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Panel).BackgroundImage, DragDropEffects.Copy);
        }

        bool closing = false;
        //Called by the main config window, ensure form is closed properly
        //and not just hidden
        public void close()
        {
            closing = true;
            Close();
        }

        private void Conf_ThumbRetriever_FormClosing(object sender, FormClosingEventArgs e)
        {
            thumbRetriever.Stop();
            stopAndClear();
            //if the user has requested close and the request has not come from the main config form,
            //just hide the form so it can be re-used
            if (e.CloseReason == CloseReason.UserClosing && !closing)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        //stops any running threads and clears the thumb panels
        void stopAndClear()
        {
            coversPanel.Controls.Clear();
            screensPanel.Controls.Clear();
            foreach (Bitmap image in currentImages)
            {
                try { image.Dispose(); }
                catch { }
            }
            currentImages.Clear();
        }
    }
}
