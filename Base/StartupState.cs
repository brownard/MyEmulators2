using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEmulators2
{
    public enum StartupState
    {
        EMULATORS = 0,
        GROUPS = 1,
        FAVOURITES = 2,
        PCGAMES = 3
    }

    class StartupStateHandler
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
