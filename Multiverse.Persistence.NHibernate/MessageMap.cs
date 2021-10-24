using NHibernate.Mapping.ByCode.Conformist;

namespace Multiverse.Persistence.NHibernate
{
    class MessageMap : ClassMapping<Message>
    {
        public MessageMap()
        {
            Id(x => x.Id, i => { i.Column("id"); });
            ManyToOne(x => x.World, m => { m.NotNullable(true); m.Column("world"); });
            ManyToOne(x => x.Player, m => { m.NotNullable(true); m.Column("player"); });
            Property(x => x.Type, p => { p.NotNullable(true); });
            Property(x => x.SentAt, p => { p.NotNullable(true); });
            Property(x => x.SentAtTimestamp, p => { p.NotNullable(true); });
            Property(x => x.ReceivedAtTimestamp, p => { p.NotNullable(false); });
            Property(x => x.FromUnit, p => { p.NotNullable(false); });
            Property(x => x.ToUnit, p => { p.NotNullable(false); });
            Property(x => x.Text, p => { p.NotNullable(true); p.Length(1000); });
            Component(x => x.PlayerData, y =>
            {
                y.Property(y => y.Value, p => { p.NotNullable(false); p.Length(1000); });
            });
        }
    }
}
