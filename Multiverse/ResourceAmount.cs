using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct ResourceAmount
    {
        public readonly int ResourceId;

        public readonly int Amount;

        public ResourceAmount(int resourceId, int amount)
        {
            ResourceId = resourceId;
            Amount = amount;
        }
    }
}
