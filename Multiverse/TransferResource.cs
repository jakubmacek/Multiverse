using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class TransferResource : UnitAbility
    {
        public override string Name => "TransferResource";

        public override UnitAbilityType Type => UnitAbilityType.ResourceGathering;

        public override int UsesRestoredOnCooldown => 3;

        public override int MaxAvailableUses => 3;

        public override ulong CooldownTime => 1;

        public override int ActionPointCost => 10;

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            if (use.Resource == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidResource);
            if (!use.Resource.Value.FreelyTransferrable)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidResource);

            if (use.Amount < 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NotEnoughResources);
            if (use.Amount == 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NothingToDo);

            if (use.TargetUnit == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (!unit.Place.Equals(use.TargetUnit.Place))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.TargetUnitIsTooFar);

            var transferred = universe.TransferResource(unit, use.TargetUnit, use.Resource.Value, use.Amount);
            if (transferred.TransferredAmount == 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NotEnoughCapacity, transferred.TransferredAmount);
            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success, transferred.TransferredAmount);
        }
    }
}
