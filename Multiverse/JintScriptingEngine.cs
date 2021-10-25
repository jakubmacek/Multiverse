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
    public class JintScriptingEngine : IScriptingEngine
    {
        public IScript Script { get; init; }

        private readonly Engine engine;

        private bool hasBeenDisposed;

        private string? errorMessage;

        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                if (disposing)
                {
                    // nothing
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
            engine.SetValue(name, obj);
        }

        public JintScriptingEngine(IScript script, IEnumerable<IScriptingLibrary> libraries)
        {
            Script = script;

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

        public ScriptingRunEventResult RunEvent(Event @event, IUnit unit)
        {
            if (errorMessage != null)
                return new ScriptingRunEventResult(ScriptingRunEventResultType.ScriptError, errorMessage);

            try
            {
                var oneventValue = engine.GetValue("onevent");
                var oneventFunction = oneventValue as ScriptFunctionInstance;
                if (oneventFunction == null)
                    return new ScriptingRunEventResult(ScriptingRunEventResultType.MissingOnEventHandler, null);

                var self = new ScriptingUnitSelf(unit);
                var ev = new ScriptingEvent(@event.Timestamp, @event.Type);

                var result = oneventFunction.Call(null, new[] { JsValue.FromObject(engine, self), JsValue.FromObject(engine, ev) });
                var resultString = result.IsString() ? result.AsString() : null;

                return new ScriptingRunEventResult(ScriptingRunEventResultType.Success, resultString);
            }
            catch (Exception ex)
            {
                return new ScriptingRunEventResult(ScriptingRunEventResultType.EventError, ex.Message);
            }
        }
    }
}
