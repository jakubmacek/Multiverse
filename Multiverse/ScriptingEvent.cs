using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingEvent
    {
        public readonly ulong timestamp;

        public readonly string type;

        public ScriptingEvent(ulong timestamp, EventType type)
        {
            this.timestamp = timestamp;
            this.type = type.ToString();
        }
    }
}
