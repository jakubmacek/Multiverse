using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class Event
    {
        public readonly ulong Timestamp;

        public readonly EventType Type;

        public readonly IUniverse Universe;

        public IWorld World => Universe.World;

        public Event(IUniverse universe, ulong timestamp, EventType type)
        {
            Universe = universe;
            Timestamp = timestamp;
            Type = type;
        }
    }
}
