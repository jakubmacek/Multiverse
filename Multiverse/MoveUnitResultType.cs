using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public enum MoveUnitResultType
    {
        Moved = 0,
        UnitIsImmovable = 1,
        NotEnoughMovement = 2,
        UnitIsDead = 3,
    }
}
