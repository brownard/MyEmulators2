using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class TitleMatchMap : ClassMap<TitleMatch>
    {
        public TitleMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.title);
            Map(x => x.count);
        }
    }
}
