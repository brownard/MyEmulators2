using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class GenreMatchMap : ClassMap<GenreMatch>
    {
        public GenreMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.genre);
            Map(x => x.count);
        }
    }
}
