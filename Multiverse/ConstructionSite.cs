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

        public override int GetResourceCapacity(int resourceId)
        {
            foreach (var resourceAmount in RequiredResourcesTotal)
                if (resourceAmount.ResourceId == resourceId)
                    return resourceAmount.Amount;
            return 0;
        }
    }
}
