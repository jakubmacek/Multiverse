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

        public override IEnumerable<UnitAbility> CreateAbilities()
        {
            return new UnitAbility[0];
        }

        public override ResourceCapacity GetResourceCapacity(Multiverse.Resource resource)
        {
            if (resource.Id == ResourceIds.Wood)
                return new ResourceCapacity(resource, 999999);
            else
                return new ResourceCapacity(resource, 0);
        }
    }
}
