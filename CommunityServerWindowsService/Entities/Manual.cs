using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Manual
    {
        public virtual int id { get; private set; }
        public virtual string manual { get; set; }
        public virtual IList<ManualMatch> manualMatches { get; private set; }

        public Manual()
        {
            manualMatches = new List<ManualMatch>();
        }
    }
}
