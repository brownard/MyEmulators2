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
    public partial class Wzd_Main : Form
    {
        internal List<WzdPanel> panels = null;
        int currentIndex = 0;

        public Wzd_Main()
        {
            InitializeComponent();
        }

        internal void updateButtons()
        {
            cancelButton.Visible = !(currentIndex == 0);
            nextButton.Text = currentIndex > panels.Count - 2 ? "Finish" : "Next";
        }

        void nextButton_Click(object sender, EventArgs e)
        {
            if (!panels[currentIndex].Next())
                return;

            if (currentIndex > panels.Count - 2) //finish
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            switchPanel(1);
        }

        void cancelButton_Click(object sender, EventArgs e)
        {
            if (currentIndex < 1 || !panels[currentIndex].Back())
                return;

            switchPanel(-1);
        }

        void switchPanel(int direction)
        {
            WzdPanel newPanel = panels[currentIndex + direction];
            panel1.Controls.Clear();
            panel1.Controls.Add(newPanel);
            newPanel.UpdatePanel();
            currentIndex += direction;

            updateButtons();
        }
    }
}
