using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct ScriptingRunEventResult
    {
        public readonly ScriptingRunEventResultType Type;

        public ScriptingRunEventResult(ScriptingRunEventResultType type)
        {
            Type = type;
        }
    }
}
