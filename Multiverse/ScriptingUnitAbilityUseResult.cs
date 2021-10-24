using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct ScriptingUnitAbilityUseResult
    {
        public readonly string type;

        public readonly int amount;

        public ScriptingUnitAbilityUseResult(UnitAbilityUseResult result)
        {
            type = result.Type.ToString();
            amount = result.Amount;
        }
    }
}
