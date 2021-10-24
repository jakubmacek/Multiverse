using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public enum UnitAbilityUseResultType
    {
        Success = 0,
        NoSuchAbility = 1,
        NoRemainingUses = 2,
        InvalidTargetUnit = 3,
        InvalidTargetPlace = 4,
        TargetUnitIsTooFar = 5,
        TargetPlaceIsTooFar = 6,
        NothingToDo = 7,
    }
}
