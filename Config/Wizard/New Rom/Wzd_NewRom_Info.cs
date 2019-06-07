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
    internal partial class Wzd_NewRom_Info : WzdPanel
    {
        public Wzd_NewRom_Info(Game game)
        {
            InitializeComponent();
            this.Game = game;
        }

        Emulator currentEmu = null;

        public override void UpdatePanel()
        {
            if(Game.ParentEmulator == null)
                profileComboBox.Items.Clear();
            else if (currentEmu == null || currentEmu.UID != Game.ParentEmulator.UID)
            {
                currentEmu = Game.ParentEmulator;
                profileComboBox.Items.Clear();
                foreach (EmulatorProfile profile in DB.Instance.GetProfiles(Game.ParentEmulator))
                    profileComboBox.Items.Add(profile);
                if (profileComboBox.Items.Count > 0)
                    profileComboBox.SelectedItem = profileComboBox.Items[0];
            }

            if (string.IsNullOrEmpty(txt_Title.Text))
                txt_Title.Text = titleFromPath(Game.Path);
        }

        public override bool Next()
        {
            if (string.IsNullOrEmpty(txt_Title.Text))
            {
                MessageBox.Show("Please enter a title for the new game", "No title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (profileComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select an emulator profile to use with the new game", "No profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            Game.Title = txt_Title.Text;
            Game.SelectedProfileId = ((EmulatorProfile)profileComboBox.SelectedItem).ID;
            Game.Favourite = favCheckBox.Checked;
            Game.IsInfoChecked = !importCheckBox.Checked;
            return true;
        }

        string titleFromPath(string path)
        {
            string s = "";
            int index = path.LastIndexOf(".");
            if (index > -1)
                s = path.Remove(index);

            if (s.Length > 0)
                s = s.Substring(s.LastIndexOf("\\") + 1);

            return s;
        }
    }
}
