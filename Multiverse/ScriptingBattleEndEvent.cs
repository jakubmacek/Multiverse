using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingBattleEndEvent : ScriptingBattleEvent
    {
        public ScriptingBattleEndEvent()
            : base(BattleEventType.End)
        {
        }
    }
}
