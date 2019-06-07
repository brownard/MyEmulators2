using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class ImageFanart
    {
        public virtual int id { get; private set; }
        public virtual string url { get; set; }
        public virtual IList<ImageFanartMatch> ImageFanartMatches { get; private set; }

        public ImageFanart()
        {
            ImageFanartMatches = new List<ImageFanartMatch>();
        }
    }
}
