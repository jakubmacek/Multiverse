using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimulationSandbox
{
    public class InMemoryUniversePersistence : UniversePersistence
    {
        private IWorld world;
        private List<IPlayer> players = new List<IPlayer>();
        private List<IScript> scripts = new List<IScript>();
        private List<IUnit> units = new List<IUnit>();
        private List<IUnitGroup> unitGroups = new List<IUnitGroup>();

        public IWorld World => world;

        public IQueryable<IPlayer> Players => players.AsQueryable();

        public IQueryable<IScript> Scripts => scripts.AsQueryable();

        public IQueryable<IUnit> Units => units.AsQueryable();

        public IQueryable<IUnitGroup> UnitGroups => unitGroups.AsQueryable();

        public InMemoryUniversePersistence(IWorld world)
        {
            this.world = world;
        }

        public Place GetPlace(int x, int y)
        {
            return new Place(World, x, y);
        }

        public IPlayer GetPlayer(Guid id)
        {
            return players.First(x => x.Id == id);
        }

        public IScript GetScript(Guid id)
        {
            return scripts.First(x => x.Id == id);
        }

        public IUnit GetUnit(Guid id)
        {
            return units.First(x => x.Id == id);
        }

        public IUnitGroup GetUnitGroup(Guid id)
        {
            return unitGroups.First(x => x.Id == id);
        }

        public void SaveWorld()
        {
            // Nic, netreba ukladat, je pouze v pameti.
        }

        public void Save(IPlayer player)
        {
            players.Add(player);
        }

        public void Save(IScript script)
        {
            scripts.Add(script);
        }

        public void Save(IUnit unit)
        {
            if (!units.Contains(unit))
                units.Add(unit);
        }

        public void Save(IUnitGroup unitGroup)
        {
            unitGroups.Add(unitGroup);
        }
    }
}
