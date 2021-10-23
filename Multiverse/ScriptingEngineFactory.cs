using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingEngineFactory
    {
        public ScriptingEngine Create(IScript script)
        {
            if (script.Type == ScriptEngineType.Lua)
                return new LuaScriptingEngine(script);
            else
                throw new ArgumentException($"Unknown scripting engine {script.Type} in script '{script.Id}'.", nameof(script));
        }
    }
}
