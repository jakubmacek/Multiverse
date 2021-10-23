using NHibernate;
using NHibernate.Json;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using System.Collections.Generic;

namespace Multiverse.Persistence.NHibernate
{
    class UnitMap : ClassMapping<IUnit>
    {
        public UnitMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });

            Discriminator(d =>
            {
                d.Force(true);
                d.Insert(true);
                d.Length(200);
                d.NotNullable(true);
                d.Type((IType)NHibernateUtil.String);
                d.Column("type");
            });
            Persister<UnitEntityPersister>();

            ManyToOne(x => x.Player, m => { m.NotNullable(true); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(200); });
            Component(x => x.Place, y =>
            {
                y.ManyToOne(y => y.World, p => { p.NotNullable(true); });
                y.Property(y => y.X, p => { p.NotNullable(true); });
                y.Property(y => y.Y, p => { p.NotNullable(true); });
            });
            Component(x => x.PlayerData, y =>
            {
                y.Property(y => y.Value, p => { p.NotNullable(false); p.Length(1000); });
            });
            ManyToOne(x => x.Script, m => { m.NotNullable(false); });

            Property(x => x.Name, p => { p.NotNullable(true); p.Length(200); });
            Property(x => x.Health, p => { p.NotNullable(true); });
            Property(x => x.Movement, p => { p.NotNullable(true); });
            Property(x => x.Abilities, p => { p.NotNullable(true); p.Length(40000); p.Type<JsonColumnType<List<UnitAbility>>>(); });
            Property(x => x.Resources, p => { p.NotNullable(true); p.Length(4000); p.Type<JsonColumnType<List<UnitAbility>>>(); });
        }
    }

    class UnitSubclassMap<T> : SubclassMapping<T> where T : class
    {
        public UnitSubclassMap()
        {
            DiscriminatorValue(nameof(T));
        }
    }
}
