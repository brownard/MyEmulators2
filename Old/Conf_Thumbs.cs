using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_Thumbs : myEmulators.ContentPanel
    {
        public Conf_Thumbs()
        {
            InitializeComponent();
        }

        private void Conf_Thumbs_Load(object sender, EventArgs e)
        {
            update();
        }

        public override void update()
        {
            comboBox1.Items.Clear();
            foreach (Emulator emu in DB.Instance.GetEmulators())
            {
                comboBox1.Items.Add("----------------------------------------------");
                comboBox1.Items.Add(emu);
                comboBox1.Items.Add("----------------------------------------------");
                //foreach (Game game in DB.getGames(emu.PathToRoms, emu, SearchOption.AllDirectories, false))
                //{
                    //comboBox1.Items.Add(game);
                //}
            }

            foreach (Game game in DB.Instance.GetGames())
            {
                comboBox1.Items.Add(game);
            }

            base.update();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.GetType() == typeof(Emulator))
            {
                pathText.Enabled = true;
                browse.Enabled = true;
                apply.Enabled = true;
                txt_FanArt.Enabled = true;
                btnBrowseFanart.Enabled = true;
                btnTitleScreen.Enabled = true;
                btnIngameScreenshot.Enabled = true;
                btnBrowseBack.Enabled = true;
                txt_TitleScreen.Enabled = true;
                txt_IngameScreenshot.Enabled = true;
                txt_BoxBack.Enabled = true;

                ExtendedGUIListItem item = ThumbsHandler.Instance.createEmulatorFacadeItem((Emulator)comboBox1.SelectedItem);
                if (item.ThumbnailImage != null)
                {
                    pictureBox1.ImageLocation = item.ThumbnailImage;
                }
                else
                {
                    pictureBox1.Image = null;
                }

                String FanartPath = ThumbsHandler.Instance.createEmulatorArt((Emulator)comboBox1.SelectedItem, "fanart");
                if (FanartPath != "")
                {
                    pbFanart.ImageLocation = FanartPath;
                }
                else
                {
                    pbFanart.Image = null;
                }

                String BackCoverPath = ThumbsHandler.Instance.createEmulatorArt((Emulator)comboBox1.SelectedItem, "backcover");
                if (BackCoverPath != "")
                {
                    pictureBoxBack.ImageLocation = BackCoverPath;
                }
                else
                {
                    pictureBoxBack.Image = null;
                }

                String TitleScreenPath = ThumbsHandler.Instance.createEmulatorArt((Emulator)comboBox1.SelectedItem, "titlescreen");
                if (TitleScreenPath != "")
                {
                    pictureTitleScreen.ImageLocation = TitleScreenPath;
                }
                else
                {
                    pictureTitleScreen.Image = null;
                }

                String IngameScreenPath = ThumbsHandler.Instance.createEmulatorArt((Emulator)comboBox1.SelectedItem, "ingamescreen");
                if (IngameScreenPath != "")
                {
                    pictureIngameScreenshot.ImageLocation = IngameScreenPath;
                }
                else
                {
                    pictureIngameScreenshot.Image = null;
                }

                btnNewManual.Enabled = false;
                btnManual.Enabled = false;
            }
            else if (comboBox1.SelectedItem.GetType() == typeof(Game))
            {
                pathText.Enabled = true;
                browse.Enabled = true;
                apply.Enabled = true;
                txt_FanArt.Enabled = true;
                btnBrowseFanart.Enabled = true;
                btnTitleScreen.Enabled = true;
                btnIngameScreenshot.Enabled = true;
                btnBrowseBack.Enabled = true;
                txt_TitleScreen.Enabled = true;
                txt_IngameScreenshot.Enabled = true;
                txt_BoxBack.Enabled = true;
                btnNewManual.Enabled = true;
                txt_Manual.Enabled = true;

                String BoxFront = ThumbsHandler.Instance.createGameArt((Game)comboBox1.SelectedItem, "BoxFront", false);
                if (BoxFront != "")
                {
                    pictureBox1.ImageLocation = BoxFront;
                }
                else
                {
                    pictureBox1.Image = null;
                }

                String FanartPath = ThumbsHandler.Instance.createGameArt((Game)comboBox1.SelectedItem, "Fanart", false);
                if (FanartPath != "")
                {
                    pbFanart.ImageLocation = FanartPath;
                }
                else
                {
                    pbFanart.Image = null;
                }

                String BackCoverPath = ThumbsHandler.Instance.createGameArt((Game)comboBox1.SelectedItem, "BoxBack", false);
                if (BackCoverPath != "")
                {
                    pictureBoxBack.ImageLocation = BackCoverPath;
                }
                else
                {
                    pictureBoxBack.Image = null;
                }

                String TitleScreenPath = ThumbsHandler.Instance.createGameArt((Game)comboBox1.SelectedItem, "TitleScreenshot", false);
                if (TitleScreenPath != "")
                {
                    pictureTitleScreen.ImageLocation = TitleScreenPath;
                }
                else
                {
                    pictureTitleScreen.Image = null;
                }

                String IngameScreenPath = ThumbsHandler.Instance.createGameArt((Game)comboBox1.SelectedItem, "IngameScreenshot", false);
                if (IngameScreenPath != "")
                {
                    pictureIngameScreenshot.ImageLocation = IngameScreenPath;
                }
                else
                {
                    pictureIngameScreenshot.Image = null;
                }

                String ManualPath = ThumbsHandler.Instance.createGameArt((Game)comboBox1.SelectedItem, "manual", false);
                if (ManualPath != "")
                {
                    btnManual.Enabled = true;
                }
                else
                {
                    btnManual.Enabled = false;
                }
            }
            else
            {
                pathText.Enabled = false;
                txt_FanArt.Enabled = false;
                browse.Enabled = false;
                btnBrowseFanart.Enabled = false;
                apply.Enabled = false;
                btnTitleScreen.Enabled = false;
                btnIngameScreenshot.Enabled = false;
                btnBrowseBack.Enabled = false;
                txt_TitleScreen.Enabled = false;
                txt_Manual.Enabled = false;
                txt_IngameScreenshot.Enabled = false;
                txt_BoxBack.Enabled = false;
                btnNewManual.Enabled = false;
                btnManual.Enabled = false;

                pictureBox1.Image = null;
                pbFanart.Image = null;
                pictureTitleScreen.Image = null;
                pictureIngameScreenshot.Image = null;
                pictureBoxBack.Image = null;
            }

            pathText.Text = "";
            txt_IngameScreenshot.Text = "";
            txt_FanArt.Text = "";
            txt_BoxBack.Text = "";
            txt_TitleScreen.Text = "";
        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select thumbnail image";
            diag.Filter = "Image files (*.gif, *.jpg, *.png) | *.gif;*.jpg;*.png";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                pathText.Text = diag.FileName;
            }
        }

        private void apply_Click(object sender, EventArgs e)
        {
            if (pathText.Text != "")
            {
                try
                {
                    String extension = pathText.Text.Substring(pathText.Text.Length - 3);
                    if (comboBox1.SelectedItem.GetType() == typeof(Emulator))
                    {
                        File.Copy(pathText.Text, ThumbsHandler.Instance.thumb_emulators + @"\" + ((Emulator)comboBox1.SelectedItem).Title + "." + extension, true);
                        pathText.Text = "";
                    }
                    else if (comboBox1.SelectedItem.GetType() == typeof(Game))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title);
                        }
                        File.Copy(pathText.Text, ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\" + ((Game)comboBox1.SelectedItem).Filename + "." + extension, true);
                        pathText.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to thumbnail location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (txt_FanArt.Text != "")
            {
                try
                {
                    String extension = txt_FanArt.Text.Substring(txt_FanArt.Text.Length - 3);
                    if (comboBox1.SelectedItem.GetType() == typeof(Emulator))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_emulators + @"\fanart"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_emulators + @"\fanart");
                        }

                        File.Copy(txt_FanArt.Text, ThumbsHandler.Instance.thumb_emulators + @"\fanart\" + ((Emulator)comboBox1.SelectedItem).Title + "." + extension, true);
                        txt_FanArt.Text = "";
                    }
                    else if (comboBox1.SelectedItem.GetType() == typeof(Game))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title);
                        }

                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\fanart\"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\fanart\");
                        }

                        File.Copy(txt_FanArt.Text, ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\fanart\" + ((Game)comboBox1.SelectedItem).Filename + "." + extension, true);
                        txt_FanArt.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to thumbnail location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (txt_BoxBack.Text != "")
            {
                try
                {
                    String extension = txt_BoxBack.Text.Substring(txt_BoxBack.Text.Length - 3);
                    if (comboBox1.SelectedItem.GetType() == typeof(Emulator))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_emulators + @"\backcover"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_emulators + @"\backcover");
                        }

                        File.Copy(txt_BoxBack.Text, ThumbsHandler.Instance.thumb_emulators + @"\backcover\" + ((Emulator)comboBox1.SelectedItem).Title + "." + extension, true);
                        txt_BoxBack.Text = "";
                    }
                    else if (comboBox1.SelectedItem.GetType() == typeof(Game))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title);
                        }

                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\backcover\"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\backcover\");
                        }

                        File.Copy(txt_BoxBack.Text, ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\backcover\" + ((Game)comboBox1.SelectedItem).Filename + "." + extension, true);
                        txt_BoxBack.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to thumbnail location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (txt_TitleScreen.Text != "")
            {
                try
                {
                    String extension = txt_TitleScreen.Text.Substring(txt_TitleScreen.Text.Length - 3);
                    if (comboBox1.SelectedItem.GetType() == typeof(Emulator))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_emulators + @"\titlescreen"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_emulators + @"\titlescreen");
                        }

                        File.Copy(txt_TitleScreen.Text, ThumbsHandler.Instance.thumb_emulators + @"\titlescreen\" + ((Emulator)comboBox1.SelectedItem).Title + "." + extension, true);
                        txt_TitleScreen.Text = "";
                    }
                    else if (comboBox1.SelectedItem.GetType() == typeof(Game))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title);
                        }

                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\titlescreen\"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\titlescreen\");
                        }

                        File.Copy(txt_TitleScreen.Text, ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\titlescreen\" + ((Game)comboBox1.SelectedItem).Filename + "." + extension, true);
                        txt_TitleScreen.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to thumbnail location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (txt_IngameScreenshot.Text != "")
            {
                try
                {
                    String extension = txt_IngameScreenshot.Text.Substring(txt_IngameScreenshot.Text.Length - 3);
                    if (comboBox1.SelectedItem.GetType() == typeof(Emulator))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_emulators + @"\ingamescreen"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_emulators + @"\ingamescreen");
                        }

                        File.Copy(txt_IngameScreenshot.Text, ThumbsHandler.Instance.thumb_emulators + @"\ingamescreen\" + ((Emulator)comboBox1.SelectedItem).Title + "." + extension, true);
                        txt_IngameScreenshot.Text = "";
                    }
                    else if (comboBox1.SelectedItem.GetType() == typeof(Game))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title);
                        }

                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\ingamescreen\"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\ingamescreen\");
                        }

                        File.Copy(txt_IngameScreenshot.Text, ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\ingamescreen\" + ((Game)comboBox1.SelectedItem).Filename + "." + extension, true);
                        txt_IngameScreenshot.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to thumbnail location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (txt_Manual.Text != "")
            {
                try
                {
                    String extension = txt_Manual.Text.Substring(txt_Manual.Text.Length - 3);
                    
                    if (comboBox1.SelectedItem.GetType() == typeof(Game))
                    {
                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title);
                        }

                        if (!Directory.Exists(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\manual\"))
                        {
                            Directory.CreateDirectory(ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\manual\");
                        }

                        File.Copy(txt_Manual.Text, ThumbsHandler.Instance.thumb_games + @"\" + ((Game)comboBox1.SelectedItem).ParentEmulator.Title + @"\manual\" + ((Game)comboBox1.SelectedItem).Filename + "." + extension, true);
                        txt_Manual.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not copy the file to manual location.", "Error when copying", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            comboBox1_SelectedIndexChanged(sender, e);
        }

        private void btnBrowseFanart_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select fanart image";
            diag.Filter = "Image files (*.gif, *.jpg, *.png) | *.gif;*.jpg;*.png";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                txt_FanArt.Text = diag.FileName;
            }
        }

        private void btnBrowseBack_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select thumbnail image";
            diag.Filter = "Image files (*.gif, *.jpg, *.png) | *.gif;*.jpg;*.png";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                txt_BoxBack.Text = diag.FileName;
            }
        }

        private void btnTitleScreen_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select thumbnail image";
            diag.Filter = "Image files (*.gif, *.jpg, *.png) | *.gif;*.jpg;*.png";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                txt_TitleScreen.Text = diag.FileName;
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

        private void btnIngameScreenshot_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Title = "Select thumbnail image";
            diag.Filter = "Image files (*.gif, *.jpg, *.png) | *.gif;*.jpg;*.png";
            diag.RestoreDirectory = true;
            diag.ValidateNames = true;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                txt_IngameScreenshot.Text = diag.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Executor.launchDocument((Game)comboBox1.SelectedItem);
        }
    }
}
