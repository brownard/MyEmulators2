using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Year
    {
        public virtual int id { get; private set; }
        public virtual string year { get; set; }
        public virtual IList<YearMatch> yearMatches { get; private set; }

        public Year()
        {
            yearMatches = new List<YearMatch>();
        }
    }
}
