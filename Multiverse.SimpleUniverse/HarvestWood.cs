using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public abstract class HarvestWood : UnitAbility
    {
        public override string Name => "Harvest wood";

        public abstract int HarvestedOnUse { get; }

        public override UnitAbilityType Type => UnitAbilityType.ResourceGathering;

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            var woodSource = use.TargetUnit as WoodSource;
            if (woodSource == null || use.TargetUnit == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);

            if (!unit.Place.Equals(use.TargetUnit.Place))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.TargetUnitIsTooFar);

            var harvested = universe.TransferResource(use.TargetUnit, unit, R.Wood, HarvestedOnUse);
            if (harvested.TransferredAmount == 0)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.NothingToDo, harvested.TransferredAmount);
            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success, harvested.TransferredAmount);
        }
    }
}
