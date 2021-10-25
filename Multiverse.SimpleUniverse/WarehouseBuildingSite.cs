using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class WarehouseBuildingSite : BuildingSite<Warehouse>
    {
        public override IEnumerable<ResourceAmount> RequiredResources
        {
            get
            {
                yield return new ResourceAmount(R.BuildingWork.Id, 150);
                yield return new ResourceAmount(R.Wood.Id, 50);
            }
        }

    }
}
