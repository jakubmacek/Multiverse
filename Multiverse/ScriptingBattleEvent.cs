using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingBattleEvent
    {
        public readonly string type;

        public ScriptingBattleEvent(BattleEventType type)
        {
            this.type = type.ToString();
        }
    }
}
