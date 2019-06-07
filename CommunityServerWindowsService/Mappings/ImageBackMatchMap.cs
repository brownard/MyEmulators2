using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageBackMatchMap : ClassMap<ImageBackMatch>
    {
        public ImageBackMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.ImageBack);
            Map(x => x.count);
        }
    }
}
