using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class SettlerStartBuildingSite : StartBuildingSite
    {
        public override IEnumerable<UnitType> AvailableBuildingSiteTypes => new UnitType[] { U.WarehouseBuildingSite };

        public override int ActionPointCost => 50;
    }
}
