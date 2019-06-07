using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace CommunityServerWindowsService
{
    public class FilenameMatchMap : ClassMap<FilenameMatch>
    {
        public FilenameMatchMap()
        {
            Id(x => x.id);
            References(x => x.game);
            References(x => x.filename);
            Map(x => x.count);
        }
    }
}
