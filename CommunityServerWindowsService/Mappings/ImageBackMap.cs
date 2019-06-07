using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageBackMap : ClassMap<ImageBack>
    {
        public ImageBackMap()
        {
            Id(x => x.id);
            Map(x => x.url);
            HasMany(x => x.ImageBackMatches)
                .Cascade.All();
        }
    }
}
