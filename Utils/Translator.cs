using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using System.Reflection;

namespace MyEmulators2
{
    public class Translator
    {
        #region Translations

        public string layoutheader = "Layouts Menu";
        public string layout = "Layout";
        public string layoutlist = "List";
        public string layoutlargeicons = "Large Icons";
        public string layouticons = "Icons";
        public string layoutfilmstrip = "Filmstrip";
        public string layoutcoverflow = "Coverflow";
        public string layoutlastused = "Last Used";

        public string selectgrade = "Select grade for this game";
        public string favourite = "Favourite";
        public string favourites = "Favourites";
        public string pcgames = "PC Games";
        public string never = "Never";
        public string allgames = "All Games";
        public string launchwithprofile = "Launch with profile";

        public string goodmerge = "Goodmerge";
        public string goodmergeselect = "Goodmerge select";
        public string goodmergeerror = "Goodmerge error";
        public string goodmergeempty = "The selected archive appears to be empty";

        public string nogamesfound = "No matching games found";
        public string releasedate = "Release Date";
        public string developer = "Developer";
        public string genre = "Genre";
        public string lastplayed = "Last Played";
        public string grade = "Grade";
        public string sortby = "Sort by";
        public string year = "Year";
        public string title = "Title";
        public string playcount = "Play Count";
        public string defaultsort = "Default";
        public string lastused = "Last used";

        public string goodmerge7zerror = "Error setting path to 7z.dll";
        public string goodmergearchiveerror = "Error reading archive";
        public string goodmergefileerror = "Error creating temporary game file";
        public string goodmergeextracterror = "Error extracting archive";

        public string nocontrollerconnected = "Starting game with no controllers connected.";

        public string noitemstodisplay = "No items to display";

        public string switchview = "Switch View";
        public string viewemulators = "Emulators";
        public string viewgroups = "Groups";

        public string disc = "Disc";
        public string discselect = "Select disc";
        public string runimport = "Run Importer";
        public string retrieveonlineinfo = "Retrieve online info";
        public string unknown = "Unknown";
        public string profile = "Profile";
        public string selectprofile = "Select Profile";

        public string emulatorpath = "Emulator Path";
        public string arguments = "Arguments";
        public string workingdirectory = "Working Directory";
        public string suspendmp = "Suspend MediaPortal";
        public string usequotes = "Use Quotes";
        public string mountimages = "Mount Images";
        public string escapetoexit = "Closes with Esc";
        public string checkcontroller = "Check for controller";
        public string launchedexe = "Launched exe";
        public string options = "Options";
        public string clicktodetails = "Click to details";
        public string stopmediaplayback = "Stop media playback";
        public string showsortvalue = "Show sort value";
        public string startupview = "Start with";

        public string currentprofile = "Current Profile";

        #endregion

        public Translator()
        {
            init();
        }

        static Translator instance = null;
        public static Translator Instance
        {
            get
            {
                if (instance == null)
                    instance = new Translator();
                return instance;
            }
        }

        void init()
        {
            string transFolder = Config.GetFolder(Config.Dir.Language) + @"\Emulators2";
            if (!Directory.Exists(transFolder))
            {
                Directory.CreateDirectory(transFolder);
            }

            //CreateEngXml();

            //Load the translation file
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(transFolder + "\\" + Options.Instance.GetStringOption("language") + ".xml");
                XmlNodeList nodes = doc.GetElementsByTagName("translatedstring");
                Dictionary<string, string> translations = new Dictionary<string, string>();
                foreach (XmlNode node in nodes)
                {
                    string key = node.Attributes.GetNamedItem("key").Value;
                    string value = node.InnerText;
                    translations[key] = value;
                }

                Type transType = typeof(Translator);
                FieldInfo[] fieldInfos = transType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fi in fieldInfos)
                {
                    if (translations.ContainsKey(fi.Name))
                        fi.SetValue(this, translations[fi.Name]);
                    else
                        Logger.LogDebug("Translation missing for field '{0}'", fi.Name);
                }
            }
            catch (Exception)
            {
                //Could not open the selected translation file, 
                //so use the standard english sentences
            }
        }

        public void CreateEngXml()
        {
            Type transType = typeof(Translator);
            FieldInfo[] fieldInfos = transType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            StringBuilder sb = new StringBuilder("<?xml version='1.0' encoding='UTF-8'?>");
            sb.AppendLine("<translation>");
            foreach (FieldInfo fi in fieldInfos)
                sb.AppendLine(string.Format("<translatedstring key='{0}'>{1}</translatedstring>", fi.Name, fi.GetValue(this)));

            sb.AppendLine("</translation>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            doc.Save("c:\\English.xml");
        }

        public List<string> GetLanguages()
        {
            string transFolder = Config.GetFolder(Config.Dir.Language) + @"\Emulators2";
            List<string> languages = new List<string>();
            try
            {
                foreach (String languageFile in Directory.GetFiles(transFolder, "*.xml"))
                    languages.Add(System.IO.Path.GetFileNameWithoutExtension(languageFile));
            }
            catch { }
            if (languages.Count < 1)
                languages.Add("English");
            return languages;
        }

        public void TranslateSkin()
        {
            Type transType = typeof(Translator);
            FieldInfo[] fieldInfos = transType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo fi in fieldInfos)
            {
                GUIPropertyManager.SetProperty("#Emulators2.Label." + fi.Name, (string)fi.GetValue(this));
            }
        }
    }
}
