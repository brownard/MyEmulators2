using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmulators2
{
    internal partial class Wzd_NewEmu_Config : WzdPanel
    {
        public Wzd_NewEmu_Config(Emulator emu)
        {
            InitializeComponent();
            this.Emulator = emu;
        }

        public override void UpdatePanel()
        {
            pathTextBox.Text = Emulator.DefaultProfile.EmulatorPath;
            workingDirTextBox.Text = Emulator.DefaultProfile.WorkingDirectory;
            argumentsTextBox.Text = Emulator.DefaultProfile.Arguments;
            useQuotesCheckBox.Checked = Emulator.DefaultProfile.UseQuotes != false;
        }

        public override bool Next()
        {
            if (!string.IsNullOrEmpty(workingDirTextBox.Text) && !System.IO.Directory.Exists(workingDirTextBox.Text))
            {
                MessageBox.Show("Please enter a valid working directory, or leave empty to use the exe/bat directory.", "Invalid working directory", MessageBoxButtons.OK);
                return false;
            }
            Emulator.DefaultProfile.WorkingDirectory = workingDirTextBox.Text;
            Emulator.DefaultProfile.Arguments = argumentsTextBox.Text;
            Emulator.DefaultProfile.UseQuotes = useQuotesCheckBox.Checked;

            return true;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string title = "Select working directory";
            string initialDir;

            if (System.IO.Directory.Exists(workingDirTextBox.Text))
                initialDir = workingDirTextBox.Text;
            else if (Emulator.DefaultProfile.EmulatorPath.LastIndexOf("\\") > -1)
                initialDir = Emulator.DefaultProfile.EmulatorPath.Substring(0, Emulator.DefaultProfile.EmulatorPath.LastIndexOf("\\"));
            else
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (FolderBrowserDialog dlg = Emulators2Settings.OpenFolderDialog(title, initialDir))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    workingDirTextBox.Text = dlg.SelectedPath;
            }
        }

    }
}
