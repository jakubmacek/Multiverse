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

        public readonly string? Message;

        public ScriptingRunEventResult(ScriptingRunEventResultType type, string? message)
        {
            Type = type;
            Message = message;
        }
    }
}
