using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public static class U
    {
        public static UnitType Forest = new UnitType("Forest", "Forest", typeof(Forest));
        public static UnitType Settler = new UnitType("Settler", "Settler", typeof(Settler));
        public static UnitType Warrior = new UnitType("Warrior", "Warrior", typeof(Warrior));
        public static UnitType Warehouse = new UnitType("Warehouse", "Warehouse", typeof(Warehouse));
        public static UnitType WarehouseBuildingSite = new UnitType("WarehouseBuildingSite", "Warehouse building site", typeof(WarehouseBuildingSite));
    }
}
