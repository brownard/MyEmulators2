using System;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace myEmulators
{
    internal partial class Conf_Emu_Details : Form
    {
        Emulator emulator;
        Image imgLogo;
        Image imgFanart;



        public Conf_Emu_Details(Emulator item)
        {
            emulator = item;
            InitializeComponent();

            ddlPlatform.DataSource = Dropdowns.GetDefinedSystems();
            ddlPlatform.ValueMember = "Value";
            ddlPlatform.DisplayMember = "Text";
        }

        public Emulator getEmulator()
        {
            return emulator;
        }

        private void Conf_Emu_Details_Load(object sender, EventArgs e)
        {
            resetInfoText(sender, e);
            if (emulator == null)
            {
                //Add mode
                Text = "Add emulator";
                //create emulator now to avoid null reference if trying to view art
                emulator = new Emulator();
                emulator.View = Options.Instance.GetIntOption("defaultviewroms");
            }
            else
            {
                //Edit mode
                Text = "Edit emulator";
                emuPathBox.Text = emulator.PathToEmulator;
                romPathBox.Text = emulator.PathToRoms;
                titleBox.Text = emulator.Title;
                filterBox.Text = emulator.Filter;
                workingBox.Text = emulator.WorkingFolder;
                argsBox.Text = emulator.Arguments;
                quotesBox.Checked = emulator.UseQuotes;
                suspendRenderingBox.Checked = emulator.SuspendRendering;

                chkEnableGoodmerge.Checked = emulator.EnableGoodmerge;
                ddlGoodmergePref1.Text = emulator.GoodmergePref1;
                ddlGoodmergePref2.Text = emulator.GoodmergePref2;
                ddlGoodmergePref3.Text = emulator.GoodmergePref3;

                ddl_Grade.Text = emulator.Grade.ToString();
                txt_Company.Text = emulator.Company;
                txt_Yearmade.Text = emulator.Yearmade.ToString();
                txt_Description.Text = emulator.Description;

                String ManualPath = ThumbsHandler.Instance.createEmulatorArt(emulator, "manual");

                if (ManualPath != "")
                {
                    btnManual.Enabled = true;
                }
                else
                {
                    btnManual.Enabled = false;
                }

                String logo = ThumbsHandler.Instance.createEmulatorArt(emulator, "Logo");
                if (logo != "")
                {
                    imgLogo = Bitmap.FromFile(logo);
                    pnlLogo.BackgroundImage = imgLogo;
                    txt_Logo.Text = logo;
                }

                String fanart = ThumbsHandler.Instance.createEmulatorArt(emulator, "Fanart");
                if (fanart != "")
                {
                    imgFanart = Bitmap.FromFile(fanart);
                    pnlFanart.BackgroundImage = imgFanart;
                    txt_Fanart.Text = fanart;
                }
            }
        }

        private void resetInfoText(object sender, EventArgs e)
        {
            infoLabel.Text = "Move your mouse over an option to get information.";
        }

        private void showEmuHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "The location in the file system where your emulator software is located. For example: C:\\Emulators\\SNES\\snes9xw.exe";
        }

        private void showRomHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "The location in the file system where you keep your games for this emulator. For example: C:\\Emulators\\SNES\\Games";
        }

        private void showTitleHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "This is the name that appears on the main screen, so name it whatever you like. For example: Nintendo 64.";
        }

        private void showFilterHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "If you want to show only certain game files from a folder, use this field. Separate each filter with a semi-colon. For example: *.sma;game?.smb;*.smc";
        }

        private void showWorkingHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "The emulator will use this folder as the current folder when executing, which is necessary to be set for Mame to work. If left empty, the folder of the emulator executable will be used.";
        }

        private void showArgsHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "Use this field to pass arguments to the emulator. The path to the game is inserted in the end, or you can specify %ROM% to have it where you want. For example: -arg1 %ROM% -arg2";
        }

        private void showQuotesHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "Only uncheck this if you can't get the games to start. This will omit the quotes around the path to the game when passing it as an argument to the emulator.";
        }

        private void showGoodmergeHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "Check this box to enable good merged set support";
        }

        private void showGoodmergePreferenceHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "The rules here will be used to find the ROM to execute from the set. If no ROM matches, the 1st will be loaded.";
        }

        private void showGoodmergeTempHelp(object sender, EventArgs e)
        {
            infoLabel.Text = "This directory will be used to store the extracted ROM. WARNING: The contents of this directory will be deleted when selecting a new ROM!";
        }

        private String lastBrowsedFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        private void emuPathButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.CheckFileExists = true;
            diag.CheckPathExists = true;
            diag.Filter = "Executables (*.bat, *.exe) | *.bat;*.exe";
            diag.InitialDirectory = lastBrowsedFolder;
            diag.Multiselect = false;
            diag.RestoreDirectory = true;
            diag.Title = "Select path to emulator";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                lastBrowsedFolder = diag.FileName.Remove(diag.FileName.LastIndexOf("\\"));
                emuPathBox.Text = diag.FileName;
            }
        }

        private void romPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            diag.Description = "Select path to folder with ROMs";
            diag.SelectedPath = lastBrowsedFolder;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                lastBrowsedFolder = diag.SelectedPath;
                romPathBox.Text = diag.SelectedPath;
            }
        }

        private void workingButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            diag.Description = "Select emulator working folder";
            diag.SelectedPath = lastBrowsedFolder;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                lastBrowsedFolder = diag.SelectedPath;
                workingBox.Text = diag.SelectedPath;
            }
        }

        private EmuAutoConf autoConf = new EmuAutoConf();

        private void emuPathBox_TextChanged(object sender, EventArgs e)
        {
            if (Options.Instance.GetBoolOption("autoconfemu"))
            {
                //A filename has been entered
                if (emuPathBox.Text.LastIndexOf(".") > emuPathBox.Text.LastIndexOf("\\"))
                {
                    //Contents of filter box has not been changed
                    if (filterBox.Text.Equals("*.*"))
                    {
                        String newFilter = autoConf.getFilterString(emuPathBox.Text.Substring(emuPathBox.Text.LastIndexOf("\\") + 1));
                        if (newFilter != null)
                        {
                            filterBox.Text = newFilter;
                        }
                    }
                }
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (emulator == null)
            {
                //shouldn't be hit as new emulator now created on load
                emulator = new Emulator();
                emulator.View = Options.Instance.GetIntOption("defaultviewroms");
            }

            bool founderror = false;

            if (emulator.UID != -1)
            {
                if (emuPathBox.Text.Length == 0 || romPathBox.Text.Length == 0 || titleBox.Text.Length == 0 || filterBox.Text.Length == 0)
                {
                    MessageBox.Show("Please make sure no required field is empty", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    founderror = true;
                }
                else if (!(emuPathBox.Text.EndsWith(".exe") || emuPathBox.Text.EndsWith(".bat")) || !File.Exists(emuPathBox.Text))
                {
                    MessageBox.Show("Please specify a valid emulator executable path", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    founderror = true;
                }
                else if (!Directory.Exists(romPathBox.Text))
                {
                    MessageBox.Show("Please specify an existing folder where the ROMs are", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    founderror = true;
                }
                else if (workingBox.Text.Length > 0 && !Directory.Exists(workingBox.Text))
                {
                    MessageBox.Show("Please specify an existing folder for the working folder", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    founderror = true;
                }
                else if (chkEnableGoodmerge.Checked && txtGoodmergeTemp.Text.Length == 0 && !Directory.Exists(txtGoodmergeTemp.Text))
                {
                    MessageBox.Show("Please specify an existing folder for the goodmerge temp directory", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    founderror = true;
                }
            }

            if (!founderror)
            {
                emulator.PathToEmulator = emuPathBox.Text;
                emulator.PathToRoms = romPathBox.Text;
                emulator.Title = titleBox.Text;
                emulator.Filter = filterBox.Text;
                emulator.WorkingFolder = workingBox.Text;
                emulator.Arguments = argsBox.Text;
                emulator.UseQuotes = quotesBox.Checked;
                emulator.SuspendRendering = suspendRenderingBox.Checked;

                emulator.EnableGoodmerge = chkEnableGoodmerge.Checked;
                emulator.GoodmergePref1 = ddlGoodmergePref1.Text;
                emulator.GoodmergePref2 = ddlGoodmergePref2.Text;
                emulator.GoodmergePref3 = ddlGoodmergePref3.Text;

                emulator.EnableGoodmerge = chkEnableGoodmerge.Checked;

                try
                {
                    emulator.Grade = Convert.ToInt32(ddl_Grade.Text);
                }
                catch
                {
                    emulator.Grade = 0;
                }

                emulator.Company = txt_Company.Text;

                try
                {
                    emulator.Yearmade = Convert.ToInt32(txt_Yearmade.Text);
                }
                catch { }

                emulator.Description = txt_Description.Text;

                string SavePath = ThumbsHandler.Instance.thumb_emulators + @"\" + emulator.Title;
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()[1];
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }

                if (txt_Manual.Text != "")
                {
                    try
                    {
                        String extension = txt_Manual.Text.Substring(txt_Manual.Text.Length - 3);
                        File.Copy(txt_Manual.Text, SavePath + @"\Manual." + extension, true);
                        txt_Manual.Text = "";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not copy the file to manual location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (pnlLogo.BackgroundImage != null)
                {
                    try
                    {
                        Image saveBmp = null;
                        bool save = false;
                        if (txt_Logo.Text == "" || (!txt_Logo.Text.EndsWith(".jpg") && !txt_Logo.Text.EndsWith(".png")))
                        {
                            saveBmp = ImageHandler.Instance.NewImage(pnlLogo.BackgroundImage);
                            save = true;
                        }
                        try
                        {
                            pnlLogo.BackgroundImage.Dispose();
                        }
                        catch { }

                        pnlLogo.BackgroundImage = null;

                        try
                        {
                            imgLogo.Dispose();
                        }
                        catch { }

                        if (save)
                        {
                            saveBmp.Save(SavePath + @"\Logo.jpg", jpegCodec, encoderParams);
                            saveBmp.Dispose();
                        }
                        else
                        {
                            string destinationFile = SavePath + @"\Logo." + txt_Logo.Text.Substring(txt_Logo.Text.Length - 3).ToLower();
                            if (txt_Logo.Text != destinationFile)
                                File.Copy(txt_Logo.Text, destinationFile, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not copy the file to thumbnail location." + ex.Message, "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        File.Delete(SavePath + @"\Logo.jpg");
                    }
                    catch { }

                    try
                    {
                        File.Delete(SavePath + @"\Logo.png");
                    }
                    catch { }
                }

                if (pnlFanart.BackgroundImage != null)
                {
                    try
                    {
                        Image saveBmp = null;
                        bool save = false;
                        if (txt_Fanart.Text == "" || (!txt_Fanart.Text.EndsWith(".jpg") && !txt_Fanart.Text.EndsWith(".png")))
                        {
                            saveBmp = ImageHandler.Instance.NewImage(pnlFanart.BackgroundImage);
                            save = true;
                        }

                        try
                        {
                            pnlFanart.BackgroundImage.Dispose();
                        }
                        catch { }

                        pnlFanart.BackgroundImage = null;

                        try
                        {
                            imgFanart.Dispose();
                        }
                        catch { }

                        if (save)
                        {
                            saveBmp.Save(SavePath + @"\Fanart.jpg", jpegCodec, encoderParams);
                            saveBmp.Dispose();
                        }
                        else
                        {
                            string destinationFile = SavePath + @"\Fanart." + txt_Fanart.Text.Substring(txt_Fanart.Text.Length - 3).ToLower();
                            if (txt_Fanart.Text != destinationFile)
                                File.Copy(txt_Fanart.Text, destinationFile, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not copy the file to thumbnail location." + ex.Message, "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        File.Delete(SavePath + @"\Fanart.jpg");
                    }
                    catch { }

                    try
                    {
                        File.Delete(SavePath + @"\Fanart.png");
                    }
                    catch { }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Conf_Emu_Details_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                imgLogo.Dispose();
            }
            catch { }

            imgLogo = null;

            try
            {
                imgFanart.Dispose();
            }
            catch { }

            imgFanart = null;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGoodmergeBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            diag.Description = "Select goodmerge temporary extract directory";
            diag.SelectedPath = lastBrowsedFolder;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                lastBrowsedFolder = diag.SelectedPath;
                txtGoodmergeTemp.Text = diag.SelectedPath;
            }
        }

        private void btnNewManual_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select thumbnail image";
            diag.Filter = "PDF files (*.pdf) | *.pdf";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                txt_Manual.Text = diag.FileName;
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Executor.launchDocument(emulator);
        }

        private void txt_Manual_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void txt_Manual_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                txt_Manual.Text = files[0];
            }
        }

        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            Panel destination = (Panel)sender;

            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                destination.BackgroundImage = (Bitmap)e.Data.GetData(typeof(Bitmap));

                //make sure we don't leave any references to old files
                switch (destination.Name)
                {
                    case "pnlFanart":
                        {
                            txt_Fanart.Text = "";
                            break;
                        }
                    case "pnlLogo":
                        {
                            txt_Logo.Text = "";
                            break;
                        }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                destination.BackgroundImage = new Bitmap(files[0]);

                switch (destination.Name)
                {
                    case "pnlFanart":
                        {
                            txt_Fanart.Text = files[0];
                            break;
                        }
                    case "pnlLogo":
                        {
                            txt_Logo.Text = files[0];
                            break;
                        }
                }
            }
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            try
            {
                pnlLogo.BackgroundImage.Dispose();
            }
            catch { }

            pnlLogo.BackgroundImage = null;

            try
            {
                imgLogo.Dispose();
            }
            catch { }

            txt_Logo.Text = "";
        }

        private void btnDelFanart_Click(object sender, EventArgs e)
        {
            try
            {
                pnlFanart.BackgroundImage.Dispose();
            }
            catch { }

            pnlFanart.BackgroundImage = null;

            try
            {
                imgFanart.Dispose();
            }
            catch { }

            txt_Fanart.Text = "";
        }

        private void ddlPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPlatform.SelectedIndex != 0)
            {
                titleBox.Text = ddlPlatform.Text;
            }
        }

        private void btnLogoView_Click(object sender, EventArgs e)
        {
            String logoStr; //= ThumbsHandler.Instance.createEmulatorArt(emulator, "Logo");
            if (txt_Logo.Text != "")
                logoStr = txt_Logo.Text; //load image directly from source
            else
            {
                if (pnlLogo.BackgroundImage == null)
                    return;
                //create temp image and load that
                Bitmap logo = new Bitmap(pnlLogo.BackgroundImage);
                string savePath = Path.Combine(Path.GetTempPath(), "myEmulators2." + Guid.NewGuid().ToString() + ".bmp");
                try
                {
                    logo.Save(savePath, ImageFormat.Bmp);
                    logo.Dispose();
                }
                catch
                {
                    return;
                }
                logoStr = savePath;
            }

            Process proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = logoStr;
            proc.Start();
        }

        private void btnFanartView_Click(object sender, EventArgs e)
        {
            String fanart = ThumbsHandler.Instance.createEmulatorArt(emulator, "Fanart");
            if (fanart != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = fanart;
                proc.Start();
            }
        }
    }
}

