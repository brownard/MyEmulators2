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
    public partial class Wzd_NewEmu_Main : Wzd_Main
    {
        Emulator newEmu = null;
        public Emulator NewEmulator
        {
            get { return newEmu; }
        }

        Wzd_NewEmu_Info infoPanel = null;

        public Wzd_NewEmu_Main()
        {
            InitializeComponent();

            this.Text = "New Emulator";

            newEmu = new Emulator();
            infoPanel = new Wzd_NewEmu_Info(newEmu);

            panels = new List<WzdPanel>(new WzdPanel[] { new Wzd_NewEmu_Start(newEmu), infoPanel, new Wzd_NewEmu_Config(newEmu), new Wzd_NewEmu_Config2(newEmu), new Wzd_NewEmu_Roms(newEmu) });
            panel1.Controls.Add(panels[0]);
            updateButtons();
        }

        public Image Logo
        {
            get { return infoPanel.Logo; }
        }

        public Image Fanart
        {
            get { return infoPanel.Fanart; }
        }
    }
}
