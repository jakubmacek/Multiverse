using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class LuaScriptingEngine : ScriptingEngine
    {
        public LuaScriptingEngine(IScript script)
            : base(script)
        {
        }
    }
}
