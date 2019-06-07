using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class GradeMap : ClassMap<Grade>
    {
        public GradeMap()
        {
            Id(x => x.id);
            Map(x => x.grade);
            HasMany(x => x.gradeMatches)
                .Cascade.All();
        }
    }
}
