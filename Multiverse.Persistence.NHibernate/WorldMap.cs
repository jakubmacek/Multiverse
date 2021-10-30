using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    public class WorldMap : ClassMapping<World>
    {
        public WorldMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            Property(x => x.Universe, p => { p.NotNullable(true); p.Length(200); });
            Property(x => x.Timestamp, p => { p.NotNullable(true); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(100); });
        }
    }
}
