using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Description
    {
        public virtual int id { get; private set; }
        public virtual string description { get; set; }
        public virtual IList<DescriptionMatch> descriptionMatches { get; private set; }

        public Description()
        {
            descriptionMatches = new List<DescriptionMatch>();
        }
    }
}
