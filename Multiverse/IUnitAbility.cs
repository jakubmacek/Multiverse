namespace Multiverse
{
    public interface IUnitAbility
    {
        string Name { get; }

        long CooldownTime { get; }

        long CooldownTimestamp { get; set; }

        int MaxAvailableUses { get; }

        int RemainingUses { get; set; }

        UnitAbilityType Type { get; }

        int UsesRestoredOnCooldown { get; }

        int ActionPointCost { get; }

        UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use);
    }
}
