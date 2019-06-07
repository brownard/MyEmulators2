using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using Cornerstone.ScraperEngine;

namespace myEmulators
{
    class Conf_Scraper
    {
        //Background scraper for use in Configuration

        public static List<ScraperResult> getSearchResults(ScriptableScraper scraper, Dictionary<string, string> paramList)
        {
            List<ScraperResult> results = new List<ScraperResult>();
            if (scraper == null)
                return results;

            Dictionary<string, string> scraperResults = scraper.Execute("search", paramList);
            if (scraperResults == null)
            {
                return results;
            }

            string respage = "";

            scraperResults.TryGetValue("search_page", out respage);
            int count = 0;
            // The movie result is only valid if the script supplies a unique site

            while (scraperResults.ContainsKey("game[" + count + "].site_id"))
            {
                string siteId;
                string title;
                string yearmade;
                string system;
                string xsiteId;
                string xgamePattern;
                string prefix = "game[" + count + "].";
                count++;


                if (!scraperResults.TryGetValue(prefix + "site_id", out siteId))
                    continue;

                if (scraperResults.TryGetValue(prefix + "system", out system))
                    system = system.Trim();
                scraperResults.TryGetValue(prefix + "title", out title);
                scraperResults.TryGetValue(prefix + "yearmade", out yearmade);
                scraperResults.TryGetValue(prefix + "extraGameUrl", out xsiteId);
                scraperResults.TryGetValue(prefix + "extraGamePattern", out xgamePattern);

                if (!string.IsNullOrEmpty(xsiteId))
                    siteId = xsiteId;

                results.Add(new ScraperResult(siteId, title, system, yearmade));

                //additional field added as mobygames combines results from different platforms so we need
                //to split them to get the correct details/thumbs for our platform
                if (!string.IsNullOrEmpty(xgamePattern))
                {
                    //split combined results from mobygames
                    foreach (Match match in new Regex(@"<a href=""/game/([^""]*)"">([^<]*)</a> \(<em>(\d{4})").Matches(xgamePattern))
                    {
                        results.Add(new ScraperResult(match.Groups[1].Value, title, match.Groups[2].Value, match.Groups[3].Value));
                    }
                }
            }

            return results;
        }

        public static ScraperGame getGame(ScriptableScraper scraper, Dictionary<string, string> paramList)
        {
            if (scraper == null)
                return null;

            Dictionary<string, string> results = scraper.Execute("get_details", paramList);
            if (results == null)
            {
                return null;
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

            return new ScraperGame(title, company, yearmade, grade, description, genre);
        }

        
        public static List<Bitmap> getCovers(ScriptableScraper scraper, Dictionary<string, string> paramList, out Bitmap front, out Bitmap back, Conf_OnlineLookup.onProgress progress, int progressStart, int progressEnd, bool unattended)
        {
            List<Bitmap> imageResults = new List<Bitmap>();
            front = null;
            back = null;

            if (scraper == null)
                return imageResults;

            Dictionary<string, string> results = scraper.Execute("get_cover_art", paramList);

            if (results == null)
            {
                return imageResults;
            }

            string covers = "";

            if (results.TryGetValue("game.covers", out covers))
            {
                string[] coverurls = covers.Split('|');

                //calculate percentage increment for progress bar based on number of results
                int increment = progressEnd - progressStart;
                if (coverurls.Length > 0)
                    increment = (int)Math.Floor(increment / (double)coverurls.Length);

                for (int i = 1; i < coverurls.Length; i++)
                {
                    progressStart += increment;
                    progress.Invoke(string.Format("Getting cover art {0}/{1}...", i.ToString(), coverurls.Length.ToString()), progressStart);

                    bool isFront = false;
                    bool isBack = false;
                    //Bitmap pic = ImageHandler.Instance.BitmapFromWeb(coverurls[i]);
                    //imageResults.Add(pic);

                    if (i == 1)
                    {
                        isFront = true;
                    }
                    else if (i == 2)
                    {
                        isBack = true;
                    }

                    if (unattended)
                    {
                        if (isFront)
                        {
                            front = ImageHandler.Instance.BitmapFromWeb(coverurls[i]);
                        }
                        else if (isBack)
                        {
                            back = ImageHandler.Instance.BitmapFromWeb(coverurls[i]);
                        }
                        //no point in getting additional thumbs as import is unattended
                        if (front != null && back != null)
                            break;
                    }
                    else
                    {
                        Bitmap thePic = ImageHandler.Instance.BitmapFromWeb(coverurls[i]);
                        imageResults.Add(thePic);
                        if (isFront)
                        {
                            front = thePic;
                        }
                        else if (isBack)
                        {
                            back = thePic;
                        }
                    }
                }
            }

            return imageResults;
        }

        public static List<string> getCoverUrls(ScriptableScraper scraper, Dictionary<string, string> paramList)
        {
            List<string> imageResults = new List<string>();
            imageResults.Add("");
            imageResults.Add("");
            if (scraper == null)
                return imageResults;

            Dictionary<string, string> results;

            results = scraper.Execute("get_cover_art", paramList);

            if (results == null)
            {
                return imageResults;
            }

            string covers = "";

            if (results.TryGetValue("game.covers", out covers))
            {
                string[] coverurls = covers.Split('|');

                for (int i = 1; i < coverurls.Length; i++)
                {
                    if (i == 1)
                    {
                        imageResults[0] = coverurls[i];
                    }
                    else if (i == 2)
                    {
                        imageResults[1] = coverurls[i];
                    }
                }
            }
            return imageResults;
        }

        public static List<Bitmap> getScreens(ScriptableScraper scraper, Dictionary<string, string> paramList, out Bitmap title, out Bitmap inGame, Conf_OnlineLookup.onProgress progress, int progressStart, int progressEnd, bool unattended)
        {
            List<Bitmap> imageResults = new List<Bitmap>();
            title = null;
            inGame = null;

            if (scraper == null)
                return imageResults;

            Dictionary<string, string> results;

            results = scraper.Execute("get_screenshots", paramList);

            if (results == null)
            {
                return imageResults;
            }

            string screens = "";
            string screenshotsbaseurl = "";

            results.TryGetValue("game.screenshotsbaseurl", out screenshotsbaseurl);

            if (results.TryGetValue("game.screenshots", out screens))
            {
                string[] screenurls = screens.Split('|');
                bool foundTitleScreen = false;
                bool foundInGame = false;

                int increment = progressEnd - progressStart;
                if (screenurls.Length > 0)
                    increment = (int)Math.Floor(increment / (double)screenurls.Length);

                for (int i = 1; i < screenurls.Length; i++)
                {
                    progressStart += increment;
                    progress.Invoke(string.Format("Getting screenshot {0}/{1}...", i.ToString(), screenurls.Length.ToString()), progressStart);

                    if (!screenurls[i].ToLower().StartsWith("http://"))
                    {
                        screenurls[i] = screenshotsbaseurl + screenurls[i];
                    }

                    bool isTitle = false;
                    bool isInGame = false;

                    if (!foundTitleScreen || !foundInGame)
                    {
                        if (screenurls[i].Contains("screenshot-start-screen"))
                        {
                            isTitle = true;
                            foundTitleScreen = true;
                        }
                        else if (screenurls[i].Contains("screenshot-title-screen"))
                        {
                            if (!foundTitleScreen)
                            {
                                isTitle = true;
                            }
                        }
                        else if (title == null)
                        {
                            isTitle = true;
                        }
                        else if (inGame == null)
                        {
                            isInGame = true;
                            foundInGame = true;
                        }
                    }

                    if (unattended) //only download image if we need to
                    {
                        if (isTitle)
                        {
                            title = ImageHandler.Instance.BitmapFromWeb(screenurls[i]);
                        }
                        else if (isInGame)
                        {
                            inGame = ImageHandler.Instance.BitmapFromWeb(screenurls[i]);
                        }
                        if (foundInGame && foundTitleScreen) //best matches found, no point in continuing search
                            break;
                    }
                    else //if not unattended then download all images to allow user selection
                    {
                        Bitmap thePic = ImageHandler.Instance.BitmapFromWeb(screenurls[i]);
                        imageResults.Add(thePic);
                        if (isTitle)
                        {
                            title = thePic;
                        }
                        else if (isInGame)
                        {
                            inGame = thePic;
                        }
                    }
                }
            }

            return imageResults;
        }

        public static List<string> getScreenUrls(ScriptableScraper scraper, Dictionary<string, string> paramList)
        {
            List<string> imageResults = new List<string>();
            imageResults.Add("");
            imageResults.Add("");

            if (scraper == null)
                return imageResults;

            Dictionary<string, string> results;

            results = scraper.Execute("get_screenshots", paramList);

            if (results == null)
            {
                return imageResults;
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
                    if (foundTitleScreen && foundInGame)
                        break;
                    if (!screenurls[i].ToLower().StartsWith("http://"))
                    {
                        screenurls[i] = screenshotsbaseurl + screenurls[i];
                    }


                    if (screenurls[i].Contains("screenshot-start-screen"))
                    {
                        imageResults[0] = screenurls[i];
                        foundTitleScreen = true;
                    }
                    else if (screenurls[i].Contains("screenshot-title-screen"))
                    {
                        if (!foundTitleScreen)
                        {
                            imageResults[0] = screenurls[i];
                        }
                    }
                    else if (imageResults[0] == "")
                    {
                        imageResults[0] = screenurls[i];
                    }
                    else if (imageResults[1] == "")
                    {
                        imageResults[1] = screenurls[i];
                        foundInGame = true;
                    }
                }
            }

            return imageResults;
        }
    }

}
