namespace Multiverse
{
    public class MapUnitAbility
    {
        public string Name { get; init; }

        public UnitAbilityType Type { get; init; }

        public long CooldownTime { get; init; }

        public long CooldownTimestamp { get; init; }

        public int MaxAvailableUses { get; init; }

        public int RemainingUses { get; init; }

        public int UsesRestoredOnCooldown { get; init; }

        public MapUnitAbility(ScriptingUnitAbility ability)
        {
            Name = ability.name;
            Type = (UnitAbilityType)ability.type;
            CooldownTime = ability.cooldownTime;
            CooldownTimestamp = ability.cooldownTimestamp;
            MaxAvailableUses = ability.maxAvailableUses;
            RemainingUses = ability.remainingUses;
            UsesRestoredOnCooldown = ability.usesRestoredOnCooldown;
        }
    }
}
