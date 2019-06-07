using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class ImageBack
    {
        public virtual int id { get; private set; }
        public virtual string url { get; set; }
        public virtual IList<ImageBackMatch> ImageBackMatches { get; private set; }

        public ImageBack()
        {
            ImageBackMatches = new List<ImageBackMatch>();
        }
    }
}
