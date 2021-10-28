using Multiverse.Server.Authorization;
using NHibernate.Json;
using NHibernate.Mapping.ByCode.Conformist;
using System.Collections.Generic;

namespace Multiverse.Server.Persistence
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Id(x => x.Name, i => { i.Column("name"); i.Length(100); });
            Property(x => x.Players, p => { p.Column("players"); p.NotNullable(true); p.Length(4000); p.Type<JsonColumnType<List<UserPlayer>>>(); });
        }
    }
}
