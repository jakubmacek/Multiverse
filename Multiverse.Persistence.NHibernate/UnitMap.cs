using NHibernate;
using NHibernate.Json;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using System.Collections.Generic;

namespace Multiverse.Persistence.NHibernate
{
    class UnitMap : ClassMapping<Unit>
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

            ManyToOne(x => x.Player, m => { m.NotNullable(true); m.Column("player"); });
            Property(x => x.Name, p => { p.NotNullable(true); p.Length(200); });
            ManyToOne(x => x.World, p => { p.NotNullable(true); p.Column("world"); });
            Component(x => x.Place, y =>
            {
                y.Property(y => y.X, p => { p.NotNullable(true); p.Column("x"); });
                y.Property(y => y.Y, p => { p.NotNullable(true); p.Column("y"); });
            });
            Component(x => x.PlayerData, y =>
            {
                y.Property(y => y.Value, p => { p.NotNullable(false); p.Length(1000); });
            });
            ManyToOne(x => x.Script, m => { m.NotNullable(false); m.Column("script"); });

            Property(x => x.Name, p => { p.NotNullable(true); p.Length(200); });
            Property(x => x.Health, p => { p.NotNullable(true); });
            Property(x => x.MovementPoints, p => { p.NotNullable(true); });
            Property(x => x.Abilities, p => { p.NotNullable(true); p.Length(40000); p.Type<JsonColumnType<List<IUnitAbility>>>(); });
            Property(x => x.Resources, p => { p.NotNullable(true); p.Length(4000); p.Type<JsonColumnType<Dictionary<int, int>>>(); });
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
