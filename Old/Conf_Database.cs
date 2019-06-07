using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_Database : myEmulators.ContentPanel
    {
        public Conf_Database()
        {
            InitializeComponent();
            //ddlPlatform.DataSource = Dropdowns.GetDefinedSystems();
            //ddlPlatform.ValueMember = "Value";
            //ddlPlatform.DisplayMember = "Text";
        }

        private void Conf_Database_Load(object sender, EventArgs e)
        {
            toggleSearch();
            update();
        }

        public override void update()
        {
            int SelectedSystem = -2;
            string SelectedSystemStr = null;
            if (ddlPlatform.Items.Count > 0)
            {
                try
                {
                    SelectedSystem = Convert.ToInt32(ddlPlatform.SelectedValue.ToString()); //get currently selected emulators
                    SelectedSystemStr = ddlPlatform.Text;
                }
                catch { }
            }

            ddlPlatform.SelectedIndexChanged -= new EventHandler(this.ddlPlatform_SelectedIndexChanged);
            ddlPlatform.DataSource = Dropdowns.GetDefinedSystems(); //refresh list of emulators, do this after getting currently selected emulator as 'SelectedValue' is reset
            ddlPlatform.ValueMember = "Value";
            ddlPlatform.DisplayMember = "Text";

            if (SelectedSystemStr != null)
            {
                int index = ddlPlatform.FindStringExact(SelectedSystemStr);
                if (index > -1)
                    ddlPlatform.SelectedIndex = index; //re-select current emulator
                else
                    SelectedSystem = -2; //current emulator has been deleted, set to all emulators
            }
            ddlPlatform.SelectedIndexChanged += new EventHandler(this.ddlPlatform_SelectedIndexChanged);

            dataGridView1.CellValueChanged -= new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            dataGridView1.Rows.Clear();
            //Emulator games

            if (SelectedSystem == -2)
            {
                foreach (Game game in DB.Instance.GetGames())
                {
                    addGameRow(game);
                }
            }
            else
            {
                Emulator currentEmulator = DB.Instance.GetEmulator(SelectedSystem);

                foreach (Game game in DB.Instance.GetGames(currentEmulator))
                {
                    addGameRow(game);
                }
            }

            /*
            foreach (Emulator emu in DB.getEmulators())
            {
                foreach (Game game in DB.getGames(emu.PathToRoms, emu, System.IO.SearchOption.AllDirectories, true))
                {
                    addGameRow(game);
                }
            }
             */
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            base.update();
        }

        private void addGameRow(Game game)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = game;

            DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
            cell1.Value = game.Path;
            cell1.Style.BackColor = System.Drawing.Color.LightGray;
            row.Cells.Add(cell1);

            DataGridViewButtonCell cell2 = new DataGridViewButtonCell();
            cell2.Value = "0";
            row.Cells.Add(cell2);

            DataGridViewButtonCell cell3 = new DataGridViewButtonCell();
            cell3.Value = "L";
            row.Cells.Add(cell3);


            DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
            cell4.Value = game.Title;
            row.Cells.Add(cell4);

            DataGridViewComboBoxCell cell5 = new DataGridViewComboBoxCell();
            cell5.Items.Add("0");
            cell5.Items.Add("1");
            cell5.Items.Add("2");
            cell5.Items.Add("3");
            cell5.Items.Add("4");
            cell5.Items.Add("5");
            cell5.Items.Add("6");
            cell5.Items.Add("7");
            cell5.Items.Add("8");
            cell5.Items.Add("9");
            cell5.Items.Add("10");
            cell5.Value = "" + game.Grade;
            row.Cells.Add(cell5);

            DataGridViewTextBoxCell cell6 = new DataGridViewTextBoxCell();
            cell6.Value = game.Yearmade;
            row.Cells.Add(cell6);

            DataGridViewTextBoxCell cell7 = new DataGridViewTextBoxCell();
            cell7.Value = game.Description;
            row.Cells.Add(cell7);

            DataGridViewTextBoxCell cell8 = new DataGridViewTextBoxCell();
            cell8.Value = game.Genre;
            row.Cells.Add(cell8);

            DataGridViewTextBoxCell cell9 = new DataGridViewTextBoxCell();
            cell9.Value = game.Company;
            row.Cells.Add(cell9);

            DataGridViewCheckBoxCell cell10 = new DataGridViewCheckBoxCell();
            cell10.Value = game.Favourite;
            row.Cells.Add(cell10);

            DataGridViewCheckBoxCell cell11 = new DataGridViewCheckBoxCell();
            cell11.Value = game.Visible;
            row.Cells.Add(cell11);

            dataGridView1.Rows.Add(row);
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                changedRows.Add(dataGridView1.Rows[e.RowIndex]);
                OnChange(sender, EventArgs.Empty);
            }
            catch (Exception) { }
        }

        private List<DataGridViewRow> changedRows = new List<DataGridViewRow>();

        public override void save()
        {
            foreach (DataGridViewRow row in changedRows)
            {
                Game game = (Game)row.Tag;
                game.Title = (String)row.Cells[3].Value;
                game.Grade = Int32.Parse(row.Cells[4].Value.ToString());
                try
                {
                    game.Yearmade = Int32.Parse((String)row.Cells[5].Value);
                }
                catch (Exception) { }
                game.Description = (String)row.Cells[6].Value;
                game.Genre = (String)row.Cells[7].Value;
                game.Company = (String)row.Cells[8].Value;
                game.Favourite = (bool)row.Cells[9].Value;
                game.Visible = (bool)row.Cells[10].Value;
                game.Save();
            }
            changedRows = new List<DataGridViewRow>();
            base.save();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[3].Value.ToString().ToLower().Contains(searchBox.Text.ToLower()))
                {
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }
        }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                toggleSearch();
            }
        }

        private void toggleSearch()
        {
            //if (!searchBox.Visible)
            //{
                dataGridView1.Height = this.Height - 32;
                searchLabel.Visible = false;
                searchBox.Visible = false;
                //searchBox.Focus();
            //}
            //else
            //{
                //dataGridView1.Height = this.Height - 4;
                //searchLabel.Visible = false;
                //searchBox.Visible = false;
                //searchBox.Text = "";
            //}
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                toggleSearch();
            }
            else
            {
                keyDown(sender, e);
            }
        }

        public override void resize(object sender, EventArgs e)
        {
            base.resize(sender, e);
            dataGridView1.Width = this.Width - 4;
            searchLabel.Top = this.Height - 25;
            searchBox.Top = this.Height - 28;

            btnUpdateAll.Top = this.Height - 28;
            btnUpdateWithoutData.Top = this.Height - 28;
            btnBatchNew.Top = this.Height - 28;
            btnBatchAll.Top = this.Height - 28;

            ddlPlatform.Top = this.Height - 28;

            //if (!searchBox.Visible)
            //{
                //dataGridView1.Height = this.Height - 4;
            //}
            //else
            //{
                dataGridView1.Height = this.Height - 32;
            //}
        }


        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            bool FoundStart = false;

            bool aborted = false;
            bool unattended = false;

            //check if we want to run an unnattended import
            DialogResult uaDlg = MessageBox.Show("Run unattended import based on import settings?", "", MessageBoxButtons.YesNo);
            if (uaDlg == DialogResult.Yes)
                unattended = true;

            List<string> skipped = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Selected) //start at selected row
                {
                    FoundStart = true;
                }

                if (FoundStart)
                {

                    Game selected = (Game)row.Tag;

                    Conf_OnlineLookup detailsForm = new Conf_OnlineLookup(selected, false, unattended);
                    detailsForm.StartPosition = FormStartPosition.CenterScreen;
                    DialogResult dlg = detailsForm.ShowDialog();

                    if (dlg == DialogResult.Abort) //user aborted, quit loop
                    {
                        aborted = true;
                        break;
                    }
                    if (unattended && dlg != DialogResult.OK) //no match was found during unattended import
                    {
                        skipped.Add(selected.Title);
                    }
                }
            }

            
            if (unattended)
            {
                //show list of all items that were skipped
                Conf_UnattendedResult uaResults = new Conf_UnattendedResult(string.Format("Import {0}", aborted ? "aborted" : "finished"), skipped);
                uaResults.ShowDialog();
                uaResults.Dispose();
            }
            

            update();
        }

        private void btnUpdateWithoutData_Click(object sender, EventArgs e)
        {
            bool aborted = false;
            bool unattended = false;

            List<string> skipped = new List<string>();

            DialogResult uaDlg = MessageBox.Show("Run unattended import based on import settings?", "", MessageBoxButtons.YesNo);
            if (uaDlg == DialogResult.Yes)
                unattended = true;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Game selected = (Game)row.Tag;

                if (selected.Yearmade == 0)
                {
                    Conf_OnlineLookup detailsForm = new Conf_OnlineLookup(selected, false, unattended);
                    detailsForm.StartPosition = FormStartPosition.CenterScreen;
                    DialogResult dlg = detailsForm.ShowDialog();
                    if (dlg == DialogResult.Abort)
                    {
                        aborted = true;
                        break;
                    }
                    if (unattended && dlg != DialogResult.OK)
                    {
                        skipped.Add(selected.Title);
                    }
                }
            }

            
            if (unattended)
            {
                Conf_UnattendedResult uaResults = new Conf_UnattendedResult(string.Format("Import {0}", aborted ? "aborted" : "finished"), skipped);
                uaResults.ShowDialog();
                uaResults.Dispose();
            }
            

            update();
        }

        private void btnBatchNew_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Game selected = (Game)row.Tag;

                bool NeedsUpdate = false;

                if (selected.Yearmade == 0)
                {
                    NeedsUpdate = true;
                }
                else if (selected.Grade == 0)
                {
                    NeedsUpdate = true;
                }
                else if (selected.Genre == "")
                {
                    NeedsUpdate = true;
                }
                else if (selected.Description == "")
                {
                    NeedsUpdate = true;
                }
                else if (selected.Company == "")
                {
                    NeedsUpdate = true;
                }
                else if (ThumbsHandler.Instance.createGameArt(selected, "BoxFront", false) == "")
                {
                    NeedsUpdate = true;
                }
                else if (ThumbsHandler.Instance.createGameArt(selected, "BoxBack", false) == "")
                {
                    NeedsUpdate = true;
                }
                else if (ThumbsHandler.Instance.createGameArt(selected, "TitleScreenshot", false) == "")
                {
                    NeedsUpdate = true;
                }
                else if (ThumbsHandler.Instance.createGameArt(selected, "IngameScreenshot", false) == "")
                {
                    NeedsUpdate = true;
                }

                if (NeedsUpdate)
                {
                    Conf_OnlineLookup detailsForm = new Conf_OnlineLookup(selected, true);
                    detailsForm.StartPosition = FormStartPosition.CenterScreen;
                    if (detailsForm.ShowDialog() == DialogResult.Abort)
                    {
                        break;
                    }
                }
            }

            update();
        }

        private void btnBatchAll_Click(object sender, EventArgs e)
        {
            bool FoundStart = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Selected)
                {
                    FoundStart = true;
                }

                if (FoundStart)
                {
                    Game selected = (Game)row.Tag;

                    Conf_OnlineLookup detailsForm = new Conf_OnlineLookup(selected, true);
                    detailsForm.StartPosition = FormStartPosition.CenterScreen;
                    if (detailsForm.ShowDialog() == DialogResult.Abort)
                    {
                        break;
                    }
                }
            }

            update();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool LoadLocal = false;

            if (e.ColumnIndex == 1)
            {
                LoadLocal = false;
            }
            else if (e.ColumnIndex == 2)
            {
                LoadLocal = true;
            }
            else
            {
                return;
            }

            Game selected = (Game)dataGridView1.Rows[e.RowIndex].Tag;

            Conf_OnlineLookup detailsForm = new Conf_OnlineLookup(selected, LoadLocal);
            detailsForm.StartPosition = FormStartPosition.CenterScreen;

            if (detailsForm.ShowDialog() == DialogResult.OK)
            {
                update();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() == selected.Path)
                    {
                        dataGridView1.CurrentCell = row.Cells[3];
                        break;
                    }
                }
            }
            detailsForm.Dispose();
        }

        private void ddlPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            update();
        }
    }
}

