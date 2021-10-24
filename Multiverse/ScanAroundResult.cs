using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScanAroundResult
    {
        public readonly List<ScriptingUnit> Units;

        public ScanAroundResult(List<ScriptingUnit> units)
        {
            Units = units;
        }
    }
}
