using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class ImageIngame
    {
        public virtual int id { get; private set; }
        public virtual string url { get; set; }
        public virtual IList<ImageIngameMatch> ImageIngameMatches { get; private set; }

        public ImageIngame()
        {
            ImageIngameMatches = new List<ImageIngameMatch>();
        }
    }
}
