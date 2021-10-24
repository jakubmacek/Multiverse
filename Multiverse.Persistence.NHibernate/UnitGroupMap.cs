using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    class UnitGroupMap : ClassMapping<UnitGroup>
    {
        public UnitGroupMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            ManyToOne(x => x.World, m => { m.NotNullable(true); m.Column("world"); });
            ManyToOne(x => x.Player, m => { m.NotNullable(true); m.Column("player"); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(200); });
            Component(x => x.PlayerData, y =>
            {
                y.Property(y => y.Value, p => { p.NotNullable(false); p.Length(1000); });
            });
            Set(x => x.Units, m =>
            {
                m.Table("unitgroupunits");
                m.Key(k => k.Column(col => col.Name("unitgroup")));
                //m.Cascade(Cascade.All | Cascade.DeleteOrphans); //optional
            }, a => a.ManyToMany());
        }
    }
}
