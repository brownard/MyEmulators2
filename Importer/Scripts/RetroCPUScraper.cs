using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace MyEmulators2
{
    class RetroCPUScraper : Import_Scraper
    {
        string retroMameUrl = "http://www.retrocpu.com/mame/roms/";
        public RetroCPUScraper()
            : base("retrocpu.com", "99999997")
        {
        }

        object cacheSync = new object();
        string htmlCache = null;

        public override List<ScraperResult> GetMatches(Dictionary<string, string> paramList)
        {
            List<ScraperResult> results = new List<ScraperResult>();
            string platform;
            if (!paramList.TryGetValue("search.platform", out platform))
                return results;
            if (platform != "Arcade" && platform != "Unspecified")
                return results;

            string searchString;
            if (!paramList.TryGetValue("search.title", out searchString))
                return results;

            string lHtml;
            lock (cacheSync)
            {
                if (string.IsNullOrEmpty(htmlCache))
                    htmlCache = getHtml(retroMameUrl);
                lHtml = htmlCache;
            }

            string fileReg = @"<strong>&raquo;</strong> <a href=""([^""]*)"">([^<]*)</a> <small style=""color: silver;"">[(][^;]*{0}[^;]*; (\d+)[^)]*[)]</small><br/>";
            string titleReg = @"<strong>&raquo;</strong> <a href=""([^""]*)"">([^<]*{0}[^<]*)</a> <small style=""color: silver;"">[(][^;]*; (\d+)[^)]*[)]</small><br/>";

            foreach (Match m in new Regex(string.Format(fileReg, searchString)).Matches(lHtml))
                results.Add(new ScraperResult("http://www.retrocpu.com" + m.Groups[1].Value, m.Groups[2].Value, "Arcade", m.Groups[3].Value, this) { SearchParams = paramList });

            foreach (Match m in new Regex(string.Format(titleReg, searchString)).Matches(lHtml))
                results.Add(new ScraperResult("http://www.retrocpu.com" + m.Groups[1].Value, m.Groups[2].Value, "Arcade", m.Groups[3].Value, this) { SearchParams = paramList });

            return results;
        }

        public override ScraperGame DownloadInfo(ScraperResult selectedMatch)
        {
            string detailsGame = getHtml(selectedMatch.SiteId);

            string rx_title = @"<tr><td><strong>Description:</strong></td><td>([^<]*)</td></tr>";
            string title = "";
            string rx_company = @"<tr><td><strong>Manufacturer:</strong></td><td>([^<]*)</td></tr>";
            string company = "";
            string rx_yearmade = @"<tr><td><strong>Year:</strong></td><td>(\d+)</td></tr>";
            string yearmade = "";

            Match m;
            if ((m = new Regex(rx_title).Match(detailsGame)).Success)
                title = m.Groups[1].Value;
            if ((m = new Regex(rx_company).Match(detailsGame)).Success)
                company = m.Groups[1].Value;
            if ((m = new Regex(rx_yearmade).Match(detailsGame)).Success)
                yearmade = m.Groups[1].Value;

            ScraperGame game = new ScraperGame(title, company, yearmade, "", "", "");

            string rx_flyer = @"<h3>Game Flyer</h3>\s*<div[^>]*>\s*<img src=""([^""]*)";
            string rx_marquee = @"<h3>Game Marquee</h3>\s*<div[^>]*>\s*<img src=""([^""]*)";
            string rx_cabinet = @"<h3>Game Cabinet</h3>\s*<div[^>]*>\s*<img src=""([^""]*)";

            string rx_screen = @"<span class=""gameplatform""><a href=""/mame/roms/"" title=""View all MAME ROMs"">MAME ROMs</a></span>\s*<img src=""([^""]*)";
            bool success = true;

            if ((m = new Regex(rx_flyer).Match(detailsGame)).Success)
                game.BoxFront = getAndCheckImage("http://www.retrocpu.com" + m.Groups[1].Value, ref success);
            if ((m = new Regex(rx_marquee).Match(detailsGame)).Success)
                game.BoxBack = getAndCheckImage("http://www.retrocpu.com" + m.Groups[1].Value, ref success);
            if ((m = new Regex(rx_cabinet).Match(detailsGame)).Success)
                game.TitleScreen = getAndCheckImage("http://www.retrocpu.com" + m.Groups[1].Value, ref success);
            if ((m = new Regex(rx_screen).Match(detailsGame)).Success)
                game.InGame = getAndCheckImage("http://www.retrocpu.com" + m.Groups[1].Value, ref success);

            return game;
        }

        string getHtml(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 6.0; en-US)";
            request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
                    return reader.ReadToEnd();
            }

            return "";
        }
    }
}
