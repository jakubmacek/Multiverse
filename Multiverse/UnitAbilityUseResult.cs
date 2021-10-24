using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct UnitAbilityUseResult
    {
        public readonly UnitAbilityUseResultType Type;

        public readonly int Amount;

        public UnitAbilityUseResult(UnitAbilityUseResultType type, int amount = 0)
        {
            Type = type;
            Amount = amount;
        }
    }
}
