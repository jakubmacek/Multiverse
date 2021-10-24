using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class Universe : IUniverse
    {
        private bool hasBeenDisposed;

        public World World { get; }

        public IRepository Repository { get; }

        public IDictionary<int, Resource> Resources { get; }

        public IDictionary<string, UnitType> UnitTypes { get; }

        protected ScriptingEngineFactory ScriptingEngineFactory { get; }

        public Universe(IRepositoryFactoryFactory repositoryFactoryFactory, int worldId, IEnumerable<Resource> resources)
        {
            UnitTypes = new Dictionary<string, UnitType>();
            var unitVisitor = new RegisterUnitIntoDictionaryVisitor(this, UnitTypes);
            EnumerateUnits(unitVisitor);
            UnitTypes = new ReadOnlyDictionary<string, UnitType>(UnitTypes);

            Repository = repositoryFactoryFactory
                .GetRepositoryFactoryForUniverse(GetType().FullName ?? "!error!", EnumerateUnits)
                .Create(worldId);
            World = Repository.World;

            if (World.Universe != GetType().FullName)
                throw new ArgumentException($"This world is in '{World.Universe}' universe, expected {GetType().FullName}.");

            Resources = new ReadOnlyDictionary<int, Resource>(resources.ToDictionary(x => x.Id));
            ScriptingEngineFactory = new ScriptingEngineFactory();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                if (disposing)
                {
                    Repository.Dispose();
                }

                hasBeenDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected abstract void EnumerateUnits(IUnitTypeVisitor v);

        public virtual T SpawnUnit<T>(Player player, Place place) where T : Unit
        {
            var unit = CreateUnit<T>(player, place);
            InitializeUnit(unit, player, place);
            Repository.Save(unit);
            return (T)unit;
        }

        public abstract Unit CreateUnit<T>(Player player, Place place) where T : Unit;

        protected virtual void InitializeUnit(Unit unit, Player player, Place place)
        {
            var worldTimestamp = World.Timestamp;

            unit.Id = Guid.NewGuid();
            unit.Name = unit.GetType().Name + " " + unit.Id;
            unit.Player = player;
            unit.Place = place;
            unit.PlayerData = new PlayerData();
            unit.Health = unit.MaxHealth;
            unit.Movement = unit.MaxMovement;
            unit.Abilities.Clear();
            foreach (var ability in unit.CreateAbilities())
            {
                ability.CooldownTimestamp = worldTimestamp + ability.CooldownTime;
                unit.Abilities.Add(ability);
            }
        }

        public UnitAbilityUseResult UseAbility<T>(Unit unit, UnitAbilityUse use) where T : class, IUnitAbility
        {
            var ability = unit.Abilities.OfType<T>().FirstOrDefault();
            if (ability == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility);

            return UseAbility(unit, ability, use);
        }

        protected UnitAbilityUseResult UseAbility(Unit unit, IUnitAbility ability, UnitAbilityUse use)
        {
            if (ability.RemainingUses <= 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoRemainingUses);

            ability.RemainingUses--;
            var result = ability.Use(this, unit, use);
            Repository.Save(unit);
            return result;
        }

        public virtual MoveUnitResult MoveUnit(Unit unit, Place place, int movementRequired)
        {
            if (unit.Immovable)
                return new MoveUnitResult() { Type = MoveUnitResultType.Immovable };

            //TODO Implementovat pohyb. Kontrolu sousedstvi places, kontrolu dostatku pohybovych bodu. Odebirani pohybovych bodu.
            unit.Place = place;
            Repository.Save(unit);
            return new MoveUnitResult() { Type = MoveUnitResultType.Moved };
        }

        public virtual TransferResourceResult TransferResource(Unit from, Unit to, Resource resource, int amount)
        {
            if (amount <= 0)
                return new TransferResourceResult(TransferResourceResultType.NothingToTransfer, 0, amount);

            var howMuchCanBeRemoved = Math.Min(from.GetResourceAmount(resource).Amount, amount);
            if (howMuchCanBeRemoved <= 0)
                return new TransferResourceResult(TransferResourceResultType.NothingToTransfer, 0, amount);

            //var remainingCapacity = use.Unit.GetRemainingCapacity(wood);
            //var harvested = woodSource.RemoveResource(wood, Math.Min(remainingCapacity.Amount, HarvestedOnUse));
            //use.Unit.AddResource(wood, harvested.TransferredAmount);

            var added = to.AddResource(resource, howMuchCanBeRemoved);
            from.RemoveResource(resource, added.TransferredAmount);
            Repository.Save(from);
            Repository.Save(to);
            return added;
        }

        protected virtual void CooldownForAllAbilities(IEnumerable<Unit> units)
        {
            var worldTimestamp = World.Timestamp;

            foreach (var unit in units)
            {
                var unitChanged = false;

                if (unit.Movement < unit.MaxMovement)
                {
                    unit.Movement = unit.MaxMovement;
                    unitChanged = true;
                }

                foreach (var ability in unit.Abilities)
                {
                    if (ability.RemainingUses < ability.MaxAvailableUses)
                    {
                        if (ability.CooldownTimestamp >= worldTimestamp)
                        {
                            ability.RemainingUses = Math.Min(ability.MaxAvailableUses, ability.RemainingUses + ability.UsesRestoredOnCooldown);
                            ability.CooldownTimestamp = worldTimestamp + ability.CooldownTime;
                            unitChanged = true;
                        }
                    }
                }

                if (unitChanged)
                    Repository.Save(unit);
            }
        }

        protected virtual void RunEventScript(IEnumerable<Unit> units, Event @event)
        {
            foreach (var unit in units)
            {
                if (unit.Script == null)
                    continue;

                using (var scriptingEngine = ScriptingEngineFactory.Create(unit.Script))
                {
                    scriptingEngine.RunEvent(@event, unit);
                    //TODO Nekam uzivateli ukladat chybove hlasky.
                }

                Repository.Save(unit);
            }
        }

        public virtual void Tick()
        {
            CooldownForAllAbilities(Repository.Units);

            World.Timestamp++;
            Repository.SaveWorld();

            var tickEvent = new Event(this, World.Timestamp, EventType.Tick);

            RunEventScript(Repository.Units, tickEvent); //TODO fronta vsech jednotek, ktere muzou reagovat skriptem na tick; postupne vyrizovat v davkach; pridavat do fronty udalosti na ktere je potreba reagovat
        }

        public virtual void EnsureInitialWorldState()
        {
        }

        public ScanAroundResult ScanAroundUnit(Guid id)
        {
            var unit = Repository.GetUnit(id);
            if (unit == null)
                return new ScanAroundResult(new List<ScriptingUnit>());
            return ScanAround(unit);
        }

        public ScanAroundResult ScanAround(Unit self)
        {
            var scanCapability = self.ScanCapability;
            var placesInRange = scanCapability.GetRange(self.Place).ToList();
            if (placesInRange.Count == 0)
                return new ScanAroundResult(new List<ScriptingUnit>());

            var query = Repository.Units;
            //TODO omezit lepe vzdalenost dohledu - query = query.Where(x => x.Place.X ...)
            var units = query.ToList();
            units = units.Where(x => placesInRange.Any(y => y.Equals(x.Place))).ToList();

            var scannedUnits = new List<ScriptingUnit>();
            foreach (var unit in units)
            {
                var scannedUnit = scanCapability.Scan(self, unit);
                if (scannedUnit != null)
                    scannedUnits.Add(scannedUnit);
            }

            return new ScanAroundResult(scannedUnits);
        }
    }
}
