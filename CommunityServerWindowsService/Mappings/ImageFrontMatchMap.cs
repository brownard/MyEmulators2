using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageFrontMatchMap : ClassMap<ImageFrontMatch>
    {
        public ImageFrontMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.ImageFront);
            Map(x => x.count);
        }
    }
}
