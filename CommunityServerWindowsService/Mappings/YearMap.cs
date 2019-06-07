using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class YearMap : ClassMap<Year>
    {
        public YearMap()
        {
            Id(x => x.id);
            Map(x => x.year);
            HasMany(x => x.yearMatches)
                .Cascade.All();
        }
    }
}
