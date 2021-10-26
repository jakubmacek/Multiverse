using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingBattle
    {
        public readonly ScriptingUnit[] participants;

        public ScriptingBattle(ScriptingUnit[] participants)
        {
            this.participants = participants;
        }
    }
}
