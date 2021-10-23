using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class ScriptingEngine
    {
        public IScript Script { get; }

        private List<ScriptingLibrary> libraries;

        public ScriptingEngine(IScript script)
        {
            Script = script;
            libraries = new List<ScriptingLibrary>();
        }

        public void AddLibrary(ScriptingLibrary library)
        {
            libraries.Add(library);
            //TODO Registrovat funkce pri inicializaci enginu.
        }

        public ScriptingRunEventResult RunEvent(Event @event)
        {
            return new ScriptingRunEventResult(ScriptingRunEventResultType.Success);
        }
    }
}
