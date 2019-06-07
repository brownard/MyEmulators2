using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace MyEmulators2
{
    public class EmuSettingsAutoFill
    {
        /// <summary>
        /// Flag used to denote directory where emulator exe is located
        /// </summary>
        public static readonly string USE_EMULATOR_DIRECTORY = "%EMU_EXE_DIR%";
        Dictionary<string, EmulatorProfile> autoConfigDictionary;
        Dictionary<string, double> aspectDictionary;

        public EmuSettingsAutoFill()
        {
            initSettings();
        }

        static object instanceSync = new object();
        static EmuSettingsAutoFill instance = null;
        public static EmuSettingsAutoFill Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceSync)
                        if (instance == null)
                            instance = new EmuSettingsAutoFill();
                return instance;
            }
        }

        public EmulatorProfile CheckForSettings(string emuPath)
        {
            emuPath = getExeName(emuPath);
            foreach (string key in autoConfigDictionary.Keys)
            {
                if (Regex.IsMatch(emuPath, wildcardToRegex(key), RegexOptions.IgnoreCase))
                    return autoConfigDictionary[key];
            }
            return null;
        }

        public double GetCaseAspect(string platform)
        {
            if (platform == null)
                return 0;
            double aspect;
            if (!aspectDictionary.TryGetValue(platform, out aspect))
                aspect = 0;
            return aspect;
        }
        
        public static void SetupAspectDropdown(System.Windows.Forms.ComboBox thumbAspectComboBox, double aspect)
        {
            bool selected = false;
            string fmtStr = "{0} ({1})";

            thumbAspectComboBox.Items.Clear();
            int index = thumbAspectComboBox.Items.Add(string.Format(fmtStr, 0, "Default"));
            if (aspect == 0)
            {
                thumbAspectComboBox.SelectedIndex = index;
                selected = true;
            }

            index = thumbAspectComboBox.Items.Add(string.Format(fmtStr, 0.71, "DVD"));
            if (aspect == 0.71)
            {
                thumbAspectComboBox.SelectedIndex = index;
                selected = true;
            }

            index = thumbAspectComboBox.Items.Add(string.Format(fmtStr, 1.12, "GameBoy"));
            if (aspect == 1.12)
            {
                thumbAspectComboBox.SelectedIndex = index;
                selected = true;
            }

            index = thumbAspectComboBox.Items.Add(string.Format(fmtStr, 1.14, "CD"));
            if (aspect == 1.14)
            {
                thumbAspectComboBox.SelectedIndex = index;
                selected = true;
            }

            index = thumbAspectComboBox.Items.Add(string.Format(fmtStr, 1.45, "Cartridge"));
            if (aspect == 1.45)
            {
                thumbAspectComboBox.SelectedIndex = index;
                selected = true;
            }

            if (!selected)
            {
                thumbAspectComboBox.Items.Insert(0, string.Format(fmtStr, aspect, "Custom"));
                thumbAspectComboBox.SelectedIndex = 0;
            }
        }

        void initSettings()
        {
            autoConfigDictionary = new Dictionary<string, EmulatorProfile>();
            aspectDictionary = new Dictionary<string, double>();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MyEmulators2.Data.EmuSettings.xml"));
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading Emulator auto configuration settings - {0}", ex.Message);
                return;
            }
            XmlNodeList platforms = doc.SelectNodes("//Platform");
            XmlNode dummyAttr;
            foreach (XmlNode platform in platforms)
            {
                dummyAttr = platform.SelectSingleNode("./@name");
                string platformName = dummyAttr != null ? dummyAttr.Value : null;
                bool hasPlatformReference = !string.IsNullOrEmpty(platformName);

                dummyAttr = platform.SelectSingleNode("./@caseaspect");
                if (hasPlatformReference && dummyAttr != null)
                {
                    double aspect;
                    if (double.TryParse(dummyAttr.Value, out aspect))
                        aspectDictionary[platformName] = aspect; 
                }

                foreach (XmlNode emulator in platform.SelectNodes("./Emulator"))
                {
                    dummyAttr = emulator.SelectSingleNode("./@exe");
                    if (dummyAttr == null || string.IsNullOrEmpty(dummyAttr.Value))
                        continue;
                    EmulatorProfile autoConfig = createAutoConfig(emulator, dummyAttr.Value);
                    if(hasPlatformReference)
                        autoConfig.Platform = platformName;
                    autoConfigDictionary[dummyAttr.Value] = autoConfig;
                }
            }
        }

        EmulatorProfile createAutoConfig(XmlNode emulator, string exeName)
        {
            EmulatorProfile autoConfig = new EmulatorProfile(false);
            XmlNode titleNode = emulator.SelectSingleNode("./@name");
            if (titleNode != null)
                autoConfig.Title = titleNode.Value;
            else
                autoConfig.Title = exeName;

            foreach (XmlNode property in emulator.SelectNodes("./*"))
            {
                if (string.IsNullOrEmpty(property.InnerText))
                    continue;
                System.Reflection.PropertyInfo pi = null;
                try { pi = typeof(EmulatorProfile).GetProperty(property.Name); }
                catch { }
                if (pi == null)
                    continue;

                if (pi.PropertyType == typeof(string))
                    pi.SetValue(autoConfig, property.InnerText, null);
                else if (pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(bool?))
                {
                    bool result;
                    if (bool.TryParse(property.InnerText, out result))
                        pi.SetValue(autoConfig, result, null);
                }
                else
                    continue;
                if (property.Name != "Filters")
                    autoConfig.HasSettings = true;
            }
            return autoConfig;
        }

        string getExeName(string input)
        {
            string ret = input;
            int index = ret.LastIndexOf("\\");
            if (index > -1 && index < ret.Length - 1)
                ret = ret.Substring(index + 1);

            return ret;
        }

        string wildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }
    }
}
