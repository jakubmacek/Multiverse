using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class LuaScriptingEngine : ScriptingEngine
    {
        private readonly Lua lua;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lua.Dispose();
            }
            base.Dispose(true);
        }

        public override void RegisterObject(string name, object obj)
        {
            lua[name] = obj;
        }

        public LuaScriptingEngine(IScript script, IEnumerable<IScriptingLibrary> libraries)
            : base(script)
        {
            lua = new Lua();

            lua.State.Encoding = Encoding.UTF8;

            foreach (var library in libraries)
                library.Register(this);

            lua.DoString(@"
                function setlimit()
                    local debug = debug
                        debug.sethook(
                        function()
                            debug.sethook()
                            error('Script run time exceeded')
                        end
                    , '', 10000);
                end
                setlimit()
                setlimit = nil
            ");

            // Sandbox
            lua.DoString(@"
		        require = function () end
		        import = function () end
		        dofile = function () end
		        loadfile = function () end
		        loadstring = function () end
		        setfenv = function () end
		        setmetatable = function () end
		        collectgarbage = function () end
		        os.execute = function () end
		        os.exit = function () end
		        os.getenv = function () end
		        os.remove = function () end
		        os.rename = function () end
		        os.setlocale = function () end
		        os.tmpname = function () end
                module = nil
                coroutine = nil
                package = nil
                io = nil
                debug = nil
                newproxy = nil

                -- print = function () end
	        ", "sandboxing");

            //TODO Implementovat mechanismus, kterym jde vkladat jine skripty? Musel by tu byt pristup k databazi, nebo si to prednacist jinak.

            try
            {
                lua.DoString(script.Source, "script");
            }
            catch (Exception ex) // NLua.Exceptions.LuaScriptException
            {
                errorMessage = ex.Message;
            }
        }

        public override ScriptingRunEventResult RunFunction(string functionName, params object[] parameters)
        {
            try
            {
                var oneventValue = lua[functionName];
                var oneventFunction = oneventValue as LuaFunction;
                if (oneventFunction == null)
                    return new ScriptingRunEventResult(ScriptingRunEventResultType.MissingEventHandler, null);

                var result = oneventFunction.Call(parameters);

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
