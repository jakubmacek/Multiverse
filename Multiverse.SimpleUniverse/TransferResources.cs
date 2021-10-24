using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class TransferResources : UnitAbility
    {
        public override string Name => "TransferResources";

        public override UnitAbilityType Type => UnitAbilityType.ResourceGathering;

        public override int UsesRestoredOnCooldown => 3;

        public override int MaxAvailableUses => 3;

        public override ulong CooldownTime => 1;

        public override int ActionPointCost => 10;

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            //TODO implementovat
            //var woodSource = use.TargetUnit as WoodSource;
            //if (woodSource == null || use.TargetUnit == null)
            //    return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);

            //if (!unit.Place.Equals(use.TargetUnit.Place))
            //    return new UnitAbilityUseResult(UnitAbilityUseResultType.TargetUnitIsTooFar);

            //var harvested = universe.TransferResource(use.TargetUnit, unit, R.Wood, HarvestedOnUse);
            //if (harvested.TransferredAmount == 0)
            //    return new UnitAbilityUseResult(UnitAbilityUseResultType.NothingToDo, harvested.TransferredAmount);
            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success, 0);
        }
    }
}
