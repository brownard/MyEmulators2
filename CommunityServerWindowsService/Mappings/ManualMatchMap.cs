using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class ManualMatchMap : ClassMap<ManualMatch>
    {
        public ManualMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.manual);
            Map(x => x.count);
        }
    }
}
