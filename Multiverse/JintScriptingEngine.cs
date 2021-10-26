using Jint;
using Jint.Native;
using Jint.Native.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class JintScriptingEngine : ScriptingEngine
    {
        private readonly Engine engine;

        public override void RegisterObject(string name, object obj)
        {
            engine.SetValue(name, obj);
        }

        public JintScriptingEngine(IScript script, IEnumerable<IScriptingLibrary> libraries)
            : base(script)
        {
            engine = new Engine(options =>
            {
                options.Culture = new System.Globalization.CultureInfo("cs-CZ");
                options.TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                options.LimitMemory(2_000_000); // 1 MB
                //options.TimeoutInterval(TimeSpan.FromSeconds(1));
                options.MaxStatements(10000);
                //options.CancellationToken(cancellationToken);

                //options.SetWrapObjectHandler((engine, target) =>
                // {
                //     var instance = new Jint.Runtime.Interop.ObjectWrapper(engine, target);
                //     if (instance.IsArrayLike)
                //     {
                //         instance.SetPrototypeOf(engine.Realm.Intrinsics.Array.PrototypeObject);
                //     }
                //     return instance;
                // });
            });

            foreach (var library in libraries)
                library.Register(this);

            try
            {
                engine.Execute(script.Source);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public override ScriptingRunEventResult RunFunction(string functionName, params object[] parameters)
        {
            try
            {
                var oneventValue = engine.GetValue(functionName);
                var oneventFunction = oneventValue as ScriptFunctionInstance;
                if (oneventFunction == null)
                    return new ScriptingRunEventResult(ScriptingRunEventResultType.MissingEventHandler, null);

                var pars = new JsValue[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    pars[i] = JsValue.FromObject(engine, parameters[i]);

                var result = oneventFunction.Call(null, pars);

                return new ScriptingRunEventResult(ScriptingRunEventResultType.Success, result.IsString() ? result.AsString() : null);
            }
            catch (Exception ex)
            {
                return new ScriptingRunEventResult(ScriptingRunEventResultType.EventError, ex.Message);
            }
        }
    }
}
