using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class ImageFront
    {
        public virtual int id { get; private set; }
        public virtual string url { get; set; }
        public virtual IList<ImageFrontMatch> ImageFrontMatches { get; private set; }

        public ImageFront()
        {
            ImageFrontMatches = new List<ImageFrontMatch>();
        }
    }
}
