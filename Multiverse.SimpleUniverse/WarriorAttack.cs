using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class WarriorAttack : UnitAbility, IUnitBattleAbility
    {
        public override string Name => "Attack";

        public override int UsesRestoredOnCooldown => 1;

        public override int MaxAvailableUses => 1;

        public override ulong CooldownTime => 1;

        public override UnitAbilityType Type => UnitAbilityType.Attack;

        public override int ActionPointCost => 5;

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            if (use.TargetUnit == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (use.TargetUnit.Indestructible)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (use.TargetUnit.Place != unit.Place)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.TargetUnitIsTooFar);
            if (use.TargetUnit.InBattle == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (!ReferenceEquals(use.TargetUnit.InBattle, unit.InBattle))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);

            universe.SoundEffects["WarriorAttack"].Play();
            var damage = 25;
            //TODO defense/resistance
            damage = Math.Min(use.TargetUnit.Health, damage);
            use.TargetUnit.Health = use.TargetUnit.Health - damage;
            if (use.TargetUnit.Dead)
                universe.SoundEffects["Death"].Play();
            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success, damage);
        }
    }
}
