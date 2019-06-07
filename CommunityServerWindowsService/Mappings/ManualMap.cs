using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ManualMap : ClassMap<Manual>
    {
        public ManualMap()
        {
            Id(x => x.id);
            Map(x => x.manual);
            HasMany(x => x.manualMatches)
                .Cascade.All();
        }
    }
}
