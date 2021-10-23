using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct UnitResource
    {
        public readonly Resource Resource;

        public int Amount;

        public UnitResource(Resource resource, int amount = 0)
        {
            Resource = resource;
            Amount = amount;
        }
    }
}
