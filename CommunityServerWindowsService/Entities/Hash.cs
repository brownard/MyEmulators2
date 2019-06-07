using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Hash
    {
        public virtual int id { get; private set; }
        public virtual string hash { get; set; }
        public virtual IList<HashMatch> hashMatches { get; private set; }

        public Hash()
        {
            hashMatches = new List<HashMatch>();
        }
    }
}
