using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class HashMatchMap : ClassMap<HashMatch>
    {
        public HashMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.hash);
            Map(x => x.count);
        }
    }
}
