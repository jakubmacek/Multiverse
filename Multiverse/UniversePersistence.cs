using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    /// <summary>
    /// Is always bound to a single World instance. Cross-world interaction is not supported.
    /// </summary>
    public interface UniversePersistence
    {
        public abstract IWorld World { get; }

        public abstract IQueryable<IPlayer> Players { get; }

        public abstract IPlayer GetPlayer(Guid id);

        public abstract IQueryable<IScript> Scripts { get; }

        public abstract IScript GetScript(Guid id);

        //public abstract IQueryable<Place> Places { get; }

        public abstract Place GetPlace(int x, int y);

        public abstract IQueryable<IUnit> Units { get; }

        public abstract IUnit GetUnit(Guid id);

        public abstract IQueryable<IUnitGroup> UnitGroups { get; }

        public abstract IUnitGroup GetUnitGroup(Guid id);

        public abstract void SaveWorld();

        public abstract void Save(IUnit unit);

        public abstract void Save(IUnitGroup unitGroup);

        public abstract void Save(IPlayer player);

        public abstract void Save(IScript script);
    }
}
