using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public enum ScriptingRunEventResultType
    {
        Success = 0,
        ScriptError = 1,
        EventError = 2,
        MissingOnEventHandler = 3,
    }
}
