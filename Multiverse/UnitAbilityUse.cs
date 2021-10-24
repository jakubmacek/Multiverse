using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct UnitAbilityUse
    {
        public readonly Unit? TargetUnit;

        public readonly Place TargetPlace;

        public UnitAbilityUse(Unit? targetUnit, Place targetPlace)
        {
            TargetUnit = targetUnit;
            TargetPlace = targetPlace;
        }
    }
}
