using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;

namespace myEmulators
{
    public partial class Conf_PC_Details : Form
    {
        Emulator emulator;
        Image imgLogo;
        Image imgFanart;

        public Conf_PC_Details(Emulator item)
        {
            emulator = item;
            InitializeComponent();
        }

        public Emulator getEmulator()
        {
            return emulator;
        }

        private void Conf_PC_Details_Load(object sender, EventArgs e)
        {
            Text = "Edit PC";
            if (emulator != null)
            {
                titleBox.Text = emulator.Title;
                txt_Description.Text = emulator.Description;

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

        private void save_Click(object sender, EventArgs e)
        {
            if (emulator == null)
            {
                emulator = new Emulator();
                emulator.View = Options.Instance.GetIntOption("defaultviewroms");
            }

            bool founderror = false;

            if (titleBox.Text.Length == 0)
            {
                MessageBox.Show("Please make sure no required field is empty", "Form not entered correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                founderror = true;
            }

            if (!founderror)
            {
                emulator.Title = titleBox.Text;
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

                if (pnlLogo.BackgroundImage != null)
                {
                    try
                    {
                        bool save = false;
                        Image saveBmp = null;
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

        private void Conf_PC_Details_FormClosed(object sender, FormClosedEventArgs e)
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

        private void btnLogoView_Click(object sender, EventArgs e)
        {
            String logo = ThumbsHandler.Instance.createEmulatorArt(emulator, "Logo");
            if (logo != "")
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = logo;
                proc.Start();
            }
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
