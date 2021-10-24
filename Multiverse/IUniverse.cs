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

        Unit CreateUnit<T>(Player player, Place place) where T : Unit;

        MoveUnitResult MoveUnit(Unit unit, Place place, int movementRequired);

        T SpawnUnit<T>(Player player, Place place) where T : Unit;

        TransferResourceResult TransferResource(Unit from, Unit to, Resource resource, int amount);

        UnitAbilityUseResult UseAbility<T>(Unit unit, UnitAbilityUse use) where T : class, IUnitAbility;

        ScanAroundResult ScanAroundUnit(Guid id);

        ScanAroundResult ScanAround(Unit unit);

        void EnsureInitialWorldState();

        Place GetPlace(int x, int y);
    }
}