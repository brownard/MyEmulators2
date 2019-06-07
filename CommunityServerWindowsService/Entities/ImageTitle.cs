using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class ImageTitleScreen
    {
        public virtual int id { get; private set; }
        public virtual string url { get; set; }
        public virtual IList<ImageTitleScreenMatch> ImageTitleScreenMatches { get; private set; }

        public ImageTitleScreen()
        {
            ImageTitleScreenMatches = new List<ImageTitleScreenMatch>();
        }
    }
}
