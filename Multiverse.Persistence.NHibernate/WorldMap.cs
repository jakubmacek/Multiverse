using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    class WorldMap : ClassMapping<IWorld>
    {
        public WorldMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            Property(x => x.Universe, p => { p.NotNullable(true); p.Length(200); });
            Property(x => x.Timestamp, p => { p.NotNullable(true); });
        }
    }
}
