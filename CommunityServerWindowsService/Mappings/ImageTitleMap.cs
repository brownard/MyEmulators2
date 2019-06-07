using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageTitleScreenMap : ClassMap<ImageTitleScreen>
    {
        public ImageTitleScreenMap()
        {
            Id(x => x.id);
            Map(x => x.url);
            HasMany(x => x.ImageTitleScreenMatches)
                .Cascade.All();
        }
    }
}
