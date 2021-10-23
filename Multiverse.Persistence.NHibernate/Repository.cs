using NHibernate;
using System;
using System.Linq;

namespace Multiverse.Persistence.NHibernate
{
    public class Repository : UniversePersistence, IDisposable
    {
        private bool hasBeenDisposed;

        private readonly ISession session;

        private readonly IWorld world;

        public IWorld World => world;

        public IQueryable<IPlayer> Players => session.Query<IPlayer>();

        public IQueryable<IScript> Scripts => session.Query<IScript>();

        public IQueryable<IUnit> Units => session.Query<IUnit>().Where(x => x.Place.World == world);

        public IQueryable<IUnitGroup> UnitGroups => session.Query<IUnitGroup>().Where(x => x.World == world);

        public Repository(ISession session, IWorld world)
        {
            this.session = session;
            this.world = world;
        }

        public Place GetPlace(int x, int y)
        {
            return new Place(world, x, y);
        }

        public IPlayer GetPlayer(Guid id)
        {
            return session.Get<IPlayer>(id);
        }

        public IScript GetScript(Guid id)
        {
            return session.Get<IScript>(id);
        }

        public IUnit GetUnit(Guid id)
        {
            return session.Get<IUnit>(id);
        }

        public IUnitGroup GetUnitGroup(Guid id)
        {
            return session.Get<IUnitGroup>(id);
        }

        public void Save(IUnit unit)
        {
            session.Save(unit);
            session.Flush();
        }

        public void Save(IUnitGroup unitGroup)
        {
            session.Save(unitGroup);
            session.Flush();
        }

        public void Save(IPlayer player)
        {
            session.Save(player);
            session.Flush();
        }

        public void Save(IScript script)
        {
            session.Save(script);
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
