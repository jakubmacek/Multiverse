using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class Battle
    {
        public Unit Initiator { get; init; }
        public Unit Target { get; init; }
        public ISet<Unit> Participants { get; init; } = new HashSet<Unit>();
        public int MaxRounds { get; init; }

        public Battle(Unit initiator, Unit target, int maxRounds = 100)
        {
            Initiator = initiator;
            Target = target;
            MaxRounds = maxRounds;
        }
    }
}
