using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class Universe : IUniverse
    {
        public IWorld World { get; }

        public UniversePersistence Persistence { get; }

        public IReadOnlyCollection<Resource> Resources { get; }

        private ScriptingEngineFactory scriptingEngineFactory;

        public Universe(UniversePersistence persistence, IEnumerable<Resource> resources)
        {
            Persistence = persistence;
            World = persistence.World;
            Resources = ImmutableArray.Create(resources.ToArray());
            scriptingEngineFactory = new ScriptingEngineFactory();
        }

        public virtual T SpawnUnit<T>(IPlayer player, Place place) where T : class, IUnit
        {
            var unit = CreateUnit<T>(player, place);
            InitializeUnit(unit, player, place);
            Persistence.Save(unit);
            return (T)unit;
        }

        public abstract IUnit CreateUnit<T>(IPlayer player, Place place) where T : class, IUnit;

        protected virtual void InitializeUnit(IUnit unit, IPlayer player, Place place)
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

        public UnitAbilityUseResult UseAbility<T>(IUnit unit, UnitAbilityUse use) where T : class, IUnitAbility
        {
            var ability = unit.Abilities.OfType<T>().FirstOrDefault();
            if (ability == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility);

            return UseAbility(unit, ability, use);
        }

        protected UnitAbilityUseResult UseAbility(IUnit unit, IUnitAbility ability, UnitAbilityUse use)
        {
            if (ability.RemainingUses <= 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoRemainingUses);

            ability.RemainingUses--;
            return ability.Use(this, unit, use);
        }

        public virtual MoveUnitResult MoveUnit(IUnit unit, Place place, int movementRequired)
        {
            if (unit.Immovable)
                return new MoveUnitResult() { Type = MoveUnitResultType.Immovable };

            //TODO Implementovat pohyb. Kontrolu sousedstvi places, kontrolu dostatku pohybovych bodu. Odebirani pohybovych bodu.
            unit.Place = place;
            return new MoveUnitResult() { Type = MoveUnitResultType.Moved };
        }

        public virtual TransferResourceResult TransferResource(IUnit from, IUnit to, Resource resource, int amount)
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
            return added;
        }

        protected virtual void CooldownForAllAbilities(IEnumerable<IUnit> units)
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
                    Persistence.Save(unit);
            }
        }

        protected virtual void RunEventScript(IEnumerable<IUnit> units, Event @event)
        {
            foreach (var unit in units)
            {
                if (unit.Script == null)
                    continue;

                var scriptingEngine = scriptingEngineFactory.Create(unit.Script);
                scriptingEngine.RunEvent(@event);

                Persistence.Save(unit);
            }
        }

        public virtual void Tick()
        {
            CooldownForAllAbilities(Persistence.Units);

            World.Timestamp++;
            Persistence.SaveWorld();

            var tickEvent = new Event(this, World.Timestamp, EventType.Tick);

            RunEventScript(Persistence.Units, tickEvent); //TODO fronta vsech jednotek, ktere muzou reagovat skriptem na tick; postupne vyrizovat v davkach; pridavat do fronty udalosti na ktere je potreba reagovat
        }
    }
}
