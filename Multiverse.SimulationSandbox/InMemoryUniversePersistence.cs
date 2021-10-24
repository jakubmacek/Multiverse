using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimulationSandbox
{
    //public class InMemoryUniversePersistence : IRepository
    //{
    //    private World world;
    //    private List<Player> players = new List<Player>();
    //    private List<Script> scripts = new List<Script>();
    //    private List<Unit> units = new List<Unit>();
    //    private List<UnitGroup> unitGroups = new List<UnitGroup>();

    //    public World World => world;

    //    public IQueryable<Player> Players => players.AsQueryable();

    //    public IQueryable<Script> Scripts => scripts.AsQueryable();

    //    public IQueryable<Unit> Units => units.AsQueryable();

    //    public IQueryable<UnitGroup> UnitGroups => unitGroups.AsQueryable();

    //    public InMemoryUniversePersistence(World world)
    //    {
    //        this.world = world;
    //    }

    //    public Player GetPlayer(Guid id)
    //    {
    //        return players.First(x => x.Id == id);
    //    }

    //    public Script GetScript(Guid id)
    //    {
    //        return scripts.First(x => x.Id == id);
    //    }

    //    public Unit GetUnit(Guid id)
    //    {
    //        return units.First(x => x.Id == id);
    //    }

    //    public UnitGroup GetUnitGroup(Guid id)
    //    {
    //        return unitGroups.First(x => x.Id == id);
    //    }

    //    public void SaveWorld()
    //    {
    //        // Nic, netreba ukladat, je pouze v pameti.
    //    }

    //    public void Save(Player player)
    //    {
    //        players.Add(player);
    //    }

    //    public void Save(Script script)
    //    {
    //        scripts.Add(script);
    //    }

    //    public void Save(Unit unit)
    //    {
    //        if (!units.Contains(unit))
    //            units.Add(unit);
    //    }

    //    public void Save(UnitGroup unitGroup)
    //    {
    //        unitGroups.Add(unitGroup);
    //    }
    //}
}
