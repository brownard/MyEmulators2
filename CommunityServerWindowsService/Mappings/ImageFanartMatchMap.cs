using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageFanartMatchMap : ClassMap<ImageFanartMatch>
    {
        public ImageFanartMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.ImageFanart);
            Map(x => x.count);
        }
    }
}
