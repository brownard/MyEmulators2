using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class DescriptionMatchMap : ClassMap<DescriptionMatch>
    {
        public DescriptionMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.description);
            Map(x => x.count);
        }
    }
}
