using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class Forest : Unit, WoodSource
    {
        public override bool Indestructible => true;

        public override int MaxHealth => 0;

        public override bool Immovable => true;

        public override int MaxMovement => 0;

        public override IScanCapability ScanCapability => ScanCapabilities.Nothing;

        public override IEnumerable<UnitAbility> CreateAbilities()
        {
            return new UnitAbility[0];
        }

        public override int GetResourceCapacity(int resourceId)
        {
            if (resourceId == R.Wood.Id)
                return 999999;
            else
                return 0;
        }
    }
}
