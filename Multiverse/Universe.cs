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
            unit.World = World;
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

        public UnitAbilityUseResult UseAbility(Unit unit, string abilityName, UnitAbilityUse use)
        {
            var ability = unit.Abilities.Where(x => x.Name == abilityName).FirstOrDefault();
            if (ability == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility);

            return UseAbility(unit, ability, use);
        }

        protected UnitAbilityUseResult UseAbility(Unit unit, IUnitAbility ability, UnitAbilityUse use)
        {
            if (ability.RemainingUses <= 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoRemainingUses);

            if (ability.CooldownTime != 0)
                ability.RemainingUses--;
            var result = ability.Use(this, unit, use);
            Repository.Save(unit);
            return result;
        }

        public virtual MoveUnitResult MoveUnit(Unit unit, Place place, int movementRequired)
        {
            if (unit.Immovable)
                return new MoveUnitResult() { Type = MoveUnitResultType.Immovable };

            if (unit.Movement < movementRequired)
                return new MoveUnitResult() { Type = MoveUnitResultType.NotEnoughMovement };

            unit.Movement -= movementRequired;
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
                        if (ability.CooldownTime != 0 && ability.CooldownTimestamp >= worldTimestamp)
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
                if (unit.Dead)
                    continue;
                var script = unit.Script;
                if (script == null)
                    continue;

                using (var scriptingEngine = ScriptingEngineFactory.Create(script))
                {
                    var result = scriptingEngine.RunEvent(@event, unit);

                    if (result.Type != ScriptingRunEventResultType.Success)
                    {
                        var message = new Message()
                        {
                            Id = Guid.NewGuid(),
                            World = World,
                            Player = script.Player,
                            Type = MessageType.Error,
                            SentAt = DateTime.Now,
                            SentAtTimestamp = World.Timestamp,
                            FromUnit = unit.Id,
                            Text = result.Type.ToString() + ": " + result.Message,
                        };
                        Repository.Save(message);
                    }
                }

                Repository.Save(unit);
            }
        }

        protected virtual void BatchRunEventScriptForAllUnits(Event @event)
        {
            const int batchSize = 100;
            var allIds = Repository.Units.Select(x => x.Id).ToList();

            for (int offset = 0; offset < allIds.Count; offset += batchSize)
            {
                var ids = allIds.GetRange(offset, Math.Min(batchSize, allIds.Count - offset));
                var units = Repository.Units.Where(x => ids.Contains(x.Id)).ToList();
                RunEventScript(units, @event);
            }
        }

        public virtual void Tick()
        {
            CooldownForAllAbilities(Repository.Units);

            World.Timestamp++;
            Repository.SaveWorld();

            var tickEvent = new Event(this, World.Timestamp, EventType.Tick);

            BatchRunEventScriptForAllUnits(tickEvent);
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
