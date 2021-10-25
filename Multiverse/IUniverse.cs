using System;
using System.Collections.Generic;

namespace Multiverse
{
    public interface IUniverse : IDisposable
    {
        IRepository Repository { get; }

        World World { get; }

        IDictionary<int, Resource> Resources { get; }

        IDictionary<string, UnitType> UnitTypes { get; }

        void Tick();

        MoveUnitResult MoveUnit(Unit unit, Place place, int movementRequired);

        T SpawnUnit<T>(Player player, Place place) where T : Unit;

        Unit SpawnUnit(UnitType unit, Player player, Place place);

        TransferResourceResult TransferResource(Unit from, Unit to, Resource resource, int amount);

        UnitAbilityUseResult UseAbility<T>(Unit unit, UnitAbilityUse use) where T : class, IUnitAbility;

        UnitAbilityUseResult UseAbility(Unit unit, string abilityName, UnitAbilityUse use);

        ScanAroundResult ScanAroundUnit(Guid id);

        ScanAroundResult ScanAround(Unit unit);

        void EnsureInitialWorldState();

        void RemoveUnit(Unit unit);
    }
}