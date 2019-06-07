using System;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cornerstone.ScraperEngine;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace myEmulators
{
    public partial class Conf_OnlineLookup : Form
    {
        private ScriptableScraper scraper = null;
        Game searchGame;
        Image boxFrontPic;
        Image boxBackPic;
        Image boxTitlePic;
        Image boxIngamePic;
        Image boxFanart;
        bool LoadLocal = false;
        bool unattended = false;
        Thread workerThread = null;
        string SearchString = "";
        string SearchSystem = "";
        double thumbDimensions;

        delegate void resultsDelegate(List<ScraperResult> results);
        delegate void getGameDelegate(ScraperGame game, List<Bitmap> covers, List<Bitmap> screens);
        public delegate void onProgress(string label, int percent);

        public Conf_OnlineLookup(Game item, bool loadlocal)
        {
            InitializeComponent();
            init(item, loadlocal);
        }
        public Conf_OnlineLookup(Game item, bool loadlocal, bool unattended)
        {
            this.unattended = unattended;
            InitializeComponent();
            init(item, loadlocal);
        }

        void init(Game item, bool loadlocal)
        {
            this.FormClosing += new FormClosingEventHandler(Conf_OnlineLookup_FormClosing);
            LoadLocal = loadlocal;
            searchGame = item;

            LoadDropDowns();

            lblPath.Text = searchGame.Path;
            txt_SearchTerm.Text = StripRomCodes(searchGame.Title).Trim();

            int selecteditem = 0;

            selecteditem = ddlPlatform.FindStringExact(searchGame.ParentEmulator.Title);

            if (selecteditem > 0)
            {
                ddlPlatform.SelectedIndex = selecteditem;
            }
            else
            {
                if (searchGame.ParentEmulator.isPc())
                    selecteditem = ddlPlatform.FindString("Windows");
                else
                    selecteditem = ddlPlatform.FindString(searchGame.ParentEmulator.Title);

                if (selecteditem > 0)
                {
                    ddlPlatform.SelectedIndex = selecteditem;
                }
                else
                {
                    ddlPlatform.SelectedIndex = 0;
                }
            }

            thumbDimensions = ImageHandler.Instance.GetCaseAspect(searchGame.ParentEmulator.Title);

            chk_Visible.Checked = searchGame.Visible;
            chk_Favourite.Checked = searchGame.Favourite;

            if (LoadLocal)
            {
                txt_yearmade.Text = searchGame.Yearmade.ToString();
                txt_Title.Text = searchGame.Title;
                txt_grade.Text = searchGame.Grade.ToString();
                txt_genre.Text = searchGame.Genre;
                txt_description.Text = searchGame.Description;
                txt_company.Text = searchGame.Company;

                String boxfront = ThumbsHandler.Instance.createGameArt(searchGame, "BoxFront", false);
                if (boxfront != "")
                {
                    boxFrontPic = Bitmap.FromFile(boxfront);
                    pnlBoxFront.BackgroundImage = boxFrontPic;
                }

                String BackCoverPath = ThumbsHandler.Instance.createGameArt(searchGame, "BoxBack", false);
                if (BackCoverPath != "")
                {
                    boxBackPic = new Bitmap(BackCoverPath);
                    pnlBoxBack.BackgroundImage = boxBackPic;
                }

                String TitleScreenPath = ThumbsHandler.Instance.createGameArt(searchGame, "TitleScreenshot", false);
                if (TitleScreenPath != "")
                {
                    boxTitlePic = new Bitmap(TitleScreenPath);
                    pnlShotTitle.BackgroundImage = boxTitlePic;
                }

                String IngameScreenPath = ThumbsHandler.Instance.createGameArt(searchGame, "IngameScreenshot", false);
                if (IngameScreenPath != "")
                {
                    boxIngamePic = new Bitmap(IngameScreenPath);
                    pnlShotIngame.BackgroundImage = boxIngamePic;
                }

                String FanartPath = ThumbsHandler.Instance.createGameArt(searchGame, "Fanart", false);
                if (FanartPath != "")
                {
                    boxFanart = new Bitmap(FanartPath);
                    pnlFanart.BackgroundImage = boxFanart;
                }

                String ManualPath = ThumbsHandler.Instance.createGameArt(searchGame, "manual", false);
                if (ManualPath != "")
                {
                    btnManual.Enabled = true;
                }
                else
                {
                    btnManual.Enabled = false;
                }
            }
            else
            {
                button1_Click(this, null);
            }
        }

        void Conf_OnlineLookup_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopWorker(); //stop background thread if closing
        }

        public void LoadDropDowns()
        {
            ddlPlatform.DataSource = Dropdowns.GetSystems();
            ddlPlatform.ValueMember = "Value";
            ddlPlatform.DisplayMember = "Text";

            ddlPlatform.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e) //Search
        {
            stopWorker();
            if (!LoadLocal)
            {
                ClearForm();
            }

            //load scraper
            Assembly asm = Assembly.GetExecutingAssembly();
            StreamReader sr = new StreamReader(asm.GetManifestResourceStream("myEmulators.Scripts.Mobygames.xml"));
            LoadScraper(sr.ReadToEnd());
            sr.Dispose();
            
            SearchString = txt_SearchTerm.Text;
            SearchSystem = "";
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList["search.title"] = SearchString; //add game title to scraper params
            if (ddlPlatform.SelectedValue.ToString() != "0") //see if we have a platform match
            {
                paramList["search.system"] = ddlPlatform.SelectedValue.ToString(); //add platform to scraper params
                SearchSystem = ddlPlatform.Text;
            }
            
            workerThread = new Thread(new ThreadStart(delegate()
                {
                    //executed on background thread
                    try
                    {
                        onProgress prog = new onProgress(onThreadProgress);
                        prog.Invoke("Searching...", 0);
                        List<ScraperResult> results = Conf_Scraper.getSearchResults(scraper, paramList);
                        prog.Invoke("Complete", 100);
                        BeginInvoke(new resultsDelegate(getResults), new object[] { results });
                    }
                    catch(ThreadAbortException) 
                    {
                        Thread.ResetAbort();
                    }
                }
                )) { IsBackground = true };
            workerThread.Start();

            //List<ScraperResult> results = Scraper.getSearchResults(scraper, paramList);
            //getResults(results);
            //Get();
        }

        void getResults(List<ScraperResult> results)
        {
            dataGridView1.Rows.Clear();
            foreach (ScraperResult result in results)
            {
                DataGridViewRow row = new DataGridViewRow();

                //title
                DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                cell1.Value = result.Title;
                row.Cells.Add(cell1);

                //year
                DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                cell2.Value = result.Year;
                row.Cells.Add(cell2);

                //console
                DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                cell3.Value = result.System;
                row.Cells.Add(cell3);

                //site_id
                DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                cell4.Value = result.SiteId;
                row.Cells.Add(cell4);

                dataGridView1.Rows.Add(row);
            }

            bool importTop = Options.Instance.GetBoolOption("importtop");

            if (dataGridView1.RowCount == 0)
            {
                if (unattended)
                {
                    Close(); //no results and unattended so close
                    return;
                }
                txt_Title.Text = SearchString;
            }
            else if (dataGridView1.RowCount == 1 && !LoadLocal && importTop)
            {
                getGameInfo(dataGridView1.Rows[0].Cells[3].Value.ToString());
            }
            else if (!LoadLocal)
            {
                int possibleTitleMatches = 0;
                int titleIndex = 0;
                int possiblePlatformMatches = 0;
                int platformIndex = 0;
                bool isExact = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((row.Cells[0].Value as string).ToLower() == SearchString.ToLower() && (row.Cells[2].Value as string).ToLower() == SearchSystem.ToLower())
                    {
                        //exact match so break and get info
                        isExact = true;
                        possibleTitleMatches = 1;
                        titleIndex = row.Index;
                        break;
                    }
                    else if ((row.Cells[0].Value as string).ToLower() == SearchString) //title match
                    {
                        possibleTitleMatches++;
                        if (possibleTitleMatches == 1)
                            titleIndex = row.Index;
                    }
                    else if ((row.Cells[2].Value as string).ToLower() == SearchSystem) //platfrom match
                    {
                        possiblePlatformMatches++;
                        if (possiblePlatformMatches == 1)
                            platformIndex = row.Index;
                    }
                }

                bool importExact = Options.Instance.GetBoolOption("importexact");

                int selectedItem = -1;
                if (possibleTitleMatches == 1 && (isExact || !importExact))
                    selectedItem = titleIndex; //only 1 title match, select if also platform match or 'import exact' option not selected
                else if (possiblePlatformMatches == 1 && !importExact)
                    selectedItem = platformIndex; ////only 1 platform match, only select if 'import exact' not selected

                if (selectedItem > -1) //if we have a match get game info
                {
                    dataGridView1.Rows[selectedItem].Selected = true;
                    getGameInfo(dataGridView1.Rows[selectedItem].Cells[3].Value.ToString());
                }
                else if (Options.Instance.GetBoolOption("importtop")) //if no match and 'import top' selected, get first result
                {
                    dataGridView1.Rows[0].Selected = true;
                    getGameInfo(dataGridView1.Rows[0].Cells[3].Value.ToString());
                }
                else if (unattended)
                    Close(); //no match and unattended so close
            }
        }

        void onThreadProgress(string label, int percent)
        {
            //called by background thread periodically
            if (InvokeRequired) //update gui on main thread
                Invoke(new onProgress(onThreadProgress), new object[] { label, percent }); 
            else
            {
                if (percent < 0)
                    percent = 0;
                else if (percent > 100)
                    percent = 100;
                searchLabel.Text = label;
                searchProgressBar.Value = percent;
            }
        }

        public void getGameInfo(string gameId)
        {
            stopWorker();
            if (!LoadLocal)
                ClearForm();
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList["game.site_id"] = gameId;

            workerThread = new Thread(new ThreadStart(delegate() 
                {
                    try
                    {
                        onProgress progress = new onProgress(onThreadProgress);
                        progress.Invoke("Getting game info...", 0);
                        ScraperGame game = Conf_Scraper.getGame(scraper, paramList);
                        progress.Invoke("Getting cover art...", 30);
                        Bitmap front, back, title, inGame;
                        List<Bitmap> covers = Conf_Scraper.getCovers(scraper, paramList, out front, out back, progress, 30, 65, unattended);
                        game.BoxFront = front;
                        game.BoxBack = back;
                        progress.Invoke("Getting screenshots...", 65);
                        List<Bitmap> screens = Conf_Scraper.getScreens(scraper, paramList, out title, out inGame, progress, 65, 100, unattended);
                        game.TitleScreen = title;
                        game.InGame = inGame;
                        BeginInvoke(new getGameDelegate(addGameInfo), new object[] { game, covers, screens });
                    }
                    catch
                    {
                    }
                }
                ));
            //don't allow save while searching
            btnSave.Enabled = false;

            workerThread.Start();
        }

        public void addGameInfo(ScraperGame game, List<Bitmap> covers, List<Bitmap> screens)
        {
            onThreadProgress("Complete", 100);

            game.Description = StripTags(game.Description);

            txt_Title.Text = game.Title;

            if (!LoadLocal)
            {
                txt_yearmade.Text = game.Year;

                try
                {
                    txt_grade.Text = Convert.ToInt32(Math.Round((Convert.ToDouble(game.Grade, System.Globalization.CultureInfo.InvariantCulture) * 2), 0)).ToString();
                }
                catch
                {
                    txt_grade.Text = "0";
                }

                try
                {
                    txt_genre.Text = game.Genre.Split('|')[1];
                }
                catch
                {
                    txt_genre.Text = "";
                }

                txt_description.Text = game.Description;
                txt_company.Text = game.Company;

                pnlBoxFront.BackgroundImage = game.BoxFront;
                pnlBoxBack.BackgroundImage = game.BoxBack;
                pnlShotTitle.BackgroundImage = game.TitleScreen;
                pnlShotIngame.BackgroundImage = game.InGame;

            }
            else
            {
                if (txt_yearmade.Text == "")
                {
                    txt_yearmade.Text = game.Year;
                }

                if (txt_grade.Text == "")
                {
                    try
                    {
                        txt_grade.Text = Convert.ToInt32(Math.Round((Convert.ToDouble(game.Grade, System.Globalization.CultureInfo.InvariantCulture) * 2), 0)).ToString();
                    }
                    catch
                    {
                        txt_grade.Text = "0";
                    }
                }

                if (txt_genre.Text == "")
                {
                    try
                    {
                        txt_genre.Text = game.Genre.Split('|')[1];
                    }
                    catch
                    {
                        txt_genre.Text = "";
                    }
                }

                if (txt_description.Text == "")
                {
                    txt_description.Text = game.Description;
                }

                if (txt_company.Text == "")
                {
                    txt_company.Text = game.Company;
                }

                if (pnlBoxFront.BackgroundImage == null)
                    pnlBoxFront.BackgroundImage = game.BoxFront;
                if (pnlBoxBack.BackgroundImage == null)
                    pnlBoxBack.BackgroundImage = game.BoxBack;
                if (pnlShotTitle.BackgroundImage == null)
                    pnlShotTitle.BackgroundImage = game.TitleScreen;
                if (pnlShotIngame.BackgroundImage == null)
                    pnlShotIngame.BackgroundImage = game.InGame;
            }

            foreach (Bitmap thepic in covers)
            {
                Panel pic = new Panel();
                pic.BackgroundImage = thepic;
                pic.Height = 115;
                pic.Width = 115;
                pic.BackgroundImageLayout = ImageLayout.Zoom;
                pic.MouseDown += new MouseEventHandler(pic_MouseDown);
                pnlBoxArt.Controls.Add(pic);
            }
            foreach (Bitmap thepic in screens)
            {
                Panel pic = new Panel();
                pic.BackgroundImage = thepic;
                pic.Height = 115;
                pic.Width = 115;
                pic.BackgroundImageLayout = ImageLayout.Zoom;
                pic.MouseDown += new MouseEventHandler(pic_MouseDown);

                pnlScreenshots.Controls.Add(pic);
            }
            btnSave.Enabled = true;
            if (unattended)
                btnSave_Click(this, null);
        }

        public bool LoadScraper(string script)
        {
            scraper = new ScriptableScraper(script, false);

            if (!scraper.LoadSuccessful)
            {
                scraper = null;
                return false;
            }

            return true;
        }

        #region Old Methods

        public void Get()
        {
            if (scraper == null)
                return;

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            Dictionary<string, string> results;

            paramList["search.title"] = txt_SearchTerm.Text;

            if (ddlPlatform.SelectedValue.ToString() != "0")
            {
                paramList["search.system"] = ddlPlatform.SelectedValue.ToString();
            }

            results = scraper.Execute("search", paramList);
            if (results == null)
            {
                return;
            }

            string respage = "";

            results.TryGetValue("search_page", out respage);
            int count = 0;
            // The movie result is only valid if the script supplies a unique site

            dataGridView1.Rows.Clear();
            while (results.ContainsKey("game[" + count + "].site_id"))
            {
                string siteId;
                string title;
                string yearmade;
                string system;
                string xsiteId;
                string xgamePattern;
                string prefix = "game[" + count + "].";
                count++;


                if (!results.TryGetValue(prefix + "site_id", out siteId))
                    continue;

                if (results.TryGetValue(prefix + "system", out system))
                    system = system.Trim();
                results.TryGetValue(prefix + "title", out title);
                results.TryGetValue(prefix + "yearmade", out yearmade);
                results.TryGetValue(prefix + "extraGameUrl", out xsiteId);
                results.TryGetValue(prefix + "extraGamePattern", out xgamePattern);

                if (!string.IsNullOrEmpty(xsiteId))
                    siteId = xsiteId;

                DataGridViewRow row = new DataGridViewRow();
                //row.Tag = game;

                DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                cell2.Value = title;
                row.Cells.Add(cell2);

                DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                cell4.Value = yearmade;
                row.Cells.Add(cell4);

                DataGridViewTextBoxCell cell8 = new DataGridViewTextBoxCell();
                cell8.Value = system;
                row.Cells.Add(cell8);

                DataGridViewTextBoxCell cell9 = new DataGridViewTextBoxCell();
                cell9.Value = siteId;
                row.Cells.Add(cell9);

                dataGridView1.Rows.Add(row);

                if (!string.IsNullOrEmpty(xgamePattern))
                {
                    foreach (Match match in new Regex(@"<a href=""/game/([^""]*)"">([^<]*)</a> \(<em>(\d{4})").Matches(xgamePattern))
                    {
                        DataGridViewRow xrow = new DataGridViewRow();
                        //row.Tag = game;

                        DataGridViewTextBoxCell xcell2 = new DataGridViewTextBoxCell();
                        xcell2.Value = title;
                        xrow.Cells.Add(xcell2);

                        DataGridViewTextBoxCell xcell4 = new DataGridViewTextBoxCell();
                        xcell4.Value = match.Groups[3].Value;
                        xrow.Cells.Add(xcell4);

                        DataGridViewTextBoxCell xcell8 = new DataGridViewTextBoxCell();
                        xcell8.Value = match.Groups[2].Value;
                        xrow.Cells.Add(xcell8);

                        DataGridViewTextBoxCell xcell9 = new DataGridViewTextBoxCell();
                        xcell9.Value = match.Groups[1].Value;
                        xrow.Cells.Add(xcell9);

                        dataGridView1.Rows.Add(xrow);
                    }
                }
            }

            if (dataGridView1.RowCount == 0)
            {
                txt_Title.Text = txt_SearchTerm.Text;
            }
            else if (dataGridView1.RowCount == 1 && !LoadLocal)
            {
                ClearForm();
                GetGame(dataGridView1.Rows[0].Cells[3].Value.ToString());
                GetArtwork(dataGridView1.Rows[0].Cells[3].Value.ToString());
                GetScreenshots(dataGridView1.Rows[0].Cells[3].Value.ToString());
            }
            else if (!LoadLocal)
            {
                int possibleTitleMatches = 0;
                int titleIndex = 0;
                int possiblePlatformMatches = 0;
                int platformIndex = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((row.Cells[0].Value as string).ToLower() == txt_SearchTerm.Text.ToLower() && (row.Cells[2].Value as string).ToLower() == ddlPlatform.Text.ToLower())
                    {
                        //exact match so break and get info
                        possibleTitleMatches = 1;
                        titleIndex = row.Index;
                        break;
                    }
                    else if ((row.Cells[0].Value as string).ToLower() == txt_SearchTerm.Text.ToLower())
                    {
                        possibleTitleMatches++;
                        if (possibleTitleMatches == 1)
                            titleIndex = row.Index;
                    }
                    else if ((row.Cells[2].Value as string).ToLower() == ddlPlatform.Text.ToLower())
                    {
                        possiblePlatformMatches++;
                        if (possiblePlatformMatches == 1)
                            platformIndex = row.Index;
                    }
                }
                int selectedItem = -1;
                if (possibleTitleMatches == 1)
                    selectedItem = titleIndex;
                else if (possiblePlatformMatches == 1)
                    selectedItem = platformIndex;

                if (selectedItem > -1)
                {
                    dataGridView1.Rows[selectedItem].Selected = true;
                    ClearForm();
                    GetGame(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                    GetArtwork(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                    GetScreenshots(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                }
            }
        }

        public void GetGame(string gameID)
        {
            if (scraper == null)
                return;

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            Dictionary<string, string> results;

            paramList["game.site_id"] = gameID;

            // try to retrieve results
            results = scraper.Execute("get_details", paramList);
            if (results == null)
            {
                return;
            }

            string grade;
            string title;
            string yearmade;
            string company;
            string description;
            string genre;

            results.TryGetValue("game.grade", out grade);
            results.TryGetValue("game.title", out title);
            results.TryGetValue("game.yearmade", out yearmade);
            results.TryGetValue("game.company", out company);
            results.TryGetValue("game.description", out description);
            results.TryGetValue("game.genre", out genre);

            description = StripTags(description);

            txt_Title.Text = title;

            if (!LoadLocal)
            {
                txt_yearmade.Text = yearmade;

                try
                {
                    txt_grade.Text = Convert.ToInt32(Math.Round((Convert.ToDouble(grade, System.Globalization.CultureInfo.InvariantCulture) * 2), 0)).ToString();
                }
                catch
                {
                    txt_grade.Text = "0";
                }

                try
                {
                    txt_genre.Text = genre.Split('|')[1];
                }
                catch
                {
                    txt_genre.Text = "";
                }

                txt_description.Text = description;
                txt_company.Text = company;
            }
            else
            {
                if (txt_yearmade.Text == "")
                {
                    txt_yearmade.Text = yearmade;
                }

                if (txt_grade.Text == "")
                {
                    try
                    {
                        txt_grade.Text = Convert.ToInt32(Math.Round((Convert.ToDouble(grade, System.Globalization.CultureInfo.InvariantCulture) * 2), 0)).ToString();
                    }
                    catch
                    {
                        txt_grade.Text = "0";
                    }
                }

                if (txt_genre.Text == "")
                {
                    try
                    {
                        txt_genre.Text = genre.Split('|')[1];
                    }
                    catch
                    {
                        txt_genre.Text = "";
                    }
                }

                if (txt_description.Text == "")
                {
                    txt_description.Text = description;
                }

                if (txt_company.Text == "")
                {
                    txt_company.Text = company;
                }
            }
        }

        public void GetArtwork(string gameID)
        {
            if (scraper == null)
                return;

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            Dictionary<string, string> results;

            paramList["game.site_id"] = gameID;
            results = scraper.Execute("get_cover_art", paramList);

            if (results == null)
            {
                return;
            }

            string covers = "";

            if (results.TryGetValue("game.covers", out covers))
            {
                string[] coverurls = covers.Split('|');

                for (int i = 1; i < coverurls.Length; i++)
                {
                    String extension = coverurls[i].Substring(coverurls[i].Length - (coverurls[i].Length - coverurls[i].LastIndexOf('.')));
                    System.Net.WebClient objWebClient = new System.Net.WebClient();

                    Panel pic = new Panel();
                    Bitmap thepic = new Bitmap(objWebClient.OpenRead(coverurls[i]));
                    pic.BackgroundImage = thepic;
                    pic.Height = 115;
                    pic.Width = 115;
                    pic.BackgroundImageLayout = ImageLayout.Zoom;
                    pic.MouseDown += new MouseEventHandler(pic_MouseDown);
                    pnlBoxArt.Controls.Add(pic);

                    if (i == 1 && !LoadLocal)
                    {
                        pnlBoxFront.BackgroundImage = thepic;
                    }
                    else if (i == 2 && !LoadLocal)
                    {
                        pnlBoxBack.BackgroundImage = thepic;
                    }
                }
            }
        }

        public void GetScreenshots(string gameID)
        {
            if (scraper == null)
                return;

            Dictionary<string, string> paramList = new Dictionary<string, string>();
            Dictionary<string, string> results;

            paramList["game.site_id"] = gameID;
            results = scraper.Execute("get_screenshots", paramList);

            if (results == null)
            {
                return;
            }

            string screens = "";
            string screenshotsbaseurl = "";

            results.TryGetValue("game.screenshotsbaseurl", out screenshotsbaseurl);

            if (results.TryGetValue("game.screenshots", out screens))
            {
                string[] screenurls = screens.Split('|');
                bool foundTitleScreen = false;
                bool foundInGame = false;

                for (int i = 1; i < screenurls.Length; i++)
                {
                    if (!screenurls[i].ToLower().StartsWith("http://"))
                    {
                        screenurls[i] = screenshotsbaseurl + screenurls[i];
                    }

                    String extension = screenurls[i].Substring(screenurls[i].Length - (screenurls[i].Length - screenurls[i].LastIndexOf('.')));
                    System.Net.WebClient objWebClient = new System.Net.WebClient();

                    Panel pic = new Panel();
                    Bitmap thepic = new Bitmap(objWebClient.OpenRead(screenurls[i]));
                    pic.BackgroundImage = thepic;
                    pic.Height = 115;
                    pic.Width = 115;
                    pic.BackgroundImageLayout = ImageLayout.Zoom;
                    pic.MouseDown += new MouseEventHandler(pic_MouseDown);
                    pnlScreenshots.Controls.Add(pic);

                    if (LoadLocal || (foundTitleScreen && foundInGame))
                        continue;

                    if (screenurls[i].Contains("screenshot-start-screen"))
                    {
                        pnlShotTitle.BackgroundImage = thepic;
                        foundTitleScreen = true;
                    }
                    else if (screenurls[i].Contains("screenshot-title-screen"))
                    {
                        if (!foundTitleScreen)
                        {
                            pnlShotTitle.BackgroundImage = thepic;
                        }
                    }
                    else if (pnlShotTitle.BackgroundImage == null)
                    {
                        pnlShotTitle.BackgroundImage = thepic;
                    }
                    else if (pnlShotIngame.BackgroundImage == null)
                    {
                        pnlShotIngame.BackgroundImage = thepic;
                        foundInGame = true;
                    }
                }
            }
        }

        #endregion
        

        private string StripRomCodes(string Rom)
        {
            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex(@"\(.*?\)");
            return objRegEx.Replace(Rom, "");
        }

        private string StripTags(string HTML)
        {
            HTML = HTML.Replace("<br>", System.Environment.NewLine);
            HTML = HTML.Replace("<br />", System.Environment.NewLine);
            HTML = HTML.Replace("<br/>", System.Environment.NewLine);

            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex("<[^>]*>");
            return objRegEx.Replace(HTML, "");
        }

        void pic_MouseDown(object sender, MouseEventArgs e)
        {
            Panel source = (Panel)sender;
            DoDragDrop(source.BackgroundImage, DragDropEffects.Copy);
        }
        
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!LoadLocal)
            {
                ClearForm();
            }
            /*
            GetGame(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            GetArtwork(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            GetScreenshots(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
             * */
            getGameInfo(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
        }

        public void ClearForm()
        {
            txt_yearmade.Text = "";
            txt_Title.Text = "";
            txt_grade.Text = "";
            txt_genre.Text = "";
            txt_description.Text = "";
            txt_company.Text = "";

            try
            {
                pnlBoxBack.BackgroundImage.Dispose();
            }
            catch { }

            try
            {
                pnlBoxFront.BackgroundImage.Dispose();
            }
            catch { }

            try
            {
                pnlShotTitle.BackgroundImage.Dispose();
            }
            catch { }

            try
            {
                pnlShotIngame.BackgroundImage.Dispose();
            }
            catch { }

            try
            {
                pnlFanart.BackgroundImage.Dispose();
            }
            catch { }

            pnlFanart.BackgroundImage = null;
            pnlBoxBack.BackgroundImage = null;
            pnlBoxFront.BackgroundImage = null;
            pnlShotTitle.BackgroundImage = null;
            pnlShotIngame.BackgroundImage = null;

            pnlScreenshots.Controls.Clear();
            pnlBoxArt.Controls.Clear();

            try
            {
                boxFrontPic.Dispose();
            }
            catch { }

            try
            {
                boxBackPic.Dispose();
            }
            catch { }

            try
            {
                boxTitlePic.Dispose();
            }
            catch { }

            try
            {
                boxIngamePic.Dispose();
            }
            catch { }

            try
            {
                boxFanart.Dispose();
            }
            catch { }
        }

        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            Panel destination = (Panel)sender;

            Bitmap backgroundImage = null;
            string pnlTxt = "";

            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                backgroundImage = (Bitmap)e.Data.GetData(typeof(Bitmap));
                //destination.BackgroundImage = ImageHandler.Instance.resizeImage((Bitmap)e.Data.GetData(typeof(Bitmap)), thumbDimensions);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                pnlTxt = files[0];
                try
                {
                    backgroundImage = new Bitmap(files[0]);
                }
                catch { }

                //destination.BackgroundImage = ImageHandler.Instance.resizeImage(new Bitmap(files[0]), thumbDimensions);
            }

            if (backgroundImage != null)
            {
                switch (destination.Name)
                {
                    case "pnlShotIngame":
                        {
                            destination.BackgroundImage = ImageHandler.Instance.NewImage(backgroundImage);
                            txt_BoxIngame.Text = pnlTxt;
                            break;
                        }
                    case "pnlShotTitle":
                        {
                            destination.BackgroundImage = ImageHandler.Instance.NewImage(backgroundImage);
                            txt_BoxTitle.Text = pnlTxt;
                            break;
                        }
                    case "pnlBoxBack":
                        {
                            destination.BackgroundImage = ImageHandler.Instance.resizeImage(backgroundImage, thumbDimensions);
                            txt_BoxBack.Text = pnlTxt;
                            break;
                        }
                    case "pnlBoxFront":
                        {
                            destination.BackgroundImage = ImageHandler.Instance.resizeImage(backgroundImage, thumbDimensions);
                            txt_BoxFront.Text = pnlTxt;
                            break;
                        }
                    case "pnlFanart":
                        {
                            destination.BackgroundImage = ImageHandler.Instance.NewImage(backgroundImage);
                            txt_Fanart.Text = pnlTxt;
                            break;
                        }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            searchGame.Title = txt_Title.Text;

            try
            {
                searchGame.Grade = Int32.Parse(txt_grade.Text);
            }
            catch 
            {
                searchGame.Grade = 0;
            }

            try
            {
                searchGame.Yearmade = Int32.Parse(txt_yearmade.Text);
            }
            catch { }

            searchGame.Description = txt_description.Text;
            searchGame.Genre = txt_genre.Text;
            searchGame.Company = txt_company.Text;
            searchGame.Visible = chk_Visible.Checked;
            searchGame.Favourite = chk_Favourite.Checked;

            searchGame.Save();

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
            ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()[1];
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + searchGame.ParentEmulator.Title))
            {
                Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + searchGame.ParentEmulator.Title);
            }

            string SavePath = ThumbsHandler.Instance.thumb_games + @"\" + searchGame.ParentEmulator.Title + @"\" + searchGame.GameID.ToString();

            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            //Image saving has been refactored to eliminate issues with file 
            //conversion and saving over images currently in use
            if (pnlBoxFront.BackgroundImage != null)
            {
                try
                {
                    Image saveBmp = null;
                    bool save = false;
                    if (Options.Instance.GetBoolOption("resizethumbs")) //create new, resized image based on predefined aspect ratio
                    {
                        saveBmp = ImageHandler.Instance.resizeImage(pnlBoxFront.BackgroundImage, thumbDimensions); //creates new image in memory
                        save = true;
                    }
                        //only create new image if we don't have a direct reference to a file in a valid format
                    else if (txt_BoxFront.Text == "" || (!txt_BoxFront.Text.EndsWith(".jpg") && !txt_BoxFront.Text.EndsWith(".png")))
                    {
                        saveBmp = ImageHandler.Instance.NewImage(pnlBoxFront.BackgroundImage);
                        save = true;
                    }

                    try
                    {
                        //should be safe to dispose as image to save is either a new image in memory or a file
                        pnlBoxFront.BackgroundImage.Dispose();
                    }
                    catch { }

                    pnlBoxFront.BackgroundImage = null;

                    try
                    {
                        boxFrontPic.Dispose();
                    }
                    catch { }

                    if (save)
                    {
                        //convert and save image in memory to jpeg
                        saveBmp.Save(SavePath + @"\BoxFront.jpg", jpegCodec, encoderParams);
                        saveBmp.Dispose();
                    }
                    else
                    {
                        //copy file to thumb location 
                        string destinationFile = SavePath + @"\BoxFront." + txt_BoxFront.Text.Substring(txt_BoxFront.Text.Length - 3).ToLower();
                        if (txt_BoxFront.Text != destinationFile)
                            File.Copy(txt_BoxFront.Text, destinationFile, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not copy the file to thumbnail location - {0}", ex.Message), "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    File.Delete(SavePath + @"\BoxFront.jpg");
                }
                catch { }
            }

            if (pnlBoxBack.BackgroundImage != null)
            {
                try
                {
                    Image saveBmp = null;
                    bool save = false;
                    if (Options.Instance.GetBoolOption("resizethumbs"))
                    {
                        saveBmp = ImageHandler.Instance.resizeImage(pnlBoxBack.BackgroundImage, thumbDimensions);
                        save = true;
                    }
                    else if (txt_BoxBack.Text == "" || (!txt_BoxBack.Text.EndsWith(".jpg") && !txt_BoxBack.Text.EndsWith(".png")))
                    {
                        saveBmp = ImageHandler.Instance.NewImage(pnlBoxBack.BackgroundImage);
                        save = true;
                    }
                    try
                    {
                        pnlBoxBack.BackgroundImage.Dispose();
                    }
                    catch { }

                    pnlBoxBack.BackgroundImage = null;

                    try
                    {
                        boxBackPic.Dispose();
                    }
                    catch { }

                    if (save)
                    {
                        saveBmp.Save(SavePath + @"\BoxBack.jpg", jpegCodec, encoderParams);
                        saveBmp.Dispose();
                    }
                    else
                    {
                        string destinationFile = SavePath + @"\BoxBack." + txt_BoxBack.Text.Substring(txt_BoxBack.Text.Length - 3).ToLower();
                        if (txt_BoxBack.Text != destinationFile)
                            File.Copy(txt_BoxBack.Text, destinationFile, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not copy the file to thumbnail location - {0}", ex.Message), "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    File.Delete(SavePath + @"\BoxBack.jpg");
                }
                catch { }
            }

            if (pnlShotTitle.BackgroundImage != null)
            {
                try
                {
                    Image saveBmp = null;
                    bool save = false;
                    if (txt_BoxTitle.Text == "" || (!txt_BoxTitle.Text.EndsWith(".jpg") && !txt_BoxTitle.Text.EndsWith(".png")))
                    {
                        saveBmp = ImageHandler.Instance.NewImage(pnlShotTitle.BackgroundImage);
                        save = true;
                    }
                    try
                    {
                        pnlShotTitle.BackgroundImage.Dispose();
                    }
                    catch { }

                    pnlShotTitle.BackgroundImage = null;

                    try
                    {
                        boxTitlePic.Dispose();
                    }
                    catch { }

                    if (save)
                    {
                        saveBmp.Save(SavePath + @"\TitleScreenshot.jpg", jpegCodec, encoderParams);
                        saveBmp.Dispose();
                    }
                    else
                    {
                        string destinationFile = SavePath + @"\TitleScreenshot." + txt_BoxTitle.Text.Substring(txt_BoxTitle.Text.Length - 3).ToLower();
                        if (txt_BoxTitle.Text != destinationFile)
                            File.Copy(txt_BoxTitle.Text, destinationFile, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not copy the file to thumbnail location - {0}", ex.Message), "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    File.Delete(SavePath + @"\TitleScreenshot.jpg");
                }
                catch { }
            }

            if (pnlShotIngame.BackgroundImage != null)
            {
                try
                {
                    Image saveBmp = null;
                    bool save = false;
                    if (txt_BoxIngame.Text == "" || (!txt_BoxIngame.Text.EndsWith(".jpg") && !txt_BoxIngame.Text.EndsWith(".png")))
                    {
                        saveBmp = ImageHandler.Instance.NewImage(pnlShotIngame.BackgroundImage);
                        save = true;
                    }
                    try
                    {
                        pnlShotIngame.BackgroundImage.Dispose();
                    }
                    catch { }

                    pnlShotIngame.BackgroundImage = null;

                    try
                    {
                        boxIngamePic.Dispose();
                    }
                    catch { }

                    if (save)
                    {
                        saveBmp.Save(SavePath + @"\IngameScreenshot.jpg", jpegCodec, encoderParams);
                        saveBmp.Dispose();
                    }
                    else
                    {
                        string destinationFile = SavePath + @"\IngameScreenshot." + txt_BoxIngame.Text.Substring(txt_BoxIngame.Text.Length - 3).ToLower();
                        if (txt_BoxIngame.Text != destinationFile)
                            File.Copy(txt_BoxIngame.Text, destinationFile, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not copy the file to thumbnail location - {0}", ex.Message), "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    File.Delete(SavePath + @"\IngameScreenshot.jpg");
                }
                catch { }
            }

            if (pnlFanart.BackgroundImage != null)
            {
                try
                {
                    Image saveBmp = null;
                    bool save = false;
                    if (txt_Fanart.Text == "" || (!txt_Fanart.Text.EndsWith(".jpg") && !txt_Fanart.Text.EndsWith(".png")))
                    {
                        saveBmp = ImageHandler.Instance.NewImage(pnlFanart.BackgroundImage);
                        save = true;
                    }
                    try
                    {
                        pnlFanart.BackgroundImage.Dispose();
                    }
                    catch { }

                    pnlFanart.BackgroundImage = null;

                    try
                    {
                        boxFanart.Dispose();
                    }
                    catch { }

                    if (save)
                    {
                        saveBmp.Save(SavePath + @"\Fanart.jpg", jpegCodec, encoderParams);
                        saveBmp.Dispose();
                    }
                    else
                    {
                        string destinationFile = SavePath + @"\Fanart." + txt_Fanart.Text.Substring(txt_Fanart.Text.Length - 3).ToLower();
                        if (txt_Fanart.Text != destinationFile)
                            File.Copy(txt_Fanart.Text, destinationFile, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not copy the file to thumbnail location - {0}", ex.Message), "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    File.Delete(SavePath + @"\Fanart.jpg");
                }
                catch { }
            }

            if (txt_Manual.Text != "")
            {
                try
                {
                    String extension = txt_Manual.Text.Substring(txt_Manual.Text.Length - 3);
                    File.Copy(txt_Manual.Text, SavePath + @"\Manual." + extension, true);
                    txt_Manual.Text = "";
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to manual location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }




            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelBatch_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void Conf_OnlineLookup_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopWorker();
            ClearForm();
        }

        private void btnNewManual_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select thumbnail image";
            diag.Filter = "PDF files (*.pdf) | *.pdf";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                txt_Manual.Text = diag.FileName;
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Executor.launchDocument(searchGame);
        }

        private void txt_Manual_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void txt_Manual_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                txt_Manual.Text = files[0];
            }
        }

        private void btnDelFront_Click(object sender, EventArgs e)
        {
            try
            {
                pnlBoxFront.BackgroundImage.Dispose();
            }
            catch { }

            pnlBoxFront.BackgroundImage = null;

            try
            {
                boxFrontPic.Dispose();
            }
            catch { }

            txt_BoxFront.Text = "";
        }

        private void btnDelBack_Click(object sender, EventArgs e)
        {
            try
            {
                pnlBoxBack.BackgroundImage.Dispose();
            }
            catch { }

            pnlBoxBack.BackgroundImage = null;

            try
            {
                boxBackPic.Dispose();
            }
            catch { }

            txt_BoxBack.Text = "";
        }

        private void btnDelTitle_Click(object sender, EventArgs e)
        {
            try
            {
                pnlShotTitle.BackgroundImage.Dispose();
            }
            catch { }

            pnlShotTitle.BackgroundImage = null;

            try
            {
                boxTitlePic.Dispose();
            }
            catch { }

            txt_BoxTitle.Text = "";
        }

        private void btnDelIngame_Click(object sender, EventArgs e)
        {
            try
            {
                pnlShotIngame.BackgroundImage.Dispose();
            }
            catch { }

            pnlShotIngame.BackgroundImage = null;

            try
            {
                boxIngamePic.Dispose();
            }
            catch { }

            txt_BoxIngame.Text = "";
        }

        private void btnBoxFrontView_Click(object sender, EventArgs e)
        {
            String boxfront = ThumbsHandler.Instance.createGameArt(searchGame, "BoxFront", false);
            if (boxfront != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = boxfront;
                proc.Start();
            }
        }

        private void btnBoxBackView_Click(object sender, EventArgs e)
        {
            String BackCoverPath = ThumbsHandler.Instance.createGameArt(searchGame, "BoxBack", false);
            if (BackCoverPath != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = BackCoverPath;
                proc.Start();
            }
        }

        private void btnBoxTitleView_Click(object sender, EventArgs e)
        {
            String TitleScreenPath = ThumbsHandler.Instance.createGameArt(searchGame, "TitleScreenshot", false);
            if (TitleScreenPath != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = TitleScreenPath;
                proc.Start();
            }
        }

        private void btnBoxIngameView_Click(object sender, EventArgs e)
        {
            String IngameScreenPath = ThumbsHandler.Instance.createGameArt(searchGame, "IngameScreenshot", false);
            if (IngameScreenPath != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = IngameScreenPath;
                proc.Start();
            }
        }

        private void btnViewFanart_Click(object sender, EventArgs e)
        {
            String FanartPath = ThumbsHandler.Instance.createGameArt(searchGame, "Fanart", false);
            if (FanartPath != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = FanartPath;
                proc.Start();
            }
        }

        private void btnDeleteFanart_Click(object sender, EventArgs e)
        {
            try
            {
                pnlFanart.BackgroundImage.Dispose();
            }
            catch { }

            pnlFanart.BackgroundImage = null;

            try
            {
                boxFanart.Dispose();
            }
            catch { }

            txt_Fanart.Text = "";
        }

        void stopWorker()
        {
            if (workerThread != null)
            {
                if (workerThread.IsAlive)
                {
                    workerThread.Abort();
                    workerThread.Join();
                }
                workerThread = null;
            }
            btnSave.Enabled = true;
        }

        private void pnlBoxArt_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}