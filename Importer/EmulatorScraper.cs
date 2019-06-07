using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;

namespace MyEmulators2
{
    class EmulatorScraper
    {
        public Dictionary<string, string> GetEmulators(string platform, out string selectedKey)
        {
            selectedKey = null;
            Dictionary<string, string> items = new Dictionary<string, string>();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("http://thegamesdb.net/api/GetPlatformsList.php");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving online platform info - {0}", ex.Message);
                return items;
            }

            foreach (XmlNode xmlPlatform in doc.GetElementsByTagName("Platform"))
            {
                XmlNode a = xmlPlatform.SelectSingleNode("./id");
                if (a != null)
                {
                    string id = a.InnerXml;
                    a = xmlPlatform.SelectSingleNode("./name");
                    if (a != null)
                        items.Add(a.InnerXml, id);
                }
            }

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(platformConvert, string.Format("[|]([^\n,]*),{0}[|]", platform));
            if (m.Success)
                platform = m.Groups[1].Value;

            if (items.ContainsKey(platform))
                selectedKey = platform;

            return items;
        }

        public EmulatorInfo GetInfo(string platformId)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("http://thegamesdb.net/api/GetPlatform.php?id=" + platformId);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving online platform info - {0}", ex.Message);
                return null;
            }

            XmlNode platform = doc.SelectSingleNode("Data/Platform");
            if (platform == null)
                return null;

            EmulatorInfo emuInfo = new EmulatorInfo();

            XmlNode a = platform.SelectSingleNode("./Platform");
            if (a != null)
                emuInfo.Title = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./developer");
            if (a != null)
                emuInfo.Developer = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./Rating");
            if (a != null)
                emuInfo.Grade = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./overview");
            if (a != null)
                emuInfo.Overview = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./cpu");
            if (a != null)
                emuInfo.CPU = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./memory");
            if (a != null)
                emuInfo.Memory = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./graphics");
            if (a != null)
                emuInfo.Graphics = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./sound");
            if (a != null)
                emuInfo.Sound = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./display");
            if (a != null)
                emuInfo.Display = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./media");
            if (a != null)
                emuInfo.Media = HttpUtility.HtmlDecode(a.InnerText);

            a = platform.SelectSingleNode("./maxcontrollers");
            if (a != null)
                emuInfo.MaxControllers = HttpUtility.HtmlDecode(a.InnerText);

            string baseImageUrl = "";
            a = doc.SelectSingleNode("Data/baseImgUrl");
            if (a != null)
                baseImageUrl = a.InnerXml;

            a = platform.SelectSingleNode("./Images/fanart/original");
            if (a != null)
                emuInfo.FanartUrl = baseImageUrl + a.InnerXml;

            a = platform.SelectSingleNode("./Images/boxart");
            if (a != null)
                emuInfo.LogoUrl = baseImageUrl + a.InnerXml;

            return emuInfo;
        }

        public void GetThumbs(string platformId, out IEnumerable<string> covers, out IEnumerable<string> fanart)
        {
            covers = null;
            fanart = null;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("http://thegamesdb.net/api/GetPlatform.php?id=" + platformId);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving online platform info - {0}", ex.Message);
                return;
            }

            string baseImageUrl = "";
            XmlNode a = doc.SelectSingleNode("Data/baseImgUrl");
            if (a != null)
                baseImageUrl = a.InnerXml;

            XmlNode images = doc.SelectSingleNode("Data/Platform/Images");
            if (images == null)
                return;

            List<string> lCovers = new List<string>();
            List<string> lFanart = new List<string>();
            addImages(images.SelectNodes("./boxart"), lCovers, baseImageUrl);
            addImages(images.SelectNodes("./banner"), lCovers, baseImageUrl);
            addImages(images.SelectNodes("./consoleart"), lCovers, baseImageUrl);
            addImages(images.SelectNodes("./controllerart"), lCovers, baseImageUrl);
            addImages(images.SelectNodes("./fanart/original"), lFanart, baseImageUrl);
            covers = lCovers;
            fanart = lFanart;
        }

        void addImages(XmlNodeList imageNodes, List<string> results, string baseImageUrl)
        {
            for (int x = 0; x < imageNodes.Count; x++)
                results.Add(baseImageUrl + imageNodes[x].InnerXml);
        }

        string platformConvert = @"
            |3DO,3DO|
            |Amiga,Amiga|
            |Arcade,Arcade|
            |Atari 2600,Atari 2600|
            |Atari 5200,Atari 5200|
            |Atari 7800,Atari 7800|
            |Atari Jaguar,Atari Jaguar|
            |Atari Jaguar CD,Atari Jaguar CD|
            |Atari XE,Atari XE|
            |Colecovision,Colecovision|
            |Commodore 64,Commodore 64|
            |Intellivision,Intellivision|
            |Mac OS,Macintosh|
            |Microsoft Xbox,Xbox|
            |Microsoft Xbox 360,Xbox 360|
            |NeoGeo,NeoGeo|
            |Nintendo 64,Nintendo 64|
            |Nintendo DS,Nintendo DS|
            |Nintendo Entertainment System (NES),NES|
            |Nintendo Game Boy,Game Boy|
            |Nintendo Game Boy Advance,Game Boy Advance|
            |Nintendo Game Boy Color,Game Boy Color|
            |Nintendo GameCube,GameCube|
            |Nintendo Wii,Wii|
            |Nintendo Wii U,Wii U|
            |PC,Windows|
            |Sega 32X,SEGA 32X|
            |Sega CD,SEGA CD|
            |Sega Dreamcast,Dreamcast|
            |Sega Game Gear,Game Gear|
            |Sega Genesis,Genesis|
            |Sega Master System,SEGA Master System|
            |Sega Mega Drive,SEGA Mega Drive|
            |Sega Saturn,SEGA Saturn|
            |Sony Playstation,Playstation|
            |Sony Playstation 2,Playstation 2|
            |Sony Playstation 3,Playstation 3|
            |Sony Playstation Vita,Playstation Vita|
            |Sony PSP,PSP|
            |Super Nintendo (SNES),SNES|
            |TurboGrafx 16,TurboGrafx-16|";
    }

    class EmulatorInfo
    {
        public string Title { get; set; }
        public string Developer { get; set; }
        public string Grade { get; set; }

        public string FanartUrl { get; set; }
        public string LogoUrl { get; set; }

        public string Overview
        {
            get;
            set;
        }
        public string CPU { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string Sound { get; set; }
        public string Display { get; set; }
        public string Media { get; set; }
        public string MaxControllers { get; set; }

        public string GetDescription()
        {
            string format = "{0}: {1}\r\n";
            string info = "";
            if (!string.IsNullOrEmpty(CPU))
                info += string.Format(format, "CPU", CPU);

            if (!string.IsNullOrEmpty(Memory))
                info += string.Format(format, "Memory", Memory);

            if (!string.IsNullOrEmpty(Graphics))
                info += string.Format(format, "Graphics", Graphics);

            if (!string.IsNullOrEmpty(Sound))
                info += string.Format(format, "Sound", Sound);

            if (!string.IsNullOrEmpty(Display))
                info += string.Format(format, "Display", Display);

            if (!string.IsNullOrEmpty(Media))
                info += string.Format(format, "Media", Media);

            if (!string.IsNullOrEmpty(MaxControllers))
                info += string.Format(format, "Max Controllers", MaxControllers);

            if (!string.IsNullOrEmpty(Overview))
                info = Overview + "\r\n\r\n" + info;

            return info.Trim();
        }
    }
}
