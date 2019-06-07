using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Title
    {
        public virtual int id { get; private set; }
        public virtual string title { get; set; }
        public virtual IList<TitleMatch> titleMatches { get; private set; }

        public Title()
        {
            titleMatches = new List<TitleMatch>();
        }
    }
}
