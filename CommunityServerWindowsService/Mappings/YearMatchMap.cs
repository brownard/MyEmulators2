using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class YearMatchMap : ClassMap<YearMatch>
    {
        public YearMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.year);
            Map(x => x.count);
        }
    }
}
