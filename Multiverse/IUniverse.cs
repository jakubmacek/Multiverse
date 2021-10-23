namespace Multiverse
{
    public interface IUniverse
    {
        UniversePersistence Persistence { get; }

        IWorld World { get; }

        void Tick();

        IUnit CreateUnit<T>(IPlayer player, Place place) where T : class, IUnit;

        MoveUnitResult MoveUnit(IUnit unit, Place place, int movementRequired);

        T SpawnUnit<T>(IPlayer player, Place place) where T : class, IUnit;

        TransferResourceResult TransferResource(IUnit from, IUnit to, Resource resource, int amount);

        UnitAbilityUseResult UseAbility<T>(IUnit unit, UnitAbilityUse use) where T : class, IUnitAbility;
    }
}