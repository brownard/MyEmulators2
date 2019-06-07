using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageFrontMap : ClassMap<ImageFront>
    {
        public ImageFrontMap()
        {
            Id(x => x.id);
            Map(x => x.url);
            HasMany(x => x.ImageFrontMatches)
                .Cascade.All();
        }
    }
}
