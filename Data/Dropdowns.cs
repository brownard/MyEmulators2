using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MyEmulators2
{
    class Dropdowns
    {
        public static List<ComboBoxItem> GetEmuComboBoxItems()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            items.Add(new ComboBoxItem(new Emulator(EmulatorType.manyemulators) { Title = "All Systems" }));
            foreach (Emulator emu in DB.Instance.GetEmulators())
                items.Add(new ComboBoxItem(emu));

            return items;
        }

        public static List<ComboBoxItem> GetNewRomComboBoxItems()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            foreach (Emulator emu in DB.Instance.GetEmulators(true))
                items.Add(new ComboBoxItem(emu));

            return items;
        }

        public static DataTable GetSystems()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("Value");
            tb.Columns.Add("Text");

            DataRow dr = tb.NewRow();
            dr[0] = "0";
            dr[1] = "Unspecified";
            tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "35"; dr[1] = "3DO"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "19"; dr[1] = "Amiga"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "56"; dr[1] = "Amiga CD32"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "60"; dr[1] = "Amstrad CPC"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "31"; dr[1] = "Apple II"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "51"; dr[1] = "Apple IIgs"; tb.Rows.Add(dr);
            
            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Arcade"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "28"; dr[1] = "Atari 2600"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "33"; dr[1] = "Atari 5200"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "34"; dr[1] = "Atari 7800"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "39"; dr[1] = "Atari 8-bit"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Atari Jaguar"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Atari Jaguar CD"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "24"; dr[1] = "Atari ST"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Atari XE"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "63"; dr[1] = "BREW"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "84"; dr[1] = "Browser"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "73"; dr[1] = "CD-i"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "76"; dr[1] = "Channel F"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "29"; dr[1] = "ColecoVision"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "61"; dr[1] = "Commodore 128"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "27"; dr[1] = "Commodore 64"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "77"; dr[1] = "Commodore PET/CBM"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "72"; dr[1] = "DoJa"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "2"; dr[1] = "DOS"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "79"; dr[1] = "Dragon 32/64"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "8"; dr[1] = "Dreamcast"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "70"; dr[1] = "ExEn"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "50"; dr[1] = "Game.Com"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "14"; dr[1] = "GameCube"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "10"; dr[1] = "Game Boy"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "12"; dr[1] = "Game Boy Advance"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "11"; dr[1] = "Game Boy Color"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "25"; dr[1] = "Game Gear"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "16"; dr[1] = "Genesis"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "55"; dr[1] = "Gizmondo"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "30"; dr[1] = "Intellivision"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "86"; dr[1] = "iPhone"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "80"; dr[1] = "iPod Classic"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "64"; dr[1] = "J2ME"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "17"; dr[1] = "Jaguar"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "1"; dr[1] = "Linux"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "18"; dr[1] = "Lynx"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "74"; dr[1] = "Macintosh"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "71"; dr[1] = "Mophun"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "57"; dr[1] = "MSX"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "32"; dr[1] = "N-Gage"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "36"; dr[1] = "Neo Geo"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "54"; dr[1] = "Neo Geo CD"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "52"; dr[1] = "Neo Geo Pocket"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "53"; dr[1] = "Neo Geo Pocket Color"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "22"; dr[1] = "NES"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "9"; dr[1] = "Nintendo 64"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Nintendo 3DS"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "44"; dr[1] = "Nintendo DS"; tb.Rows.Add(dr);


            dr = tb.NewRow(); dr[0] = "75"; dr[1] = "Odyssey"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "78"; dr[1] = "Odyssey 2"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "65"; dr[1] = "Palm OS"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "59"; dr[1] = "PC-FX"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "4"; dr[1] = "PC Booter"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "6"; dr[1] = "Playstation"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "7"; dr[1] = "Playstation 2"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "81"; dr[1] = "Playstation 3"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Playstation Vita"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "46"; dr[1] = "PSP"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "21"; dr[1] = "SEGA 32X"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "20"; dr[1] = "SEGA CD"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "26"; dr[1] = "SEGA Master System"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "SEGA Mega Drive"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "23"; dr[1] = "SEGA Saturn"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "15"; dr[1] = "SNES"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "85"; dr[1] = "Spectravideo"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "67"; dr[1] = "Symbian"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "47"; dr[1] = "TI-99/4A"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "58"; dr[1] = "TRS-80"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "62"; dr[1] = "TRS-80 CoCo"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "40"; dr[1] = "TurboGrafx-16"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "45"; dr[1] = "TurboGrafx CD"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "42"; dr[1] = "V.Smile"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "37"; dr[1] = "Vectrex"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "43"; dr[1] = "VIC-20"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "38"; dr[1] = "Virtual Boy"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "82"; dr[1] = "Wii"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "-1"; dr[1] = "Wii U"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "3"; dr[1] = "Windows"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "5"; dr[1] = "Windows 3.x"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "66"; dr[1] = "Windows Mobile"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "48"; dr[1] = "WonderSwan"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "49"; dr[1] = "WonderSwan Color"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "13"; dr[1] = "Xbox"; tb.Rows.Add(dr);

            dr = tb.NewRow(); dr[0] = "69"; dr[1] = "Xbox 360"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "68"; dr[1] = "Zodiac"; tb.Rows.Add(dr);
            dr = tb.NewRow(); dr[0] = "41"; dr[1] = "ZX Spectrum"; tb.Rows.Add(dr);

            return tb;
        }
    }
}
