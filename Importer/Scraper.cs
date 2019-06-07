using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using Cornerstone.ScraperEngine;

namespace MyEmulators2.Import
{
    /// <summary>
    /// A wrapper for a specified script, provides
    /// methods to search and download info.
    /// </summary>
    public class Scraper
    {
        #region Ctor

        public Scraper(string script)
        {
            if (string.IsNullOrEmpty(script))
                return;

            //try and load a valid scraper
            Stream scriptStream;
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                scriptStream = asm.GetManifestResourceStream(script);
            }
            catch (Exception ex)
            {
                scriptStream = null;
                Logger.LogError("Error loading script {0} - {1}", script, ex.Message);
            }

            if (scriptStream == null)
                return;
            string scriptTxt = "";
            using (StreamReader sr = new StreamReader(scriptStream))
            {
                scriptTxt = sr.ReadToEnd();
            }

            scraper = new ScriptableScraper(scriptTxt, false);
            if (!scraper.LoadSuccessful)
            {
                Logger.LogError("Import_Scraper - Error loading script {0}", script);
                scraper = null;
                return; //problem with scraper
            }
            name = scraper.Name;
            idString = scraper.ID.ToString();
            isReady = true;
        }

        #endregion

        ScriptableScraper scraper = null;
        bool isReady = false;
        /// <summary>
        /// Is true if specified script was loaded successfully
        /// </summary>
        public bool IsReady
        {
            get { return isReady; }
        }

        string idString = "-1";
        public string IdString
        {
            get { return idString; }
        }

        string name = "";
        public string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            return name;
        }

        public int Priority { get; set; }

        public virtual List<ScraperResult> GetMatches(ScraperSearchParams searchParams)
        {
            return getMatches(searchParams);
        }

        public ScraperResult GetFirstMatch(ScraperSearchParams searchParams)
        {
            List<ScraperResult> results = getMatches(searchParams);
            if (results.Count > 0)
            {
                results.Sort();
                return results[0];
            }
            return null;
        }

        //execute scraper search using specified param's, return results
        List<ScraperResult> getMatches(ScraperSearchParams searchParams)
        {
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            if (searchParams.Term != null)
                paramList["search.title"] = searchParams.Term;
            if (searchParams.Platform != null)
                paramList["search.platform"] = searchParams.Platform;
            if (searchParams.PlatformId != null)
                paramList["search.platformid"] = searchParams.PlatformId;

            List<ScraperResult> results = new List<ScraperResult>();
            Dictionary<string, string> scraperResults = scraper.Execute("search", paramList);
            if (scraperResults == null)
                return results;

            int count = 0;
            //loop and build results
            while (scraperResults.ContainsKey("game[" + count + "].site_id"))
            {
                string siteId, title, yearmade, system, prefix = "game[" + count + "].";
                if (!scraperResults.TryGetValue(prefix + "site_id", out siteId))
                    continue;
                if (scraperResults.TryGetValue(prefix + "system", out system))
                    system = system.Trim();
                scraperResults.TryGetValue(prefix + "title", out title);
                scraperResults.TryGetValue(prefix + "yearmade", out yearmade);
                results.Add(new ScraperResult(siteId, title, system, yearmade, this) 
                { 
                    SearchParams = searchParams, 
                    SearchDistance = FuzzyStringComparer.Score(searchParams.Term, ScraperProvider.RemoveSpecialChars(title)) 
                });
                count++;
            }

            return results;
        }

        public ScraperGame GetDetails(ScraperResult result)
        {
            Dictionary<string, string> paramList = new Dictionary<string, string>();
            paramList["game.site_id"] = result.SiteId;
            Dictionary<string, string> results = scraper.Execute("get_details", paramList);

            if (results == null)
                return null;

            string grade, title, yearmade, company, description, genre;
            results.TryGetValue("game.grade", out grade);
            results.TryGetValue("game.title", out title);
            results.TryGetValue("game.yearmade", out yearmade);
            results.TryGetValue("game.company", out company);
            results.TryGetValue("game.description", out description);
            results.TryGetValue("game.genre", out genre);
            if (!string.IsNullOrEmpty(genre) && genre.StartsWith("|"))
                genre = genre.Remove(0, 1);

            return new ScraperGame(title, company, yearmade, grade, stripTags(description), genre);
        }

        public List<string> GetCoverUrls(ScraperResult result, out string coverFront, out string coverBack)
        {
            coverFront = null;
            coverBack = null;
            Dictionary<string, string> imageResults = scraper.Execute("get_cover_art", getDetailsParams(result.SiteId));
            if (imageResults == null)
                return new List<string>();

            string baseUrl; 
            imageResults.TryGetValue("game.baseurl", out baseUrl);

            if (imageResults.TryGetValue("game.cover.front", out coverFront) && !string.IsNullOrEmpty(coverFront))
                coverFront = expandUrl(coverFront, baseUrl);
            if (imageResults.TryGetValue("game.cover.back", out coverBack) && !string.IsNullOrEmpty(coverBack))
                coverBack = expandUrl(coverBack, baseUrl);

            List<string> urls = getImageUrls(imageResults, baseUrl);
            matchUrlToImageType(urls, ref coverFront, "front", ref coverBack, "back");
            return urls;
        }

        public List<string> GetScreenUrls(ScraperResult result, out string titleScreen, out string inGame)
        {
            titleScreen = null;
            inGame = null;
            Dictionary<string, string> imageResults = scraper.Execute("get_screenshots", getDetailsParams(result.SiteId));
            if (imageResults == null)
                return new List<string>();

            string baseUrl;
            imageResults.TryGetValue("game.baseurl", out baseUrl);

            if (imageResults.TryGetValue("game.screen.title", out titleScreen) && !string.IsNullOrEmpty(titleScreen))
                titleScreen = expandUrl(titleScreen, baseUrl);
            if (imageResults.TryGetValue("game.screem.ingame", out inGame) && !string.IsNullOrEmpty(inGame))
                inGame = expandUrl(inGame, baseUrl);

            List<string> urls = getImageUrls(imageResults, baseUrl);
            matchUrlToImageType(urls, ref titleScreen, "title", ref inGame, null);
            return urls;
        }

        public List<string> GetFanartUrls(ScraperResult result, out string fanart)
        {
            fanart = null;
            Dictionary<string, string> imageResults = scraper.Execute("get_fanart", getDetailsParams(result.SiteId));
            if (imageResults == null)
                return new List<string>();

            string baseUrl;
            imageResults.TryGetValue("game.baseurl", out baseUrl);
            List<string> results = getImageUrls(imageResults, baseUrl);
            if (results.Count > 0)
                fanart = results[0];
            return results;
        }

        List<string> getImageUrls(Dictionary<string, string> results, string baseUrl)
        {
            List<string> urls = new List<string>();
            string images;
            if (results.TryGetValue("game.images", out images))
            {
                string[] imageUrls = images.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < imageUrls.Length; x++)
                    if (!string.IsNullOrEmpty(imageUrls[x]))
                        urls.Add(expandUrl(imageUrls[x], baseUrl));
            }
            return urls;
        }

        void matchUrlToImageType(List<string> urls, ref string image1, string imageTag1, ref string image2, string imageTag2)
        {
            bool found1 = image1 != null;
            bool found2 = image2 != null;
            bool checkTag1 = !string.IsNullOrEmpty(imageTag1);
            bool checkTag2 = !string.IsNullOrEmpty(imageTag2);

            if(!found1 || !found2)
                for (int x = 0; x < urls.Count; x++)
                {
                    if (found1 && found2)
                        break;

                    string url = urls[x].ToLower();
                    if (checkTag1 && !found1 && url.Contains(imageTag1))
                    {
                        image1 = urls[x];
                        found1 = true;
                    }
                    else if (checkTag2 && !found2 && url.Contains(imageTag2))
                    {
                        image2 = urls[x];
                        found2 = true;
                    }
                    else if (image1 == null)
                        image1 = urls[x];
                    else if (image2 == null)
                        image2 = urls[x];
                }
        }

        Dictionary<string, string> getDetailsParams(string siteId)
        {
            Dictionary<string, string> detailsParams = new Dictionary<string, string>();
            detailsParams["game.site_id"] = siteId;
            return detailsParams;
        }

        string expandUrl(string partialUrl, string baseUrl)
        {
            if (!string.IsNullOrEmpty(baseUrl) && !string.IsNullOrEmpty(partialUrl) && !partialUrl.ToLower().StartsWith("http://"))
                return baseUrl + partialUrl;
            return partialUrl;
        }

        string stripTags(string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return "";

            HTML = HTML.Replace("<br>", System.Environment.NewLine);
            HTML = HTML.Replace("<br />", System.Environment.NewLine);
            HTML = HTML.Replace("<br/>", System.Environment.NewLine);

            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex("<[^>]*>");
            return objRegEx.Replace(HTML, "");
        }
    }
}
