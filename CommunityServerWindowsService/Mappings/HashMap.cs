using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class HashMap : ClassMap<Hash>
    {

        public HashMap()
        {
            Id(x => x.id);
            Map(x => x.hash);
            HasMany(x => x.hashMatches)
                .Cascade.All();
        }

    }
}
