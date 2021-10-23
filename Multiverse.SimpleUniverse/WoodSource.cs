using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public interface WoodSource
    {
        public TransferResourceResult RemoveResource(Resource resource, int amount);
    }
}
