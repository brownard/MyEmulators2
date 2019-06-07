using System;
using System.Collections.Generic;
using MediaPortal.Configuration;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace MyEmulators2
{
    public enum OptionType
    {
        Bool,
        Int,
        String
    }

    class Options
    {
        const string XML_FILENAME = "Emulators_2.xml";

        //sync root for thread safety
        object optionSync = new object();

        //Dynamic options
        Dictionary<string, bool> boolOptions = null;
        Dictionary<string, int> intOptions = null;
        Dictionary<string, string> stringOptions = null;
        List<string> ignoredFiles = null;

        #region Singleton

        static object instanceSync = new object();
        static Options instance = null;
        public static Options Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceSync)
                        if (instance == null)
                            instance = new Options();
                return instance;
            }
        }

        #endregion

        #region Defaults

        //sets the default options, the option value is only set if it is not already in the current list of options
        private void loadDefaults()
        {
            AddOption("shownname", "My Emulators"); //Plugin title
            AddOption("language", "English"); //Plugin language
            AddOption("autoconfemu", true); //Whether to auto configurate the profile when selecting an executable path

            AddOption("startupstate", -1);
            AddOption("laststartupstate", 1);
            AddOption("clicktodetails", true);

            //View options, 
            //0 = List, 1=small icons, 2=large icons, 3=filmstrip, 4=coverflow

            AddOption("viewemus", 0); //The default view for the emulator list
            AddOption("defaultviewroms", 0); //default view for rom list
            AddOption("viewpcgames", 0); //default view for PC game list
            AddOption("viewfavourites", 0); //default view for favourite list

            AddOption("showsortvalue", true); //Whether to display the sorted value in the facade when sorting

            //Thumb options
            AddOption("fanartdelay", 500); //the delay in ms before fanart is loaded for a new item
            AddOption("gameartdelay", 500); //the delay in ms before gameart is loaded for a new item
            AddOption("showfanart", true); //Whether to show fanart (supported skin required)
            AddOption("showgameart", true); //Whether to show gameart (supported skin required)

            //Import options
            AddOption("autorefreshgames", true); //Whether to auto refresh the database on plugin start
            AddOption("importtop", false); //Whether to auto select the top search result when a better match is not found
            AddOption("importexact", false); //Whether to only approve exact title and platform match
            AddOption("autoimportgames", true); //Whether to auto-import games when opening the plugin in MP
            AddOption("resizethumbs", true); //Whether to resize selected box art to the correct aspect ratio for the platform
            AddOption("thoroughthumbsearch", false); //Whether to search all scrapers recursively until all missing artwork is found
            AddOption("thumblocation", MediaPortal.Configuration.Config.GetFolder(MediaPortal.Configuration.Config.Dir.Thumbs)); //The directory where the thumbs are located

            //key map
            AddOption("domap", false); //Whether to stop emulation on mapped key
            AddOption("mappedkey", ""); //The key used to stop emulation

            //community server
            AddOption("retrieveGameDetials", true); // use the community server to get the most likely info for a game, rather then scrapping for it.
            AddOption("submitGameDetails", true); // submit scrapped info to the community server for other clients to use.
            AddOption("communityServerAddress", "technohub.dyndns-home.com:4567"); // the address of the community server.
            AddOption("communityServerConnectionRetryTime", 15); // The time to wait in minutes to retry to connect to server after previous connection error.

            //advanced options
            AddOption("goodmergefilters", "*.7z;*.rar;*.zip;*.tar"); //The file filters used to determine whether a rom is a Goodmerge archive
            AddOption("showgmdialogonce", true); //Whether to display the goodmerge select dialog only on first run of game
            AddOption("showgmdialog", false); //Whether to ever display the goodmerge select dialog

            AddOption("importthreadcount", 5); //The number of background threads to use when running the importer, a higher count will give better performance on high-end machines
            AddOption("hashthreadcount", 2); //The maximum number of import threads allowed to hash files at the same time, a lower count helps limit cpu usage
            AddOption("stopmediaplayback", true); //Whether to stop any playing media in MP when starting a game

            //PC Emulator
            AddOption("pcitemtitle", "PC Games"); //Title
            AddOption("pcitemcompany", ""); //Company
            AddOption("pcitemdescription", ""); //Description
            AddOption("pcitemyear", 0); //Year
            AddOption("pcitemgrade", 0); //Grade
            AddOption("pcitemposition", 0); //List position
            AddOption("pcitemvideopreview", ""); //Preview Vid
            AddOption("pcitemcaseaspect", 71); //case aspect, default to DVD
            AddOption("pcitemcheckcontroller", false);
            AddOption("pcitemdirectory", "");
            AddOption("pcitemfilter", "*.lnk");

            //video previews
            AddOption("showvideopreview", false); //show videos
            AddOption("loopvideopreview", false); //loop videos
            AddOption("defaultvideopreview", false); //use emu video if game vid not present
            AddOption("videopreviewdelay", 2000); //delay before video starts (ms)

            AddOption("scraperpriorities", "99999996;99999999;99999995;99999998;99999997;");
            AddOption("ignoredscrapers", "99999997;99999998"); //script ids (as strings) to ignore, delimited by ';' - Ignore RetroCPU by default as SLOOOOW
            AddOption("coversscraperid", "99999996");
            AddOption("screensscraperid", "99999999");
            AddOption("fanartscraperid", "99999996");

            AddOption("maxthumbdimension", 640);

            //Backup/Restore preferences
            AddOption("backupfile", "");
            AddOption("backupthumbs", true);
            AddOption("restorefile", "");
            AddOption("restorethumbs", true);
            AddOption("restoremerge", true);
            AddOption("restoreemusetting", 0);
            AddOption("restoreprofilesetting", 0);
            AddOption("restoregamesetting", 1);

            AddOption("mappedkeydata", 0);
            AddOption("coversscraperid", "");
            AddOption("screensscraperid", "");
            AddOption("fanartscraperid", "");
        }

        #endregion

        public Options()
        {
            init();
        }

        void init()
        {
            lock (optionSync)
            {
                //initialise dictionaries
                boolOptions = new Dictionary<string, bool>();
                intOptions = new Dictionary<string, int>();
                stringOptions = new Dictionary<string, string>();
                ignoredFiles = new List<string>();

                //load the xmlfile
                loadDoc();
                //set default options if not already present
                loadDefaults();
            }
        }

        //private method used to add options from xmldoc, attempts to parse the string value to the specified type
        void addXMLOption(string name, string value, OptionType optionType)
        {
            if (value == null)
            {
                Logger.LogError("Error adding option, value cannot be null");
            }

            switch (optionType)
            {
                case OptionType.Bool:
                    bool boolVal;
                    if (!bool.TryParse(value, out boolVal)) //try and get bool from string
                    {
                        Logger.LogError("Error adding bool option '{0}', the value passed was not boolean", name);
                        return;
                    }
                    AddOption(name, boolVal); //add if successful
                    break;
                case OptionType.Int:
                    int intVal;
                    if (!int.TryParse(value, out intVal))
                    {
                        Logger.LogError("Error adding int option '{0}', the value passed was not an integer", name);
                        return;
                    }
                    AddOption(name, intVal);
                    break;
                case OptionType.String:
                    AddOption(name, value); //no need to convert string
                    break;
            }
        }

        /// <summary>
        /// Adds the specified option to options list.
        /// The option value is only set if the option is not already in the current list.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">Should be the correct Type for the value, i.e. int, bool or string</param>
        public void AddOption(string name, object value)
        {
            if (string.IsNullOrEmpty(name)) //don't allow empty or null option names
            {
                Logger.LogError("Option name cannot be empty or null");
                return;
            }

            lock (optionSync)
            {
                if (value is bool)
                {
                    if (boolOptions.ContainsKey(name))
                        return;
                    boolOptions.Add(name, (bool)value);
                }
                else if (value is int)
                {
                    if (intOptions.ContainsKey(name))
                        return;
                    intOptions.Add(name, (int)value);
                }
                else
                {
                    string stringVal = value as string;
                    if (stringVal == null)
                    {
                        Logger.LogError("Error adding option '{0}', the value was not a valid Type", name);
                        return;
                    }
                    if (stringOptions.ContainsKey(name))
                        return;
                    stringOptions.Add(name, stringVal);
                }
            }
        }

        /// <summary>
        /// Updates the specified option with the specified value.
        /// If the option is not in the current list it is added.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">Should be the correct Type for the value, i.e. int, bool or string</param>
        public void UpdateOption(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogError("Option name cannot be empty or null");
                return;
            }

            lock (optionSync)
            {
                if (value is bool)
                {
                    if (boolOptions.ContainsKey(name))
                        boolOptions[name] = (bool)value;
                    else
                        boolOptions.Add(name, (bool)value);
                }
                else if (value is int)
                {
                    if (intOptions.ContainsKey(name))
                        intOptions[name] = (int)value;
                    else
                        intOptions.Add(name, (int)value);
                }
                else
                {
                    string stringVal = value as string;
                    if (stringVal == null)
                    {
                        Logger.LogError("Error adding option '{0}', the value was not a valid Type", name);
                        return;
                    }
                    if (stringOptions.ContainsKey(name))
                        stringOptions[name] = stringVal;
                    else
                        stringOptions.Add(name, stringVal);
                }
            }
        }

        #region Get Values

        /// <summary>
        /// Returns the specified option value as a basic object which will need
        /// to be manually casted to the correct Type. Returns null if the option was not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="optionType"></param>
        /// <returns></returns>
        public object GetOption(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogError("Option name cannot be empty or null");
                return null;
            }

            lock (optionSync)
            {
                if (boolOptions.ContainsKey(name))
                    return boolOptions[name];
                else if (intOptions.ContainsKey(name))
                    return intOptions[name];
                else if (stringOptions.ContainsKey(name))
                    return stringOptions[name];
                else
                    Logger.LogError("Option '{0}' was not found", name);

                return null;
            }
        }

        /// <summary>
        /// Returns the specified bool value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetBoolOption(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogDebug("Option name cannot be empty or null, using default value of false");
                return false;
            }

            lock (optionSync)
            {
                if (!boolOptions.ContainsKey(name))
                {
                    Logger.LogDebug("Bool option '{0}' does not exist, using default value of false", name);
                    return false;
                }
                return boolOptions[name];
            }
        }

        /// <summary>
        /// Returns the specified int value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetIntOption(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogDebug("Option name cannot be empty or null, using default value of 0");
                return 0;
            }

            lock (optionSync)
            {
                if (!intOptions.ContainsKey(name))
                {
                    Logger.LogDebug("Int option '{0}' does not exist, using default value of 0", name);
                    return 0;
                }

                return intOptions[name];
            }
        }

        /// <summary>
        /// Returns the specified string value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetStringOption(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogDebug("Option name cannot be empty or null, using default value of \"\"");
                return "";
            }

            lock (optionSync)
            {
                if (!stringOptions.ContainsKey(name))
                {
                    Logger.LogDebug("String option '{0}' does not exist, using default value of \"\"", name);
                    return "";
                }
                return stringOptions[name];
            }
        }

        #endregion
        
        public List<StartupStateHandler> GetStartupOptions(out int selectedValue)
        {
            selectedValue = GetIntOption("startupstate");
            if (selectedValue < -1 || selectedValue > 3)
                selectedValue = -1;

            List<StartupStateHandler> opts = new List<StartupStateHandler>();
            opts.Add(new StartupStateHandler() { Name = Translator.Instance.lastused, Value = -1 });
            opts.Add(new StartupStateHandler() { Name = Translator.Instance.viewemulators, Value = (int)StartupState.EMULATORS });
            opts.Add(new StartupStateHandler() { Name = Translator.Instance.viewgroups, Value = (int)StartupState.GROUPS });
            opts.Add(new StartupStateHandler() { Name = Translator.Instance.pcgames, Value = (int)StartupState.PCGAMES });
            opts.Add(new StartupStateHandler() { Name = Translator.Instance.favourites, Value = (int)StartupState.FAVOURITES });
            return opts;
        }

        public StartupState GetStartupState()
        {
            int opt = GetIntOption("startupstate");
            if (opt == -1)
                opt = GetIntOption("laststartupstate");

            if (!Enum.IsDefined(typeof(StartupState), opt))
                opt = 0;
            return (StartupState)opt;
        }

        public static string GetKeyDisplayString(int keyData)
        {
            int ctrl = (int)Keys.Control;
            int alt = (int)Keys.Alt;
            int shift = (int)Keys.Shift;

            string retString = "";

            if ((keyData & shift) == shift)
            {
                keyData = keyData ^ shift;
                retString += "Shift + ";
            }
            if ((keyData & ctrl) == ctrl)
            {
                keyData = keyData ^ ctrl;
                retString += "Ctrl + ";
            }
            if ((keyData & alt) == alt)
            {
                keyData = keyData ^ alt;
                retString += "Alt + ";
            }

            return retString + Enum.GetName(typeof(Keys), keyData);
        }

        #region File Ignore List

        public List<string> IgnoredFiles()
        {
            List<string> files = new List<string>();

            lock (optionSync)
                files.AddRange(ignoredFiles);

            return files;
        }

        public bool ShouldIgnoreFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            lock (optionSync)
                return ignoredFiles.Contains(path);
        }

        public void AddIgnoreFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            lock (optionSync)
                if (!ignoredFiles.Contains(path))
                    ignoredFiles.Add(path);
        }

        public void RemoveIgnoreFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            lock (optionSync)
                if (ignoredFiles.Contains(path))
                    ignoredFiles.Remove(path);
        }

        #endregion

        #region Load/Save

        //load the options xml document and populate the list of dynamic options
        void loadDoc()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Config.GetFile(Config.Dir.Config, XML_FILENAME)); //get the xml file

                XmlNodeList nodes = doc.GetElementsByTagName("option"); //select option node

                //loop through each option and determine the option Type
                foreach (XmlNode node in nodes)
                {
                    string type = node.Attributes.GetNamedItem("type").Value;

                    OptionType optionType = OptionType.String; //default to string
                    if (type.Equals("bool"))
                        optionType = OptionType.Bool;
                    else if (type.Equals("int"))
                        optionType = OptionType.Int;

                    //add the option to the correct list based on Type
                    addXMLOption(node.Attributes.GetNamedItem("name").Value, node.Attributes.GetNamedItem("value").Value, optionType);
                }

                foreach (XmlNode node in doc.GetElementsByTagName("ignorefile"))
                {
                    string path = node.Attributes.GetNamedItem("path").Value;
                    if (!ignoredFiles.Contains(path))
                        ignoredFiles.Add(path);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Saves the current list of options to the XML file.
        /// </summary>
        public void Save()
        {
            lock (optionSync)
            {
                XmlDocument doc = new XmlDocument();
                XmlNode headNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(headNode);

                XmlNode topnode = doc.CreateElement("options");
                doc.AppendChild(topnode);

                createOptionNodes(OptionType.Bool, topnode, doc);
                createOptionNodes(OptionType.Int, topnode, doc);
                createOptionNodes(OptionType.String, topnode, doc);

                createIgnoreNodes(topnode, doc);

                doc.Save(Config.GetFile(Config.Dir.Config, XML_FILENAME));
            }
        }

        //creates an xml node with the correct type for saving to the xml file
        void createOptionNodes(OptionType optionType, XmlNode parent, XmlDocument parentDoc)
        {
            Dictionary<string, string> nodes = new Dictionary<string, string>();
            string header = null;
            switch (optionType)
            {
                case OptionType.Bool:
                    header = "bool"; //set correct type
                    lock (optionSync)
                    {
                        foreach (KeyValuePair<string, bool> keyVal in boolOptions)
                            nodes.Add(keyVal.Key, keyVal.Value.ToString()); //loop through each value and create string representation
                        break;
                    }
                case OptionType.Int:
                    header = "int";
                    lock (optionSync)
                    {
                        foreach (KeyValuePair<string, int> keyVal in intOptions)
                            nodes.Add(keyVal.Key, keyVal.Value.ToString());
                        break;
                    }
                case OptionType.String: //still loop through string values and create local copy so we are thread safe later
                    header = "string";
                    lock (optionSync)
                    {
                        foreach (KeyValuePair<string, string> keyVal in stringOptions)
                            nodes.Add(keyVal.Key, keyVal.Value);
                        break;
                    }
            }

            //loop through all the required options and add to the parent node
            foreach (KeyValuePair<string, string> keyVal in nodes)
            {
                //create node in format <option type="string" name="shownname" value="My Emulators" />
                XmlNode node = parentDoc.CreateElement("option");
                XmlAttribute att1 = parentDoc.CreateAttribute("type");
                att1.Value = header;
                node.Attributes.Append(att1);
                XmlAttribute att2 = parentDoc.CreateAttribute("name");
                att2.Value = keyVal.Key;
                node.Attributes.Append(att2);
                XmlAttribute att3 = parentDoc.CreateAttribute("value");
                att3.Value = keyVal.Value;
                node.Attributes.Append(att3);
                parent.AppendChild(node);
            }
        }

        void createIgnoreNodes(XmlNode parent, XmlDocument parentDoc)
        {
            foreach (string file in ignoredFiles)
            {
                XmlNode fileNode = parentDoc.CreateElement("ignorefile");
                XmlAttribute path = parentDoc.CreateAttribute("path");
                path.Value = file;
                fileNode.Attributes.Append(path);
                parent.AppendChild(fileNode);
            }
        }

        #endregion
    }
}
