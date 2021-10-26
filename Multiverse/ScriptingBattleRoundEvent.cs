using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingBattleRoundEvent : ScriptingBattleEvent
    {
        public readonly int round;

        public ScriptingBattleRoundEvent(int round)
            : base(BattleEventType.Round)
        {
            this.round = round;
        }
    }
}
