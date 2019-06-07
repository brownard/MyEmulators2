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
    internal partial class WzdPanel : ContentPanel
    {
        public WzdPanel()
        {
            InitializeComponent();
            //Dock = DockStyle.Fill;
        }

        public virtual bool Next() { return true; }
        public virtual bool Back() { return true; }
        //public virtual void UpdatePanel() { }

        Emulator emu = null;
        public Emulator Emulator
        {
            get 
            {
                if (emu == null)
                    emu = new Emulator();
                return emu;
            }
            set
            {
                emu = value;
            }
        }

        Game game = null;
        public Game Game
        {
            get
            {
                if (game == null)
                    game = new Game();
                return game;
            }
            set
            {
                game = value;
            }
        }
    }
}
