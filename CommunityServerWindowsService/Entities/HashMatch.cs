using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class HashMatch
    {
        public virtual int id { get; private set; }
        public virtual Game game { get; set; }
        public virtual Hash hash { get; set; }
        public virtual int count { get; set; }
    }
}
