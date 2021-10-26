using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class ScriptingEngine : IScriptingEngine
    {
        public IScript Script { get; init; }

        private bool hasBeenDisposed;

        protected string? errorMessage;

        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                hasBeenDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public abstract void RegisterObject(string name, object obj);

        public ScriptingEngine(IScript script)
        {
            Script = script;
        }

        public abstract ScriptingRunEventResult RunFunction(string functionName, params object[] parameters);

        public ScriptingRunEventResult RunEvent(Event @event, IUnit unit, string eventFunctionName = "onevent")
        {
            if (errorMessage != null)
                return new ScriptingRunEventResult(ScriptingRunEventResultType.ScriptError, errorMessage);

            return RunFunction(
                eventFunctionName,
                new ScriptingUnitSelf(unit),
                new ScriptingEvent(@event.Timestamp, @event.Type)
            );
        }

        public ScriptingRunEventResult RunBattleEvent(BattleEventType battleEventType, ScriptingBattleEvent battleEvent, IUnit unit, ScriptingBattle battle)
        {
            if (errorMessage != null)
                return new ScriptingRunEventResult(ScriptingRunEventResultType.ScriptError, errorMessage);

            var eventFunctionName =
                battleEventType == BattleEventType.Start ? "onbattlestart" :
                battleEventType == BattleEventType.End ? "onbattleend" :
                "onbattleround";

            return RunFunction(
                eventFunctionName,
                new ScriptingUnitSelf(unit),
                battleEvent,
                battle
            );
        }
    }
}
