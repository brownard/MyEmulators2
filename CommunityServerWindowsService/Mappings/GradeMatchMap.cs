using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class GradeMatchMap : ClassMap<GradeMatch>
    {
        public GradeMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.grade);
            Map(x => x.count);
        }
    }
}
