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
    internal partial class Wzd_NewRom_Start : WzdPanel
    {
        public Wzd_NewRom_Start(Game game)
        {
            InitializeComponent();
            initEmuBox();
            this.Game = game;
        }

        public override bool Next()
        {
            if (emuComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select the emulator to use to launch the game.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            Emulator emu = (Emulator)((ComboBoxItem)emuComboBox.SelectedItem).Value;
            string path = pathTextBox.Text;
            if (!checkPath(path, emu))
            {
                MessageBox.Show("Please enter a valid path.\r\nIf you are adding a PC game the file must be an exe/bat.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (Game.Exists(path))
            {
                MessageBox.Show("A game with that path already exists, you cannot have multiple games with the same path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            Game.Path = path;
            if (argsTextBox.Visible)
                Game.Arguments = argsTextBox.Text;
            Game.ParentEmulator = emu;
            return true;
        }

        private bool checkPath(string path, Emulator emu)
        {
            if (emuComboBox.SelectedItem == null)
                return false;

            path = path.Trim();
            if (emu.IsPc())
            {
                string parsedPath;
                if (!path.TryGetExecutablePath(out parsedPath))
                    return false;
                path = parsedPath;
            }
            return System.IO.File.Exists(path);
        }

        void initEmuBox()
        {
            bool selected = false;
            emuComboBox.Items.Clear();
            foreach (ComboBoxItem item in Dropdowns.GetNewRomComboBoxItems())
            {
                emuComboBox.Items.Add(item);
                if (!selected && item.ID == Emulator.GetPC().UID)
                {
                    emuComboBox.SelectedItem = item;
                    selected = true;
                }
            }
            if (!selected && emuComboBox.Items.Count > 0)
                emuComboBox.SelectedItem = emuComboBox.Items[0];

            emuComboBox.SelectedIndexChanged += new EventHandler(emuComboBox_SelectedIndexChanged);
            updateArgsVisibilty();
        }

        void emuComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateArgsVisibilty();
        }

        void updateArgsVisibilty()
        {
            ComboBoxItem item = emuComboBox.SelectedItem as ComboBoxItem;
            bool visible = item != null && item.ID == Emulator.GetPC().UID;
            argsInfoText.Visible = visible;
            argsLabel.Visible = visible;
            argsTextBox.Visible = visible;
        }

        private void pathBrowseButton_Click(object sender, EventArgs e)
        {
            string filter = "All files (*.*) | *.*";
            string initialDirectory;
            int index = pathTextBox.Text.LastIndexOf("\\");

            if (index > -1)
                initialDirectory = pathTextBox.Text.Remove(index);
            else
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (OpenFileDialog dlg = Emulators2Settings.OpenFileDialog("Path to game", filter, initialDirectory))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    pathTextBox.Text = dlg.FileName;
            }
        }
    }
}
