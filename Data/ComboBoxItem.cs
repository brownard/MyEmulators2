using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public class ComboBoxItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DBItem Value { get; set; }

        public ComboBoxItem(DBItem item)
        {
            Emulator emu = item as Emulator;
            if (emu != null)
            {
                ID = emu.UID;
                Name = emu.Title;
            }
            else
            {
                Game game = (Game)item;
                ID = game.GameID;
                Name = game.Title;
            }

            Value = item;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
