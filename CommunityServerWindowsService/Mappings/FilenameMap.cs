using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class FilenameMap : ClassMap<Filename>
    {
        public FilenameMap()
        {
            Id(x => x.id);
            Map(x => x.filename);
            HasMany(x => x.filenameMatches)
                .Cascade.All();
        }

    }
}
