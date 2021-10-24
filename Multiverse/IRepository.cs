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
    public interface IRepository : IDisposable
    {
        public abstract World World { get; }

        public abstract IQueryable<Player> Players { get; }

        public abstract Player? GetPlayer(int id);

        public abstract IQueryable<Script> Scripts { get; }

        public abstract Script? GetScript(Guid id);

        public abstract IQueryable<Unit> Units { get; }

        public abstract Unit? GetUnit(Guid id);

        public abstract IQueryable<UnitGroup> UnitGroups { get; }

        public abstract IQueryable<Message> Messages { get; }

        public abstract UnitGroup? GetUnitGroup(Guid id);

        public abstract void SaveWorld();

        public abstract void Save(Unit unit);

        public abstract void Save(UnitGroup unitGroup);

        public abstract void Save(Player player);

        public abstract void Save(Script script);

        public abstract void Save(Message message);

        public abstract void Delete(Unit unit);
    }
}
