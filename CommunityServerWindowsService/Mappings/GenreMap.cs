using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class GenreMap : ClassMap<Genre>
    {
        public GenreMap()
        {
            Id(x => x.id);
            Map(x => x.genre);
            HasMany(x => x.genreMatches)
                .Cascade.All();
        }
    }
}
