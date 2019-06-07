using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class GameMap : ClassMap<Game>
    {
        public GameMap()
        {
            Id(x => x.id);
            HasMany(x => x.yearMatches)
                .Cascade.All();
            HasMany(x => x.filenameMatches)
                .Cascade.All();
            HasMany(x => x.genreMatches)
                .Cascade.All();
            HasMany(x => x.gradeMatches)
                .Cascade.All();
            HasMany(x => x.hashMatches)
                .Cascade.All();
            HasMany(x => x.manualMatches)
                .Cascade.All();
            HasMany(x => x.titleMatches)
                .Cascade.All();
            HasMany(x => x.ImageBackMatches)
                .Cascade.All();
            HasMany(x => x.ImageFrontMatches)
                .Cascade.All();
            HasMany(x => x.ImageTitleScreenMatches)
                .Cascade.All();
            HasMany(x => x.ImageIngameMatches)
                .Cascade.All();
            HasMany(x => x.ImageFanartMatches)
                .Cascade.All();
            HasMany(x => x.descriptionMatches)
                .Cascade.All();
        }
    }
}
