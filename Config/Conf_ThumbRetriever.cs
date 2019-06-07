using MyEmulators2.Import;
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
    public partial class Conf_ThumbRetriever : Form
    {
        Game game = null;
        ThumbRetriever thumbRetriever = null;
        RomMatch romMatch = null;
        List<Bitmap> currentImages = null;
        bool closing = false;
        bool retrieverStopping = false;

        public Conf_ThumbRetriever(Game game)
        {
            InitializeComponent();

            if (game == null) //we need a valid game
                throw new ArgumentNullException("game");

            this.game = game;

            //configure thumb retriever events
            thumbRetriever = new ThumbRetriever();
            thumbRetriever.OnStatusChanged += new ThumbRetrieverStatusChangedHandler(thumbRetriever_OnStatusChanged);
            thumbRetriever.OnSearchCompleted += new SearchCompletedHandler(thumbRetriever_OnSearchCompleted);
            thumbRetriever.OnThumbDownloaded += new ThumbDownloadedHandler(thumbRetriever_OnThumbDownloaded);
            thumbRetriever.OnDownloadComplete += new ThumbDownloadCompleteHandler(thumbRetriever_OnDownloadComplete);
            resultsComboBox.DropDown += new EventHandler(resultsComboBox_DropDown);
            resultsComboBox.SelectedIndexChanged += new EventHandler(resultsComboBox_SelectedIndexChanged);
            //reset status info
            progressBar.Visible = false;
            statusLabel.Text = "";

            currentImages = new List<Bitmap>();
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
        
        //On first load set search term and start searching
        private void Conf_ThumbRetriever_Load(object sender, EventArgs e)
        {
            //configure results comboBox
            resultsComboBox.DisplayMember = "DisplayMember";
            resultsComboBox.DropDownClosed += new EventHandler(resultsComboBox_DropDownClosed);

            searchTextBox.Text = game.Title;
            searchButton_Click(this, new EventArgs());
        }

        //resets the form, updates the game to search against and starts searching
        internal void Reset(Game game)
        {
            //stop running threads and clear form
            stopAndClear();
            resultsComboBox.Items.Clear();
            closing = false;
            //update details to new game
            this.romMatch = null;
            this.game = game;
            searchTextBox.Text = game.Title;
            //start searching
            searchButton_Click(this, new EventArgs());
        }

        //Called when search button is pressed
        void searchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text)) //ensure we have a search term
                return;

            resultsComboBox.Items.Clear();
            progressBar.Visible = true;
            statusLabel.Text = "Searching...";
            thumbRetriever.Search(searchTextBox.Text, game);
        }

        void resultsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(resultsComboBox, resultsComboBox.Text);
        }

        void resultsComboBox_DropDown(object sender, EventArgs e)
        {
            int maxWidth = Emulators2Settings.GetComboBoxDropDownWidth(resultsComboBox);
            resultsComboBox.DropDownWidth = maxWidth > resultsComboBox.Width ? maxWidth : resultsComboBox.Width;
        }

        //called when an item is selected in the results box, update the romMatch
        //with the selected result and start downloading thumbs
        void resultsComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ScraperResult result = resultsComboBox.SelectedItem as ScraperResult;
            if (result == null || romMatch == null)
                return;

            if (result != romMatch.GameDetails)
            {
                //stopAndClear();
                romMatch.GameDetails = result;
                getThumbs(romMatch);
            }
        }

        //Called when the thumbRetriever has finished searching 
        void thumbRetriever_OnSearchCompleted(RomMatch romMatch)
        {
            //Ensure we're on main thread
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() 
                    {
                        thumbRetriever_OnSearchCompleted(romMatch);
                    }                    
                    ));
                return;
            }
            if (retrieverStopping)
                return;
            //reset status
            progressBar.Visible = false;
            statusLabel.Text = "";
            resultsComboBox.Items.Clear();

            //If there's no results say so and return
            if (romMatch.PossibleGameDetails == null || romMatch.PossibleGameDetails.Count < 1)
            {
                this.romMatch = null;
                resultsComboBox.Items.Add("No results");
                return;
            }

            //select top match if better not found as user can easily change
            if(romMatch.GameDetails == null)
                romMatch.GameDetails = romMatch.PossibleGameDetails[0];

            //keep a reference to the romMatch
            this.romMatch = romMatch;

            //populate the results box
            foreach (ScraperResult result in romMatch.PossibleGameDetails)
            {
                resultsComboBox.Items.Add(result);
            }
            //select the relevent combobox item
            resultsComboBox.SelectedItem = romMatch.GameDetails;
            //start downloading thumbs
            getThumbs(romMatch);
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

            currentImages.Add(image);
            //create new panel for the image
            Panel imagePnl = new Panel();
            imagePnl.BackgroundImage = image;
            imagePnl.Height = 88;
            imagePnl.Width = 109;
            
            imagePnl.BackgroundImageLayout = ImageLayout.Zoom;
            imagePnl.MouseDown += new MouseEventHandler(imagePnl_MouseDown); //add drag event handler

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

        //stop any running threads, clear the thumb panels and
        //start downloading thumbs
        void getThumbs(RomMatch romMatch)
        {
            progressBar.Visible = true;
            statusLabel.Text = "Downloading thumbs...";
            thumbRetriever.RetrieveThumbs(romMatch);
        }

        //allows drag n drop to be used on doenloaded images
        void imagePnl_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Panel).BackgroundImage, DragDropEffects.Copy);
        }
        
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
            //thumbRetriever.Stop(); //wait for threads to stop

            //ensure any remaining thumb completed events are 
            //processed so old thumbs are not added after clearing
            //Application.DoEvents();

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
