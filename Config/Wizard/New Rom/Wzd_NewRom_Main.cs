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
    public partial class Wzd_NewRom_Main : Wzd_Main
    {
        Game newGame = null;
        public Game NewGame
        {
            get { return newGame; }
        }

        public Wzd_NewRom_Main()
        {
            InitializeComponent();
            this.Text = "New Game";
            newGame = new Game();
            panels = new List<WzdPanel>(new WzdPanel[] { new Wzd_NewRom_Start(newGame), new Wzd_NewRom_Info(newGame) });

            panel1.Controls.Add(panels[0]);
            updateButtons();
        }
    }
}
