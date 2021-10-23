using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    class PlayerMap : ClassMapping<IPlayer>
    {
        public PlayerMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(100); });
        }
    }
}
