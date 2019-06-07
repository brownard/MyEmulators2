using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageIngameMap : ClassMap<ImageIngame>
    {

        public ImageIngameMap()
        {
            Id(x => x.id);
            Map(x => x.url);
            HasMany(x => x.ImageIngameMatches)
                .Cascade.All();
        }
    }
}
