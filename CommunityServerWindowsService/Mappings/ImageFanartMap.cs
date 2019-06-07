using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageFanartMap : ClassMap<ImageFanart>
    {
        public ImageFanartMap()
        {
            Id(x => x.id);
            Map(x => x.url);
            HasMany(x => x.ImageFanartMatches)
                .Cascade.All();
        }
    }
}
