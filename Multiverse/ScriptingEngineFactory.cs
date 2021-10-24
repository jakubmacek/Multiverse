using System;
using System.Collections.Generic;

namespace Multiverse
{
    public class ScriptingEngineFactory
    {
        private List<IScriptingLibrary> libraries = new List<IScriptingLibrary>();

        public IScriptingEngine Create(IScript script)
        {
            if (script.Type == ScriptEngineType.Lua)
                return new LuaScriptingEngine(script, libraries);
            else
                throw new ArgumentException($"Unknown scripting engine {script.Type} in script '{script.Id}'.", nameof(script));
        }

        public void AddLibrary(IScriptingLibrary library)
        {
            libraries.Add(library);
        }
    }
}
