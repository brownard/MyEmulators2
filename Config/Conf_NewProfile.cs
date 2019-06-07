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
    public partial class Conf_NewProfile : Form
    {
        public EmulatorProfile EmulatorProfile { get; private set; }

        public Conf_NewProfile(Emulator emu, EmulatorProfile profile)
        {
            InitializeComponent();
            if (profile == null)
            {
                EmulatorProfile = new EmulatorProfile(false);
                EmulatorProfile.EmulatorID = emu.UID;
                EmulatorProfile.EmulatorPath = emu.DefaultProfile.EmulatorPath;
            }
            else
            {
                EmulatorProfile = profile;
                this.Text = "Rename Profile";
            }
            profileTitleTextBox.SelectedText = EmulatorProfile.Title;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (EmulatorProfile != null)
            {
                EmulatorProfile.Title = profileTitleTextBox.Text;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }


    }
}
