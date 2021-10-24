using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class SettlerBuildBuilding : BuildBuilding
    {
        public override int BuildingWorkPerUse => 100;

        public override int ActionPointCost => 50;
    }
}
