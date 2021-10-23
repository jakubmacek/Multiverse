using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct ResourceCapacity
    {
        public Resource Resource { get; }

        public int MaxAmount { get; }

        public ResourceCapacity(Resource resource, int maxAmount)
        {
            Resource = resource;
            MaxAmount = maxAmount;
        }
    }
}
