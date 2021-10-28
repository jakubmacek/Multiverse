using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class SettlerHarvestWood : HarvestWood
    {
        public override int UsesRestoredOnCooldown => 2;

        public override int MaxAvailableUses => 5;

        public override long CooldownTime => 1;

        public override int HarvestedOnUse => 10;

        public override int ActionPointCost => 10;
    }
}
