using NHibernate;
using System;
using System.Linq;

namespace Multiverse.Persistence.NHibernate
{
    public class Repository : IRepository, IDisposable
    {
        private bool hasBeenDisposed;

        private readonly ISession session;

        private readonly World world;

        public World World => world;

        public IQueryable<Player> Players => session.Query<Player>();

        public IQueryable<Script> Scripts => session.Query<Script>();

        public IQueryable<Unit> Units => session.Query<Unit>().Where(x => x.World == world);

        public IQueryable<UnitGroup> UnitGroups => session.Query<UnitGroup>().Where(x => x.World == world);

        public IQueryable<Message> Messages => session.Query<Message>().Where(x => x.World == world);

        internal Repository(ISession session, World world)
        {
            this.session = session;
            this.world = world;
        }

        public Player? GetPlayer(int id)
        {
            return session.Get<Player>(id);
        }

        public Script? GetScript(Guid id)
        {
            return session.Get<Script>(id);
        }

        public Unit? GetUnit(Guid id)
        {
            return session.Get<Unit>(id);
        }

        public UnitGroup? GetUnitGroup(Guid id)
        {
            return session.Get<UnitGroup>(id);
        }

        public void Save(Unit unit)
        {
            session.Save(unit);
            session.Flush();
        }

        public void Save(UnitGroup unitGroup)
        {
            session.Save(unitGroup);
            session.Flush();
        }

        public void Save(Player player)
        {
            session.Save(player);
            session.Flush();
        }

        public void Save(Script script)
        {
            session.Save(script);
            session.Flush();
        }

        public void Save(Message message)
        {
            session.Save(message);
            session.Flush();
        }

        public void SaveWorld()
        {
            session.Save(world);
            session.Flush();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                if (disposing)
                {
                    session.Dispose();
                }

                hasBeenDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
