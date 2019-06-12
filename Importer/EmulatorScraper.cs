extern alias nsoft;

using MyEmulators2.Import.TheGamesDb;
using nsoft::Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace MyEmulators2
{
    class EmulatorScraper
    {
        const string API_KEY = "f45deae02380f9171ceb6b93db79bb6241109906da7e3dab29b91b6827fea3ee";
        const string PLATFORM_FIELDS = @"icon%2Cconsole%2Ccontroller%2Cdeveloper%2Cmanufacturer%2Cmedia%2Ccpu%2Cmemory%2Cgraphics%2Csound%2Cmaxcontrollers%2Cdisplay%2Coverview%2Cyoutube";

        static readonly object syncObj = new object();
        static List<Platform> _platforms = null;

        static ConcurrentDictionary<string, ImageResult> _platformImages = new ConcurrentDictionary<string, ImageResult>();

        bool TryLoadPlatforms()
        {
            lock (syncObj)
            {
                if (_platforms != null && _platforms.Count > 0)
                    return true;

                PlatformResult platformResult;
                try
                {
                    string json;
                    using (WebClient client = new WebClient())
                        json = client.DownloadString($"https://api.thegamesdb.net/Platforms?apikey={API_KEY}&fields={PLATFORM_FIELDS}");
                    platformResult = JsonConvert.DeserializeObject<PlatformResult>(json);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error retrieving online platform info - {0}", ex.Message);
                    platformResult = null;
                }

                if (platformResult == null)
                {
                    Logger.LogError("Failed to retrieve online platform info");
                    return false;
                }

                _platforms = platformResult?.Data?.Platforms?.Values.OrderBy(p => p.Name).ToList();
                return _platforms != null && _platforms.Count > 0;
            }
        }

        bool TryGetPlatformImages(string platformId, out ImageResult result)
        {
            if (_platformImages.TryGetValue(platformId, out result))
                return true;
            
            try
            {
                string json;
                using (WebClient client = new WebClient())
                    json = client.DownloadString($"https://api.thegamesdb.net/Platforms/Images?apikey={API_KEY}&platforms_id={platformId}");
                result = JsonConvert.DeserializeObject<ImageResult>(json);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving online platform images - {0}", ex.Message);
                result = null;
            }

            if (result == null)
            {
                Logger.LogError("Failed to retrieve online platform images");
                return false;
            }

            _platformImages.TryAdd(platformId, result);
            return true;
        }

        public Dictionary<string, string> GetEmulators(string searchPlatform, out string matchPlatform)
        {
            matchPlatform = null;
            if (!TryLoadPlatforms())
                return null;

            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(platformConvert, string.Format("[|]([^\n,]*),{0}[|]", searchPlatform));
            int matchId;
            if (!m.Success || !int.TryParse(m.Groups[1].Value, out matchId))
                matchId = -1;

            Dictionary<string, string> platforms = new Dictionary<string, string>(_platforms.Count);
            foreach (Platform platform in _platforms)
            {
                platforms[platform.Name] = platform.Id.ToString();
                if (matchId > -1 && platform.Id == matchId)
                    matchPlatform = platform.Name;
            }
            return platforms;
                
        }

        public EmulatorInfo GetInfo(string platformId)
        {
            if (!TryLoadPlatforms())
                return null;

            Platform platform = _platforms.FirstOrDefault(p => p.Id.ToString() == platformId);
            if (platform == null)
                return null;

            EmulatorInfo emuInfo = new EmulatorInfo();
            emuInfo.Title = platform.Name;
            emuInfo.Developer = platform.Developer;
            //emuInfo.Grade = HttpUtility.HtmlDecode(a.InnerText);
            emuInfo.Overview = platform.Overview;
            emuInfo.CPU = platform.Cpu;
            emuInfo.Memory = platform.Memory;
            emuInfo.Graphics = platform.Graphics;
            emuInfo.Sound = platform.Sound;
            emuInfo.Display = platform.Display;
            emuInfo.Media = platform.Media;
            emuInfo.MaxControllers = platform.MaxControllers;
            //baseImageUrl = a.InnerXml;
            //emuInfo.FanartUrl = baseImageUrl + a.InnerXml;
            //emuInfo.LogoUrl = baseImageUrl + a.InnerXml;

            return emuInfo;
        }

        public void GetThumbs(string platformId, out IEnumerable<string> covers, out IEnumerable<string> fanart)
        {
            covers = null;
            fanart = null;

            ImageResult imageResult;
            if (!TryGetPlatformImages(platformId, out imageResult))
                return;

            string baseImageUrl = imageResult.Data?.BaseUrl?.Original ?? string.Empty;
            List<Image> images = imageResult.Data?.Images?.Values.SelectMany(i => i).ToList() ?? new List<Image>();

            covers = images.Where(i => i.Type == "boxart" || i.Type == "icon" || i.Type == "banner").Select(i => baseImageUrl + i.Filename).ToList();
            fanart = images.Where(i => i.Type == "fanart").Select(i => baseImageUrl + i.Filename).ToList();
        }

        string platformConvert = @"
            |25,3DO|
            |4911,Amiga|
            |23,Arcade|
            |22,Atari 2600|
            |26,Atari 5200|
            |27,Atari 7800|
            |28,Atari Jaguar|
            |29,Atari Jaguar CD|
            |30,Atari XE|
            |31,Colecovision|
            |40,Commodore 64|
            |32,Intellivision|
            |37,Macintosh|
            |14,Xbox|
            |15,Xbox 360|
            |24,NeoGeo|
            |3,Nintendo 64|
            |8,Nintendo DS|
            |7,NES|
            |4,Game Boy|
            |5,Game Boy Advance|
            |41,Game Boy Color|
            |2,GameCube|
            |9,Wii|
            |38,Wii U|
            |1,Windows|
            |33,SEGA 32X|
            |21,SEGA CD|
            |16,Dreamcast|
            |20,Game Gear|
            |18,Genesis|
            |35,SEGA Master System|
            |36,SEGA Mega Drive|
            |17,SEGA Saturn|
            |10,Playstation|
            |11,Playstation 2|
            |12,Playstation 3|
            |39,Playstation Vita|
            |13,PSP|
            |6,SNES|
            |34,TurboGrafx-16|";
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
