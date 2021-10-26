using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingBattleStartEvent : ScriptingBattleEvent
    {
        private readonly Func<ScriptingUnit, string> _addParticipantFunc;

        public ScriptingBattleStartEvent(Func<ScriptingUnit, string> addParticipantFunc)
            : base(BattleEventType.Start)
        {
            _addParticipantFunc = addParticipantFunc;
        }

        public string addParticipant(ScriptingUnit unit)
        {
            return _addParticipantFunc(unit);
        }
    }
}
