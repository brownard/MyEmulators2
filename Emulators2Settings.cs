using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MyEmulators2.Import;

namespace MyEmulators2
{
    class Emulators2Settings : IDisposable
    {
        #region Skin Constants

        public const string SKIN_FILE = "Emulators2.xml";
        public const string HOME_HOVER = "hover_Emulators2.png";

        const string DEFAULT_LOGO = "Emulators2_Logo";
        const string DEFAULT_FANART = "Emulators2_Fanart";

        #endregion

        #region Wildcards
        public const string GAME_WILDCARD = "%ROM%";
        public const string GAME_WILDCARD_NO_EXT = "%ROM_WITHOUT_EXT%";
        #endregion

        Emulators2Settings()
        {
            initThumbDir();
            initDefaultImages();
        }

        public void Init()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 100;
        }

        public void initThumbDir()
        {
            string location = Options.Instance.GetStringOption("thumblocation");
            if (location == "")
                location = Config.GetFolder(Config.Dir.Thumbs);
            else if (!System.IO.Directory.Exists(location))
            {
                Logger.LogError("Unable to locate thumb folder '{0}', reverting to default thumb location", location);
                location = Config.GetFolder(Config.Dir.Thumbs); //default to MP thumb directory
                Options.Instance.UpdateOption("thumblocation", location);
            }

            location = location.TrimEnd('\\'); //remove any trailing '\'
            ThumbDirectory = location;            
        }

        void initDefaultImages()
        {
            string checkLogo = string.Format(@"{0}\Media\{1}", MediaPortal.GUI.Library.GUIGraphicsContext.Skin, DEFAULT_LOGO);
            if (!System.IO.File.Exists(checkLogo + ".png") && System.IO.File.Exists(checkLogo + ".jpg"))
                DefaultLogo = checkLogo + ".jpg";
            else
                DefaultLogo = checkLogo + ".png";

            string checkFanart = string.Format(@"{0}\Media\{1}", MediaPortal.GUI.Library.GUIGraphicsContext.Skin, DEFAULT_FANART);
            if (!System.IO.File.Exists(checkFanart + ".png") && System.IO.File.Exists(checkFanart + ".jpg"))
                DefaultFanart = checkFanart + ".jpg";
            else
                DefaultFanart = checkFanart + ".png";
        }

        bool isConfig = false;
        public bool IsConfig
        {
            get { return isConfig; }
            set { isConfig = value; }
        }

        public string ThumbDirectory
        {
            get;
            private set;
        }

        public string DefaultLogo
        {
            get;
            private set;
        }

        public string DefaultFanart
        {
            get;
            private set;
        }

        static object instanceLock = new object();
        static Emulators2Settings instance = null;
        public static Emulators2Settings Instance
        {
            get
            {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new Emulators2Settings();
                return instance;
            }
        }

        Importer importer = null;
        public Importer Importer
        {
            get
            {
                if (importer == null)
                    importer = new Importer();
                return importer;
            }
        }

        ThumbContext thumbContextMenu = null;
        public ThumbContext ThumbContextMenu
        {
            get
            {
                if (thumbContextMenu == null)
                    thumbContextMenu = new ThumbContext();
                return thumbContextMenu;
            }
        }

        public static OpenFileDialog OpenFileDialog(string title, string filter, string initialDirectory)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = title;
            dlg.Filter = filter;
            dlg.InitialDirectory = initialDirectory;

            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.ValidateNames = true;

            dlg.Multiselect = false;
            dlg.RestoreDirectory = true;

            return dlg;
        }

        public static FolderBrowserDialog OpenFolderDialog(string title, string initialDirectory)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = title;
            dlg.SelectedPath = initialDirectory;
            return dlg;
        }

        public static int GetComboBoxDropDownWidth(ComboBox comboBox)
        {
            int maxWidth = 0, temp = 0;

            foreach (var obj in comboBox.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), comboBox.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            if (comboBox.Items.Count > comboBox.MaxDropDownItems)
                maxWidth += SystemInformation.VerticalScrollBarWidth;
            return maxWidth;
        }

        public void ShowMPDialog(string message, params object[] args)
        {
            message = string.Format(message, args);
            string heading = Options.Instance.GetStringOption("shownname");

            if (isConfig)
            {
                MessageBox.Show(message, heading, MessageBoxButtons.OK);
                return;
            }

            MediaPortal.Dialogs.GUIDialogOK dlg_error = (MediaPortal.Dialogs.GUIDialogOK)MediaPortal.GUI.Library.GUIWindowManager.GetWindow
                ((int)MediaPortal.GUI.Library.GUIWindow.Window.WINDOW_DIALOG_OK);
            if (dlg_error != null)
            {
                dlg_error.Reset();
                dlg_error.SetHeading(heading);

                string[] lines = message.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < lines.Length; x++)
                    dlg_error.SetLine(x + 1, lines[x]);
                dlg_error.DoModal(MediaPortal.GUI.Library.GUIWindowManager.ActiveWindow);
            }
        }

        public void Dispose()
        {
            if (thumbContextMenu != null)
            {
                thumbContextMenu.Dispose();
                thumbContextMenu = null;
            }
        }
    }
}
