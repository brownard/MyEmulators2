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
    internal partial class Wzd_NewEmu_Config2 : WzdPanel
    {
        public Wzd_NewEmu_Config2(Emulator emu)
        {
            InitializeComponent();
            this.Emulator = emu;
        }

        public override void UpdatePanel()
        {
            EmulatorProfile profile = Emulator.DefaultProfile;
            mountImagesCheckBox.Checked = profile.MountImages;
            escExitCheckBox.Checked = profile.EscapeToExit;
            suspendMPCheckBox.Checked = profile.SuspendMP == true;
            enableGMCheckBox.Checked = profile.EnableGoodmerge;
        }

        public override bool Next()
        {
            EmulatorProfile profile = Emulator.DefaultProfile;
            profile.MountImages = mountImagesCheckBox.Checked;
            profile.EscapeToExit = escExitCheckBox.Checked;
            profile.SuspendMP = suspendMPCheckBox.Checked;
            profile.EnableGoodmerge = enableGMCheckBox.Checked;
            return true;
        }
    }
}
