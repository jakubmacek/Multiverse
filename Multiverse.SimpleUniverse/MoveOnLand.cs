using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class MoveOnLand : UnitAbility
    {
        public override string Name => "Move";

        public virtual int MovementRequiredForUse => 1;

        public override UnitAbilityType Type => UnitAbilityType.Movement;

        public override int MaxAvailableUses => 1;

        public override int UsesRestoredOnCooldown => 1;

        public override ulong CooldownTime => 0;

        public override int ActionPointCost => 30;

        public MoveOnLand()
            :base()
        {
            RemainingUses = MaxAvailableUses;
        }

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            if (unit.Place.Equals(use.TargetPlace))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NothingToDo);

            var distanceToTargetPlace = HexGrid.Distance(unit.Place, use.TargetPlace);
            if (distanceToTargetPlace > 1)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetPlace); // can only move to a neighboring place

            var move = universe.MoveUnit(unit, use.TargetPlace, MovementRequiredForUse);
            if (move.Type == MoveUnitResultType.Moved)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.Success);
            else
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NotEnoughMovement);
        }
    }
}
