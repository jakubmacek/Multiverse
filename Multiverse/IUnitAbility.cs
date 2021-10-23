namespace Multiverse
{
    public interface IUnitAbility
    {
        ulong CooldownTime { get; }

        ulong CooldownTimestamp { get; set; }

        int MaxAvailableUses { get; }

        int RemainingUses { get; set; }

        UnitAbilityType Type { get; }

        int UsesRestoredOnCooldown { get; }

        UnitAbilityUseResult Use(IUniverse universe, IUnit unit, UnitAbilityUse use);
    }
}
