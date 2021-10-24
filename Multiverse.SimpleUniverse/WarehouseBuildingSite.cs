using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class WarehouseBuildingSite : BuildingSite<Warehouse>
    {
        public override int TicksToBuild => 3;

        public override IEnumerable<ResourceAmount> RequiredResourcesPerTick
        {
            get
            {
                yield return new ResourceAmount(R.BuildingWork.Id, 150);
                yield return new ResourceAmount(R.Wood.Id, 10);
            }
        }

    }
}
