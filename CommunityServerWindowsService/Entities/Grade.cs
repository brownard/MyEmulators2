using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunityServerWindowsService
{
    public class Grade
    {
        public virtual int id { get; private set; }
        public virtual string grade { get; set; }
        public virtual IList<GradeMatch> gradeMatches { get; private set; }

        public Grade()
        {
            gradeMatches = new List<GradeMatch>();
        }
    }
}
