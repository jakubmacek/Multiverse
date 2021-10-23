using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class ConstructionSite<T> : Unit where T : Building
    {
        public override bool Indestructible => false;

        public override bool Immovable => true;

        public override int MaxHealth => 1;

        public abstract int TicksToBuild { get; }

        public virtual int BuiltTicks { get; set; }

        public abstract IEnumerable<ResourceAmount> RequiredResourcesPerTick { get; }

        public IEnumerable<ResourceAmount> RequiredResourcesTotal => RequiredResourcesPerTick.Select(x => new ResourceAmount(x.ResourceId, x.Amount * TicksToBuild));

        public override ResourceCapacity GetResourceCapacity(Resource resource)
        {
            foreach (var resourceAmount in RequiredResourcesTotal)
                if (resourceAmount.ResourceId == resource.Id)
                    return new ResourceCapacity(resource, resourceAmount.Amount);
            return new ResourceCapacity(resource, 0);
        }
    }
}
