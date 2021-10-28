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

        public World World { get; init; }

        public IRepository Repository { get; init; }

        public IDictionary<int, Resource> Resources { get; init; }

        public IDictionary<string, UnitType> UnitTypes { get; init; }

        protected ScriptingEngineFactory ScriptingEngineFactory { get; init; }

        protected Queue<Battle> Battles { get; init; }

        public ISoundEffects SoundEffects { get; set; }

        public Universe(IRepositoryFactoryFactory repositoryFactoryFactory, int worldId, IEnumerable<Resource> resources)
        {
            SoundEffects = new SoundEffects();

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
            Battles = new Queue<Battle>();
        }

        public virtual void RemoveUnit(Unit unit)
        {
            Repository.Delete(unit);
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
            var unitType = UnitTypes.Values.First(x => x.Type == typeof(T));
            return (T)SpawnUnit(unitType, player, place);
        }

        public virtual Unit SpawnUnit(UnitType unitType, Player player, Place place)
        {
            var unit = CreateUnit(unitType, player, place);
            InitializeUnit(unit, player, place);
            Repository.Save(unit);
            return unit;
        }

        public abstract Unit CreateUnit(UnitType unitType, Player player, Place place);

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
            unit.MovementPoints = unit.MaxMovementPoints;
            unit.Abilities.Clear();
            foreach (var ability in unit.CreateAbilities())
            {
                ability.CooldownTimestamp = worldTimestamp + ability.CooldownTime;
                unit.Abilities.Add(ability);
            }
        }

        public virtual UnitAbilityUseResult UseAbility<T>(Unit unit, UnitAbilityUse use) where T : class, IUnitAbility
        {
            var ability = unit.Abilities.OfType<T>().FirstOrDefault();
            if (ability == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility);

            return UseAbility(unit, ability, use);
        }

        public virtual UnitAbilityUseResult UseAbility(Unit unit, string abilityName, UnitAbilityUse use)
        {
            var ability = unit.Abilities.Where(x => x.Name == abilityName).FirstOrDefault();
            if (ability == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility);

            return UseAbility(unit, ability, use);
        }

        protected virtual UnitAbilityUseResult UseAbility(Unit unit, IUnitAbility ability, UnitAbilityUse use)
        {
            if (unit.Dead)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.UnitIsDead);
            if (ability.RemainingUses <= 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoRemainingUses);
            if (unit.ActionPoints < ability.ActionPointCost)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NoRemainingUses);
            if (unit.Dead)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NothingToDo);

            if (unit.InBattle == null && (ability is IUnitBattleAbility))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NotAvailableOutsideBattle);
            if (unit.InBattle != null && !(ability is IUnitBattleAbility))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.OnlyAvailableOutsideBattle);

            if (ability.CooldownTime != 0)
                ability.RemainingUses--;
            unit.ActionPoints -= ability.ActionPointCost;
            var result = ability.Use(this, unit, use);
            Repository.Save(unit);
            return result;
        }

        public virtual MoveUnitResult MoveUnit(Unit unit, Place place, int movementRequired)
        {
            if (unit.Immovable)
                return new MoveUnitResult() { Type = MoveUnitResultType.UnitIsImmovable };
            if (unit.Dead)
                return new MoveUnitResult() { Type = MoveUnitResultType.UnitIsDead };

            if (unit.MovementPoints < movementRequired)
                return new MoveUnitResult() { Type = MoveUnitResultType.NotEnoughMovement };

            unit.MovementPoints -= movementRequired;
            unit.Place = place;
            Repository.Save(unit);
            return new MoveUnitResult() { Type = MoveUnitResultType.Moved };
        }

        public virtual TransferResourceResult TransferResource(Unit from, Unit to, Resource resource, int amount)
        {
            if (from.Dead || to.Dead)
                return new TransferResourceResult(TransferResourceResultType.CannotTransfer, 0, amount);

            if (amount <= 0)
                return new TransferResourceResult(TransferResourceResultType.NothingToTransfer, 0, amount);

            var howMuchCanBeRemoved = Math.Min(from.GetResourceAmount(resource.Id), amount);
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
                if (unit.Dead)
                    continue;

                var unitChanged = false;

                if (unit.MovementPoints < unit.MaxMovementPoints)
                {
                    unit.MovementPoints = unit.MaxMovementPoints;
                    unitChanged = true;
                }

                if (unit.ActionPoints < unit.MaxActionPoints)
                {
                    unit.ActionPoints = unit.MaxActionPoints;
                    unitChanged = true;
                }

                foreach (var ability in unit.Abilities)
                {
                    if (ability is IUnitBattleAbility)
                    {
                        if (ability.RemainingUses != 0 || ability.CooldownTimestamp != 0)
                        {
                            ability.RemainingUses = 0;
                            ability.CooldownTimestamp = 0;
                            unitChanged = true;
                        }
                    }
                    else
                    {
                        if (ability.RemainingUses < ability.MaxAvailableUses)
                        {
                            if (ability.CooldownTime != 0 && ability.CooldownTimestamp <= worldTimestamp)
                            {
                                ability.RemainingUses = Math.Min(ability.MaxAvailableUses, ability.RemainingUses + ability.UsesRestoredOnCooldown);
                                ability.CooldownTimestamp = worldTimestamp + ability.CooldownTime;
                                unitChanged = true;
                            }
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
            lock (this)
            {
                CooldownForAllAbilities(Repository.Units);

                World.Timestamp++;
                Repository.SaveWorld();

                var tickEvent = new Event(this, World.Timestamp, EventType.Tick);

                BatchRunEventScriptForAllUnits(tickEvent);

                ResolveAllBattles();
            }
        }

        public virtual void EnsureInitialWorldState()
        {
        }

        public virtual ScanAroundResult ScanAround(Unit self)
        {
            if (self.Dead)
                return new ScanAroundResult(new List<ScriptingUnit>());

            var scanCapability = self.ScanCapability;
            var placesInRange = scanCapability.GetRange(self.Place).ToList();
            if (placesInRange.Count == 0)
                return new ScanAroundResult(new List<ScriptingUnit>());

            var query = Repository.Units;
            query = query.UnitIsInPlace(placesInRange);
            var units = query.ToList();

            var scannedUnits = new List<ScriptingUnit>();
            foreach (var unit in units)
            {
                var scannedUnit = scanCapability.Scan(self, unit);
                if (scannedUnit != null)
                    scannedUnits.Add(scannedUnit);
            }

            return new ScanAroundResult(scannedUnits);
        }

        public virtual ScriptingUnit? ScanUnit(Unit self, Unit target)
        {
            if (self.Dead)
                return null;

            var scanCapability = self.ScanCapability;
            var placesInRange = scanCapability.GetRange(self.Place).ToList();
            if (placesInRange.Count == 0)
                return null;
            if (!placesInRange.Contains(target.Place))
                return null;

            return scanCapability.Scan(self, target);
        }

        public virtual UnitAbilityUseResult StartBattle(Unit initiator, Unit target)
        {
            if (target.Player == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (initiator.Player == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (target.Player.Id == initiator.Player.Id) // cannot target own units, also units cannot target themselves
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (initiator.Dead || target.Dead)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);

            Battles.Enqueue(new Battle(initiator, target));

            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success);
        }

        protected virtual void ResolveAllBattles()
        {
            while (Battles.TryDequeue(out var battle))
            {
                ResolveBattle(battle);
            }
        }

        protected virtual void ResolveBattle(Battle battle)
        {
            if (battle.Initiator.Dead || battle.Target.Dead)
                return;

            var participantQueue = new Queue<Unit>(battle.Participants);
            var unitScripts = new Dictionary<Guid, IScriptingEngine>();
            ScriptingUnit[] scriptingParticipants;
            ScriptingBattle scriptingBattle;

            UnitAbilityUseResultType AddParticipant(Unit unit)
            {
                if (battle.Participants.Count >= battle.MaxParticipants)
                    return UnitAbilityUseResultType.NotEnoughCapacity;
                if (battle.Participants.Contains(unit))
                    return UnitAbilityUseResultType.Success;
                if (unit.Dead)
                    return UnitAbilityUseResultType.UnitIsDead;
                if (unit.Player == null)
                    return UnitAbilityUseResultType.InvalidTargetUnit;
                var participantCountOfPlayer = battle.Participants.Count(x => x.Player != null && x.Player.Equals(unit.Player));
                if (participantCountOfPlayer >= battle.MaxParticipantsPerPlayer)
                    return UnitAbilityUseResultType.NotEnoughCapacity;
                battle.Participants.Add(unit);
                if (participantQueue != null)
                    participantQueue.Enqueue(unit);
                var scriptingEngine = unit.Script == null ? new DummyScriptingEngine() : ScriptingEngineFactory.Create(unit.Script);
                unitScripts.Add(unit.Id, scriptingEngine);
                // unit.ActionPoints = 0; // drain all actions points, this should prevent use of all non-battle abilities
                unit.InBattle = battle;
                return UnitAbilityUseResultType.Success;
            }

            AddParticipant(battle.Initiator);
            AddParticipant(battle.Target);

            // Battle start
            var scriptingBattleStartEvent = new ScriptingBattleStartEvent(scriptingUnit =>
            {
                var newParticipant = Repository.GetUnit(scriptingUnit.idguid);
                if (newParticipant == null)
                    return UnitAbilityUseResultType.InvalidTargetUnit.ToString();
                return AddParticipant(newParticipant).ToString();
            });

            while (participantQueue.TryDequeue(out var participant))
            {
                if (!participant.Dead)
                {
                    scriptingParticipants = battle.Participants.Select(x => new ScriptingUnit(x, false, false, false, false)).ToArray();
                    scriptingBattle = new ScriptingBattle(scriptingParticipants);
                    unitScripts[participant.Id].RunBattleEvent(BattleEventType.Start, scriptingBattleStartEvent, participant, scriptingBattle);
                }
            }

            // Battle round

            scriptingParticipants = battle.Participants.Select(x => new ScriptingUnit(x, false, false, false, false)).ToArray(); // all participants are added, fixing the list
            scriptingBattle = new ScriptingBattle(scriptingParticipants);
            var aliveParticipantCountByPlayerId = new Dictionary<int, int>();

            for (var round = 1; round <= battle.MaxRounds; round++)
            {
                aliveParticipantCountByPlayerId.Clear();

                var battleRound = new ScriptingBattleRoundEvent(round);

                foreach (var participant in battle.Participants)
                {
                    if (!participant.Dead)
                    {
                        if (participant.Player != null)
                        {
                            aliveParticipantCountByPlayerId.TryGetValue(participant.Player.Id, out var aliveParticipantCounts);
                            aliveParticipantCountByPlayerId[participant.Player.Id] = 1 + aliveParticipantCounts;
                        }

                        participant.ActionPoints = participant.MaxActionPoints;
                        foreach (var ability in participant.Abilities)
                            if (ability is IUnitBattleAbility)
                                ability.RemainingUses = ability.MaxAvailableUses;

                        unitScripts[participant.Id].RunBattleEvent(BattleEventType.Round, battleRound, participant, scriptingBattle);
                    }
                }

                if (aliveParticipantCountByPlayerId.Values.Where(x => x > 0).Count() <= 1) // only one player or no players remain alive, end of battle
                {
                    break;
                }
            }

            // Battle end

            var scriptingBattleEndEvent = new ScriptingBattleEndEvent();

            foreach (var participant in battle.Participants)
            {
                if (!participant.Dead)
                {
                    unitScripts[participant.Id].RunBattleEvent(BattleEventType.End, scriptingBattleEndEvent, participant, scriptingBattle);
                }
                foreach (var ability in participant.Abilities)
                    if (ability is IUnitBattleAbility)
                        ability.RemainingUses = 0;
                participant.InBattle = null;
                Repository.Save(participant);
            }

            foreach (var scriptingEngine in unitScripts.Values)
                scriptingEngine.Dispose();
        }
    }
}
