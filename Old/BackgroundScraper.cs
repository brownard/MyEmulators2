using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cornerstone.ScraperEngine;
using System.Reflection;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using MediaPortal.GUI.Library;

namespace myEmulators
{
    //Provides asyncronous import methods

    public class BackgroundScraper
    {

        volatile bool stopWorker = false;

        public BackgroundScraper(GUIAnimation workingAnimation) 
        {
            this.workingAnimation = workingAnimation; //skin animation used to show activity
        }

        Thread workerThread = null;
        GUIAnimation workingAnimation = null;

        public delegate void refreshProgress(string status, int progress, bool completed);

        //Asyncronously download metadata for all or new roms in DB
        public void StartImport(bool onlyNew)
        {
            Logger.LogInfo("Background import starting...");

            Stop();

            Game[] games = DB.Instance.GetGames();

            if (stopWorker)
                return;


            List<Game> gamesToUpdate = new List<Game>();
            if (onlyNew)
            {
                foreach (Game item in games)
                {
                    if (item.Yearmade == 0) //assume new games are any where year is 0
                        gamesToUpdate.Add(item);
                }
            }
            else
                gamesToUpdate.AddRange(games);

            Logger.LogInfo("Found {0} games to update", gamesToUpdate.Count);
            if (gamesToUpdate.Count == 0) //no items to update
            {
                Logger.LogInfo("Background import finished");
                return;
            }

            //try and load a valid scraper
            Assembly asm = Assembly.GetExecutingAssembly();
            string script = "";
            using (StreamReader sr = new StreamReader(asm.GetManifestResourceStream("myEmulators.Scripts.Mobygames.xml")))
            {
                script = sr.ReadToEnd();
            }
            ScriptableScraper scraper = new ScriptableScraper(script, false);
            if (!scraper.LoadSuccessful)
            {
                Logger.LogError("Error loading scraper");
                scraper = null;
                return; //problem with scraper, stop import
            }


            System.Data.DataTable dt = Dropdowns.GetSystems(); //get a list of systems to match against parent emulator

            if (workingAnimation != null)
                workingAnimation.Visible = true;

            workerThread = new Thread(new ThreadStart(delegate()
                {

                    EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                    ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()[1];
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = qualityParam;
                    Bitmap gameArt = null;
                    bool aborted = false;



                    //split into groups of 3 to allow partial update of gui
                    int groups = (int)Math.Ceiling(gamesToUpdate.Count / 3.0);
                    List<Game>[] gameGroups = new List<Game>[groups];

                    for (int x = 0; x < groups; x++) //add items to a group
                    {

                        List<Game> gameGroup = new List<Game>();
                        if (x == groups - 1) //last group may contain less than 3 items
                        {
                            for (int y = 3 * x; y < gamesToUpdate.Count; y++)
                                gameGroup.Add(gamesToUpdate[y]);
                        }
                        else
                        {
                            int y = 3 * x;
                            gameGroup.AddRange(new Game[] { gamesToUpdate[y], gamesToUpdate[y + 1], gamesToUpdate[y + 2] });
                        }
                        gameGroups[x] = gameGroup;
                    }


                    if (stopWorker)
                        return;

                    try
                    {
                        foreach (List<Game> gameGroup in gameGroups)
                        {
                            //partial results
                            List<Game> gamesToReturn = new List<Game>();

                            foreach (Game item in gameGroup)
                            {
                                if (stopWorker)
                                    return;

                                string SavePath = ThumbsHandler.Instance.thumb_games + @"\" + item.ParentEmulator.Title + @"\" + item.GameID.ToString();
                                if (!Directory.Exists(SavePath))
                                    Directory.CreateDirectory(SavePath);
                                
                                string searchString = StripRomCodes(item.Title);
                                string searchSystem = "";
                                Dictionary<string, string> paramList = new Dictionary<string, string>();
                                paramList["search.title"] = searchString;

                                //try and match emulator title against predefined systems to narrow search
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    if ((row[1] as string).ToLower() == item.ParentEmulator.Title.ToLower() || (item.ParentEmulator.isPc() && row[1] as string == "Windows"))
                                    {
                                        //Console match
                                        paramList["search.system"] = row[0] as string;
                                        searchSystem = row[1] as string;
                                        break;
                                    }
                                }

                                if (stopWorker)
                                    return;
                                //get matches and choose best result based on search settings
                                List<ScraperResult> results = Conf_Scraper.getSearchResults(scraper, paramList);

                                int possibleTitleMatches = 0;
                                int possibleSystemMatches = 0;
                                string titleSiteId = "";
                                string systemSiteId = "";
                                bool isExact = false;

                                foreach (ScraperResult result in results)
                                {
                                    if (stopWorker)
                                        return;

                                    if (result.Title == searchString && result.System == searchSystem)
                                    {
                                        //exact match break and get details
                                        isExact = true;
                                        possibleTitleMatches = 1;
                                        titleSiteId = result.SiteId;
                                        break;
                                    }
                                    else if (result.Title == searchString)
                                    {
                                        if (possibleTitleMatches == 0)
                                            titleSiteId = result.SiteId;
                                        possibleTitleMatches++;
                                    }
                                    else if (result.System == searchSystem)
                                    {
                                        if (possibleSystemMatches == 0)
                                            systemSiteId = result.SiteId;
                                        possibleSystemMatches++;
                                    }
                                }

                                bool importTop = Options.Instance.GetBoolOption("importtop");
                                bool importExact = Options.Instance.GetBoolOption("importexact");

                                string matchSiteId = "";
                                if (possibleTitleMatches == 1 && (isExact || !importExact))
                                    matchSiteId = titleSiteId;
                                else if (possibleSystemMatches == 1 && !importExact)
                                    matchSiteId = systemSiteId;
                                else if (results.Count > 0 && importTop)
                                    matchSiteId = results[0].SiteId;

                                if (matchSiteId == "") //no match, skip
                                    continue;

                                //get game details
                                paramList = new Dictionary<string, string>();
                                paramList["game.site_id"] = matchSiteId;

                                if (stopWorker)
                                    return;

                                ScraperGame scraperGame = Conf_Scraper.getGame(scraper, paramList);

                                item.Title = scraperGame.Title;
                                item.Description = StripTags(scraperGame.Description);
                                item.Company = scraperGame.Company;
                                try
                                {
                                    item.Yearmade = Convert.ToInt32(scraperGame.Year);
                                }
                                catch
                                {
                                    item.Yearmade = 0;
                                }
                                try
                                {
                                    item.Grade = Convert.ToInt32(Math.Round((Convert.ToDouble(scraperGame.Grade, System.Globalization.CultureInfo.InvariantCulture) * 2), 0));
                                }
                                catch
                                {
                                    item.Grade = 0;
                                }

                                try
                                {
                                    item.Genre = scraperGame.Genre.Split('|')[1];
                                }
                                catch
                                {
                                    item.Genre = "";
                                }

                                item.Save();

                                if (stopWorker)
                                    return;

                                double thumbDimensions = 0;
                                if (Options.Instance.GetBoolOption("resizethumbs"))
                                    thumbDimensions = ImageHandler.Instance.GetCaseAspect(item.ParentEmulator.Title); //the aspect ratio used to resize thumbs

                                //get covers
                                List<string> thumbs = Conf_Scraper.getCoverUrls(scraper, paramList); //getCoverUrls handles image match logic and will return max of 2 results

                                if (thumbs[0] != "") //front cover
                                {
                                    try
                                    {
                                        gameArt = ImageHandler.Instance.BitmapFromWeb(thumbs[0]);
                                        if (Options.Instance.GetBoolOption("resizethumbs")) //resize image if necessary
                                        {
                                            using (Image resizedThumb = ImageHandler.Instance.resizeImage(gameArt, thumbDimensions))
                                            {
                                                resizedThumb.Save(SavePath + @"\BoxFront.jpg", jpegCodec, encoderParams);
                                            }
                                        }
                                        else
                                            gameArt.Save(SavePath + @"\BoxFront.jpg", jpegCodec, encoderParams);
                                    }
                                    catch { }
                                    finally
                                    {
                                        if (gameArt != null)
                                        {
                                            gameArt.Dispose();
                                            gameArt = null;
                                        }
                                    }
                                }
                                if (thumbs[1] != "") //back cover
                                {
                                    try
                                    {
                                        gameArt = ImageHandler.Instance.BitmapFromWeb(thumbs[1]);
                                        if (Options.Instance.GetBoolOption("resizethumbs"))
                                        {
                                            using (Image resizedThumb = ImageHandler.Instance.resizeImage(gameArt, thumbDimensions))
                                            {
                                                resizedThumb.Save(SavePath + @"\BoxBack.jpg", jpegCodec, encoderParams);
                                            }
                                        }
                                        else
                                            gameArt.Save(SavePath + @"\BoxBack.jpg", jpegCodec, encoderParams);
                                    }
                                    catch { }
                                    finally
                                    {
                                        if (gameArt != null)
                                        {
                                            gameArt.Dispose();
                                            gameArt = null;
                                        }
                                    }
                                }

                                if (stopWorker)
                                    return;

                                //get screens
                                thumbs = Conf_Scraper.getScreenUrls(scraper, paramList);

                                if (thumbs[0] != "") //title screenshot
                                {
                                    try
                                    {
                                        gameArt = ImageHandler.Instance.BitmapFromWeb(thumbs[0]);
                                        gameArt.Save(SavePath + @"\TitleScreenshot.jpg", jpegCodec, encoderParams);
                                    }
                                    catch { }
                                    finally
                                    {
                                        if (gameArt != null)
                                        {
                                            gameArt.Dispose();
                                            gameArt = null;
                                        }
                                    }
                                }
                                if (thumbs[1] != "") //ingame screenshot
                                {
                                    try
                                    {
                                        gameArt = ImageHandler.Instance.BitmapFromWeb(thumbs[1]);
                                        gameArt.Save(SavePath + @"\IngameScreenshot.jpg", jpegCodec, encoderParams);
                                    }
                                    catch { }
                                    finally
                                    {
                                        if (gameArt != null)
                                        {
                                            gameArt.Dispose();
                                            gameArt = null;
                                        }
                                    }
                                }

                                gamesToReturn.Add(item);
                            }
                            //Update DB and GUI
                            MediaPortal.GUI.Library.GUIWindowManager.SendThreadMessage(new MediaPortal.GUI.Library.GUIMessage() { TargetWindowId = 2497, SendToTargetWindow = true, Object = gamesToReturn });
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        Thread.ResetAbort();
                        aborted = true;
                    }
                    finally
                    {
                        object msgObject = aborted ? ScraperState.Aborted : ScraperState.Finished;
                        //send finished message to GUI
                        MediaPortal.GUI.Library.GUIWindowManager.SendThreadMessage(new MediaPortal.GUI.Library.GUIMessage() { TargetWindowId = 2497, SendToTargetWindow = true, Object = msgObject });

                        try
                        {
                            gameArt.Dispose();
                        }
                        catch { }
                    }
                }
                ));
            workerThread.Start();
        }

        public void Stop()
        {
            Stop(true);
        }

        public void Stop(bool wait)
        {
            if (workerThread != null)
            {
                if (workerThread.IsAlive)
                {
                    stopWorker = true;
                    //workerThread.Abort();
                    workerThread.Join();
                    stopWorker = false;
                }
                workerThread = null;
            }
        }

        public void StartRefresh(refreshProgress progress)
        {
            Stop();

            workerThread = new Thread(new ThreadStart(delegate()
            {
                try
                {

                    List<string> dbPaths = DB.Instance.GetAllGamePaths();
                    Emulator[] emus = DB.Instance.GetEmulators();

                    if (stopWorker)
                        return;

                    Dictionary<Emulator, string[]> allNewPaths = new Dictionary<Emulator, string[]>();
                    int filesFound = 0;
                    progress.Invoke(string.Format("Refreshing: found {0} new files", filesFound), 0, false);
                    //GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Refreshing: found {0} new files", filesFound), 0, false });

                    foreach (Emulator emu in emus)
                    {
                        if (stopWorker)
                            return;

                        string romDir = emu.PathToRoms;
                        if (!Directory.Exists(romDir))
                        {
                            continue; //rom directory doesn't exist, skip
                        }

                        List<string> newPaths = new List<string>();

                        foreach (string filter in emu.Filter.Split(';'))
                        {
                            if (stopWorker)
                                return;

                            string[] gamePaths;
                            try
                            {
                                gamePaths = Directory.GetFiles(romDir, filter, SearchOption.AllDirectories); //get list of matches
                            }
                            catch
                            {
                                continue; //error with filter, skip
                            }


                            for (int x = 0; x < gamePaths.Length; x++)
                            {
                                if (stopWorker)
                                    return;

                                if (!dbPaths.Contains(gamePaths[x]) && !newPaths.Contains(gamePaths[x])) //check that path is not already in DB
                                {
                                    newPaths.Add(gamePaths[x]);
                                    filesFound++;
                                    progress.Invoke(string.Format("Refreshing: found {0} new files", filesFound), 0, false);
                                    //GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Refreshing: found {0} new files", filesFound), 0, false });
                                }
                            }
                        }

                        if (newPaths.Count > 0)
                            allNewPaths.Add(emu, newPaths.ToArray());

                    }

                    int filesAdded = 0;
                    foreach (KeyValuePair<Emulator, string[]> val in allNewPaths)
                    {
                        foreach (string path in val.Value)
                        {
                            if (stopWorker)
                                return;

                            DB.Instance.AddGame(path, val.Key); //add new rom to DB
                            filesAdded++;
                            int perc = (int)Math.Round(((double)filesAdded / filesFound) * 100);
                            progress.Invoke(string.Format("Refreshing: adding {0} / {1} new files", filesAdded, filesFound), perc, false);
                            //GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Refreshing: adding {0} / {1} new files", filesAdded, filesFound), perc, false });
                         }
                    }
                    progress.Invoke(string.Format("Refresh complete: added {0} new files", filesAdded), 100, true);
                    //GUIGraphicsContext.form.BeginInvoke(progress, new object[] { string.Format("Refresh complete: added {0} new files", filesAdded), 100, true });
                 }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                    progress.Invoke("Refresh aborted", 100, true);
                    //GUIGraphicsContext.form.BeginInvoke(progress, new object[] { "Refresh aborted", 100, true });
                }
            }
            ));
            workerThread.Start();
        }

        public void StartClean(refreshProgress progress)
        {
            Stop();

            workerThread = new Thread(new ThreadStart(delegate()
                {
                    try
                    {
                        Game[] dbGames = DB.Instance.GetGames("", true, true);

                        if (stopWorker)
                            return;

                        int gamesCount = dbGames.Length;
                        int gamesRemoved = 0;

                        GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Cleaning: checking rom {0} / {1}", 0, gamesCount), 0, false });
                        for (int x = 0; x < gamesCount; x++)
                        {
                            if (stopWorker)
                                return;

                            int perc = (int)Math.Round(((double)x / (gamesCount + 1)) * 100);
                            GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Cleaning: checking rom {0} / {1}", x, gamesCount), perc, false });

                            Game game = dbGames[x];

                            if (!File.Exists(game.Path) || game.ParentEmulator == null) //rom doesn't exist or is orphan
                            {
                                GUIGraphicsContext.form.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
                                {
                                    Logger.LogDebug("Removing '{0}' from database", game.Title);
                                    GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Cleaning: removing rom {0} / {1}", x, gamesCount), perc, false });
                                }
                                ));

                                game.Delete(); //remove rom from DB
                                gamesRemoved++;
                            }

                            else if (game.ParentEmulator.UID > -1)
                            {
                                bool remove = true;
                                if (game.Path.StartsWith(game.ParentEmulator.PathToRoms)) //is rom in rom directory?
                                    foreach (string extension in game.ParentEmulator.Filter.Split(';'))
                                        if (extension.EndsWith(Path.GetExtension(game.Path)) || extension == "*.*") //does rom end with valid extension?
                                        {
                                            remove = false;
                                            break;
                                        }
                                if (remove)
                                {
                                    //rom isn't in rom directory or doesn't have a valid extension
                                    GUIGraphicsContext.form.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
                                    {
                                        Logger.LogDebug("Removing '{0}' from database", game.Title);
                                    }
                                    ));

                                    game.Delete(); //remove rom from DB
                                    gamesRemoved++;
                                }
                            }
                            
                        }

                        GUIGraphicsContext.form.Invoke(progress, new object[] { string.Format("Clean complete: removed {0} roms", gamesRemoved), 100, true });
                    }
                    catch (ThreadAbortException)
                    {
                        Thread.ResetAbort();
                        GUIGraphicsContext.form.BeginInvoke(progress, new object[] { "Clean aborted", 100, true });
                    }
                }
                ));

            workerThread.Start();
        }

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

    }

    public enum ScraperState
    {
        Finished,
        Aborted
    }
}
