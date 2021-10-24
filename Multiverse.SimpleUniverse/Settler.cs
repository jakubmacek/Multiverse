using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    [DisplayName("Worker")]
    public class Settler : Unit
    {
        public override bool Indestructible => false;

        public override int MaxHealth => 100;

        public override bool Immovable => false;

        public override int MaxMovementPoints => 1;

        public override int MaxActionPoints => 50;

        public override IScanCapability ScanCapability => ScanCapabilities.SeeGaiaAndOwn1;

        public override IEnumerable<UnitAbility> CreateAbilities()
        {
            yield return new MoveOnLand();
            yield return new SettlerHarvestWood();
        }

        public override int GetResourceCapacity(int resourceId)
        {
            if (resourceId == R.Wood.Id)
                return 35;
            return 0;
        }
    }
}
