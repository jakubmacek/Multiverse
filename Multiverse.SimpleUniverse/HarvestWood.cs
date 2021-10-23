using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public abstract class HarvestWood : UnitAbility
    {
        public abstract int HarvestedOnUse { get; }

        public override UnitAbilityType Type => UnitAbilityType.ResourceGathering;

        public override UnitAbilityUseResult Use(IUniverse universe, IUnit unit, UnitAbilityUse use)
        {
            var woodSource = use.TargetUnit as WoodSource;
            if (woodSource == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);
            if (!unit.Place.Equals(use.TargetUnit.Place))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.TargetUnitIsTooFar);

            var harvested = universe.TransferResource(use.TargetUnit, unit, new Resource(ResourceIds.Wood), HarvestedOnUse);
            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success, harvested.TransferredAmount);
        }
    }
}
