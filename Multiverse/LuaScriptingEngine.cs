using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class LuaScriptingEngine : IScriptingEngine
    {
        public IScript Script { get; init; }

        private readonly Lua lua;

        private bool hasBeenDisposed;

        private string? errorMessage;

        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                if (disposing)
                {
                    lua.Dispose();
                }

                hasBeenDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void RegisterObject(string name, object obj)
        {
            lua[name] = obj;
        }

        public LuaScriptingEngine(IScript script, IEnumerable<IScriptingLibrary> libraries)
        {
            Script = script;

            lua = new Lua();

            lua.State.Encoding = Encoding.UTF8;

            // Sandbox
            lua.DoString(@"
		        import = function () end
                -- print = function () end
	        ", "sandboxing");

            //TODO Odstranit dalsi standardni knihovni funkce. Napriklad praci se soubory.

            foreach (var library in libraries)
                library.Register(this);

            try
            {
                lua.DoString(script.Source, "script");
            }
            catch (Exception ex) // NLua.Exceptions.LuaScriptException
            {
                errorMessage = ex.Message;
            }

            //TODO Omezit nejak delku behu pocatecni inicializace i volani funkce.
        }

        public ScriptingRunEventResult RunEvent(Event @event, IUnit unit)
        {
            if (errorMessage != null)
                return new ScriptingRunEventResult(ScriptingRunEventResultType.ScriptError, errorMessage);

            try
            {
                var oneventValue = lua["onevent"];
                var oneventFunction = oneventValue as LuaFunction;
                if (oneventFunction == null)
                    return new ScriptingRunEventResult(ScriptingRunEventResultType.MissingOnEventHandler, null);

                var self = new ScriptingUnitSelf(unit);
                var ev = new ScriptingEvent(@event.Timestamp, @event.Type);

                var result = oneventFunction.Call(self, ev);

                oneventFunction.Dispose();

                return new ScriptingRunEventResult(ScriptingRunEventResultType.Success, result.Length > 0 ? result[0] as string : null);
            }
            catch (Exception ex)
            {
                return new ScriptingRunEventResult(ScriptingRunEventResultType.EventError, ex.Message);
            }
        }
    }
}
