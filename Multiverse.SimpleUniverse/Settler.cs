using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class Settler : Unit
    {
        public override bool Indestructible => false;

        public override int MaxHealth => 100;

        public override bool Immovable => false;

        public override int MaxMovement => 1;

        public override IEnumerable<UnitAbility> CreateAbilities()
        {
            yield return new SettlerHarvestWood();
        }

        public override ResourceCapacity GetResourceCapacity(Multiverse.Resource resource)
        {
            if (resource.Id == ResourceIds.Wood)
                return new ResourceCapacity(resource, 35);
            return new ResourceCapacity(resource, 0);
        }
    }
}
