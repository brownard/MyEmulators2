using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class DescriptionMap : ClassMap<Description>
    {
        public DescriptionMap()
        {
            Id(x => x.id);
            Map(x => x.description);
            HasMany(x => x.descriptionMatches)
                .Cascade.All();
        }
    }
}
