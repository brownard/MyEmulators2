using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    internal partial class Wzd_NewEmu_Start : WzdPanel
    {
        public Wzd_NewEmu_Start(Emulator emu)
        {
            InitializeComponent();
            this.Emulator = emu;
        }

        private void pathBrowseButton_Click(object sender, EventArgs e)
        {
            string filter = "Executables (*.bat, *.exe, *.cmd) | *.bat;*.exe;*.cmd";
            string initialDirectory;
            int index = pathTextBox.Text.LastIndexOf("\\");

            if (index > -1)
                initialDirectory = pathTextBox.Text.Remove(index);
            else
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            using (OpenFileDialog dlg = Emulators2Settings.OpenFileDialog("Path to executable", filter, initialDirectory))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    pathTextBox.Text = dlg.FileName;
            }
        }

        public override bool Next()
        {
            if (!pathTextBox.Text.IsExecutable() || !System.IO.File.Exists(pathTextBox.Text))
            {
                MessageBox.Show("Please enter a valid path to an .exe or .bat file (without arguments).", "Invalid file", MessageBoxButtons.OK);
                return false;
            }

            this.Emulator.DefaultProfile.EmulatorPath = pathTextBox.Text;
            autoConfig();
            return true;
        }

        void autoConfig()
        {
            EmulatorProfile autoSettings = EmuSettingsAutoFill.Instance.CheckForSettings(pathTextBox.Text);
            if (autoSettings == null)
                return;
            
            if (autoSettings.HasSettings && MessageBox.Show(string.Format("Would you like to use the recommended settings for {0}?", autoSettings.Title), "Use recommended settings?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(autoSettings.Filters) && Options.Instance.GetBoolOption("autoconfemu"))
                    Emulator.Filter = autoSettings.Filters;

                if (!string.IsNullOrEmpty(autoSettings.Platform))
                {
                    Emulator.PlatformTitle = autoSettings.Platform;
                    Emulator.Title = autoSettings.Platform;
                    Emulator.CaseAspect = EmuSettingsAutoFill.Instance.GetCaseAspect(autoSettings.Platform);
                }

                if (autoSettings.HasSettings)
                {
                    Emulator.DefaultProfile.Arguments = autoSettings.Arguments;
                    Emulator.DefaultProfile.UseQuotes = autoSettings.UseQuotes != false; //default to true if null
                    Emulator.DefaultProfile.SuspendMP = autoSettings.SuspendMP == true;
                    Emulator.DefaultProfile.WorkingDirectory = autoSettings.WorkingDirectory;
                    Emulator.DefaultProfile.MountImages = autoSettings.MountImages;
                    Emulator.DefaultProfile.EscapeToExit = autoSettings.EscapeToExit;
                }
            }
        }
    }
}
