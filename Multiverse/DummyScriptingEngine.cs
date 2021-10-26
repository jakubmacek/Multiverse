using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class DummyScriptingEngine : IScriptingEngine
    {
        public DummyScriptingEngine()
        {
        }

        public void Dispose()
        {
        }

        public void RegisterObject(string name, object obj)
        {
        }

        public ScriptingRunEventResult RunBattleEvent(BattleEventType battleEventType, ScriptingBattleEvent battleEvent, IUnit unit, ScriptingBattle battle)
        {
            return new ScriptingRunEventResult(ScriptingRunEventResultType.MissingEventHandler, null);
        }

        public ScriptingRunEventResult RunEvent(Event @event, IUnit unit, string eventFunctionName = "onevent")
        {
            return new ScriptingRunEventResult(ScriptingRunEventResultType.MissingEventHandler, null);
        }
    }
}
