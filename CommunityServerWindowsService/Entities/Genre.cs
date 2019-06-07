using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Genre
    {
        public virtual int id { get; private set; }
        public virtual string genre { get; set; }
        public virtual IList<GenreMatch> genreMatches { get; private set; }

        public Genre()
        {
            genreMatches = new List<GenreMatch>();
        }
    }
}
