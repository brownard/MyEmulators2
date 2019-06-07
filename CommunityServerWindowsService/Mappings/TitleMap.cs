using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class TitleMap : ClassMap<Title>
    {
        public TitleMap()
        {
            Id(x => x.id);
            Map(x => x.title);
            HasMany(x => x.titleMatches)
                .Cascade.All();
        }
    }
}
