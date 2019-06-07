using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ImageIngameMatchMap : ClassMap<ImageIngameMatch>
    {
        public ImageIngameMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.ImageIngame);
            Map(x => x.count);
        }
    }
}
