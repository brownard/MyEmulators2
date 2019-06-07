using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myEmulators
{
    internal partial class Conf_Emulators : myEmulators.ContentPanel
    {
        public Conf_Emulators()
        {
            InitializeComponent();

            foreach (Emulator item in DB.Instance.GetEmulatorsAndPC())
            {
                emuList.Items.Add(item);
            }
            
        }

        private void add_Click(object sender, EventArgs e)
        {
            Conf_Emu_Details detailsForm = new Conf_Emu_Details(null);
            if (detailsForm.ShowDialog() == DialogResult.OK)
            {
                emuList.Items.Add(detailsForm.getEmulator());
                updateButtonEnablings();
                OnChange(this, e);
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            Emulator oldEmu = (Emulator)emuList.SelectedItem;
            string oldTitle = oldEmu.Title;
            Emulator updatedEmu = null;

            if (oldEmu.isPc())
            {
                Conf_PC_Details detailsForm = new Conf_PC_Details(oldEmu);
                if (detailsForm.ShowDialog() == DialogResult.OK)
                    updatedEmu = detailsForm.getEmulator();
            }
            else
            {
                Conf_Emu_Details detailsForm = new Conf_Emu_Details(oldEmu);
                if (detailsForm.ShowDialog() == DialogResult.OK)
                    updatedEmu = detailsForm.getEmulator();
            }
            if (updatedEmu != null)
            {
                if (!ThumbsHandler.Instance.NeedThumbUpdate && oldTitle != updatedEmu.Title)
                    ThumbsHandler.Instance.NeedThumbUpdate = true;
                emuList.SelectedItem = updatedEmu;
                
                //Refresh
                int selectedIndex = emuList.SelectedIndex;
                List<Emulator> emus = new List<Emulator>();
                foreach (Emulator item in emuList.Items)
                {
                    emus.Add(item);
                }
                emuList.Items.Clear();
                foreach (Emulator item in emus)
                {
                    emuList.Items.Add(item);
                }
                emuList.SelectedIndex = selectedIndex;

                OnChange(this, e);
            }
        }

        private void emuList_DoubleClick(object sender, EventArgs e)
        {
            if (edit.Enabled && emuList.SelectedItem != null)
            {
                edit_Click(sender, e);
            }
        }

        List<Emulator> emusToDelete = new List<Emulator>();

        private void delete_Click(object sender, EventArgs e)
        {
            emusToDelete.Add((Emulator)emuList.SelectedItem);
            emuList.Items.Remove(emuList.SelectedItem);
            updateButtonEnablings();
            OnChange(this, e);
        }

        private void emuList_KeyDown(object sender, KeyEventArgs e)
        {
            if (delete.Enabled && e.KeyCode == Keys.Delete)
            {
                delete_Click(sender, EventArgs.Empty);
            }
        }

        private void updateButtonEnablings()
        {
            updateButtonEnablings(this, EventArgs.Empty);
        }
        private void updateButtonEnablings(object sender, EventArgs e)
        {
            if (emuList.SelectedIndex < 0)
            {
                moveup.Enabled = false;
                movedown.Enabled = false;
                edit.Enabled = false;
                delete.Enabled = false;
            }
            else
            {
                edit.Enabled = true;
                delete.Enabled = true;
                if (emuList.SelectedIndex > 0)
                {
                    moveup.Enabled = true;
                }
                else
                {
                    moveup.Enabled = false;
                }
                if (emuList.SelectedIndex < emuList.Items.Count - 1)
                {
                    movedown.Enabled = true;
                }
                else
                {
                    movedown.Enabled = false;
                }
                Emulator emu = emuList.SelectedItem as Emulator;
                if (emu != null)
                {
                    if (emu.isPc())
                        delete.Enabled = false;
                }
            }
        }

        private void moveup_Click(object sender, EventArgs e)
        {
            moveListItem(true, e);
        }

        private void movedown_Click(object sender, EventArgs e)
        {
            moveListItem(false, e);
        }

        private void moveListItem(bool isDirectionUp, EventArgs e)
        {
            int index = emuList.SelectedIndex;
            Object item = emuList.SelectedItem;
            emuList.Items.Remove(item);
            emuList.Items.Insert(isDirectionUp ? index - 1 : index + 1, item);
            emuList.SelectedIndex = isDirectionUp ? index - 1 : index + 1;
            updateButtonEnablings();
            OnChange(this, e);
        }

        public override void save()
        {
            foreach (Emulator item in emusToDelete)
            {
                item.Delete();
            }
            emusToDelete = new List<Emulator>();
            for (int i = 0; i < emuList.Items.Count; i++)
            {
                Emulator item = (Emulator)emuList.Items[i];
                item.Position = i;
                item.Save();
            }
            base.save();
        }

        public override void update()
        {
            //remove pc emulator if all pc games deleted
            if (form == null)
                return;
            if (!form.Text.EndsWith("*")) //only update if all changes have been saved
            {
                emuList.Items.Clear();
                foreach (Emulator item in DB.Instance.GetEmulatorsAndPC())
                    emuList.Items.Add(item);
            }
            base.update();
        }
    }
}

