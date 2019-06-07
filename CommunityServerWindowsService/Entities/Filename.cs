using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Filename
    {
        public virtual int id { get; private set; }
        public virtual string filename { get; set; }
        public virtual IList<FilenameMatch> filenameMatches { get; private set; }

        public Filename()
        {
            filenameMatches = new List<FilenameMatch>();
        }
    }
}
