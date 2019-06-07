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
    internal partial class Wzd_NewEmu_Info : WzdPanel
    {
        Image logo = null;
        public Image Logo
        {
            get { return logo; }
            protected set { logo = value; }
        }

        Image fanart = null;
        public Image Fanart
        {
            get { return fanart; }
            protected set { fanart = value; }
        }

        public Wzd_NewEmu_Info(Emulator emu)
        {
            InitializeComponent();
            platformComboBox.DisplayMember = "Text";
            platformComboBox.DataSource = Dropdowns.GetSystems();
            txt_Title.Text = "New Emulator";
            this.Emulator = emu;
        }

        bool firstLoad = true;
        public override void UpdatePanel()
        {
            if (!string.IsNullOrEmpty(Emulator.Title))
                txt_Title.Text = Emulator.Title;

            int index = platformComboBox.FindStringExact(Emulator.PlatformTitle);
            if (index > -1)
                platformComboBox.SelectedIndex = index;
            
            EmuSettingsAutoFill.SetupAspectDropdown(thumbAspectComboBox, Emulator.CaseAspect);

            if (firstLoad)
            {
                firstLoad = false;
                if (platformComboBox.SelectedIndex > 0)
                    updateEmuInfo();
            }
        }

        public override bool Next()
        {
            Emulator.Title = txt_Title.Text;
            Emulator.PlatformTitle = platformComboBox.Text;
            Emulator.Company = txt_company.Text;
            Emulator.Description = txt_description.Text;
            
            int year;
            if (!int.TryParse(txt_yearmade.Text, out year))
                year = 0;
            Emulator.Year = year;

            Emulator.Grade = (int)gradeUpDown.Value;
            Emulator.SetCaseAspect(thumbAspectComboBox.Text);

            return true;
        }

        public override bool Back()
        {
            return true;
        }

        private void updateInfoButton_Click(object sender, EventArgs e)
        {
            updateEmuInfo();
        }

        void updateEmuInfo()
        {
            EmulatorInfo lEmuInfo = new EmulatorScraperHandler().UpdateEmuInfo(platformComboBox.Text, (o) =>
                {
                    EmulatorInfo emuInfo = (EmulatorInfo)o;
                    if (logo != null)
                    {
                        logo.Dispose();
                        logo = null;
                    }
                    if (fanart != null)
                    {
                        fanart.Dispose();
                        fanart = null;
                    }
                    logo = ImageHandler.BitmapFromWeb(emuInfo.LogoUrl);
                    fanart = ImageHandler.BitmapFromWeb(emuInfo.FanartUrl);
                    return true;
                });                      

            if (lEmuInfo != null)
            {
                txt_Title.Text = lEmuInfo.Title;
                txt_company.Text = lEmuInfo.Developer;
                txt_description.Text = lEmuInfo.GetDescription();
                int grade;
                if (int.TryParse(lEmuInfo.Grade, out grade))
                    gradeUpDown.Value = grade;

                return;
            }
        }
    }
}
