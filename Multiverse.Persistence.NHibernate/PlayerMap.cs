using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    public class PlayerMap : ClassMapping<Player>
    {
        public PlayerMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(100); });
        }
    }
}
