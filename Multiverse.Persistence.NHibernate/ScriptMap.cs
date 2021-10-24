using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    class ScriptMap : ClassMapping<Script>
    {
        public ScriptMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            ManyToOne(x => x.Player, m => { m.NotNullable(true); m.Column("player"); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(200); });
            Property(x => x.Type, p => { p.NotNullable(true); });
            Property(x => x.Source, p => { p.NotNullable(true); p.Length(1000000); });
        }
    }
}
