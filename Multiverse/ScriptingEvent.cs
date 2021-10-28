using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingEvent
    {
        public readonly long timestamp;

        public readonly string type;

        public ScriptingEvent(long timestamp, EventType type)
        {
            this.timestamp = timestamp;
            this.type = type.ToString();
        }
    }
}
