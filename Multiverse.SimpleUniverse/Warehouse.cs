using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class Warehouse : Building
    {
        public override bool Indestructible => false;

        public override int MaxHealth => 1000;

        public override int MaxActionPoints => 10;

        public override IEnumerable<IUnitAbility> CreateAbilities()
        {
            yield return new TransferResources();
        }

        public override int GetResourceCapacity(int resourceId)
        {
            if (resourceId == R.Wood.Id)
                return 1000;
            else
                return 0;
        }
    }
}
