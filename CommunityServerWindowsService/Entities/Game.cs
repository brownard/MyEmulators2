using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Game
    {
        public virtual int id { get; private set; }
        public virtual IList<YearMatch> yearMatches { get; set; }
        public virtual IList<FilenameMatch> filenameMatches { get; set; }
        public virtual IList<GenreMatch> genreMatches { get; set; }
        public virtual IList<HashMatch> hashMatches { get; set; }
        public virtual IList<ImageBackMatch> ImageBackMatches { get; set; }
        public virtual IList<ImageFrontMatch> ImageFrontMatches { get; set; }
        public virtual IList<ImageIngameMatch> ImageIngameMatches { get; set; }
        public virtual IList<ImageTitleScreenMatch> ImageTitleScreenMatches { get; set; }
        public virtual IList<ImageFanartMatch> ImageFanartMatches { get; set; }
        public virtual IList<ManualMatch> manualMatches { get; set; }
        public virtual IList<GradeMatch> gradeMatches { get; set; }
        public virtual IList<TitleMatch> titleMatches { get; set; }
        public virtual IList<DescriptionMatch> descriptionMatches { get; set; }

        public Game()
        {
            yearMatches = new List<YearMatch>();
            filenameMatches = new List<FilenameMatch>();
            genreMatches = new List<GenreMatch>();
            hashMatches = new List<HashMatch>();
            ImageBackMatches = new List<ImageBackMatch>();
            ImageFanartMatches = new List<ImageFanartMatch>();
            ImageFrontMatches = new List<ImageFrontMatch>();
            ImageIngameMatches = new List<ImageIngameMatch>();
            ImageTitleScreenMatches = new List<ImageTitleScreenMatch>();
            manualMatches = new List<ManualMatch>();
            gradeMatches = new List<GradeMatch>();
            titleMatches = new List<TitleMatch>();
            descriptionMatches = new List<DescriptionMatch>();
        }
    }
}
