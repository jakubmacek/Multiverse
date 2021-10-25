using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class BuildingSite : Unit
    {
        public override bool Indestructible => false;

        public override bool Immovable => true;

        public override int MaxMovementPoints => 0;

        public override int MaxHealth => 1;

        public override int MaxActionPoints => 0;

        public override IScanCapability ScanCapability => ScanCapabilities.Nothing;

        public abstract IEnumerable<ResourceAmount> RequiredResources { get; }

        public override IEnumerable<IUnitAbility> CreateAbilities()
        {
            return new IUnitAbility[0];
        }

        public override int GetResourceCapacity(int resourceId)
        {
            foreach (var resourceAmount in RequiredResources)
                if (resourceAmount.ResourceId == resourceId)
                    return resourceAmount.Amount;
            return 0;
        }

        public abstract Building SpawnBuilding(IUniverse universe);
    }

    public abstract class BuildingSite<T> : BuildingSite where T : Building
    {
        public override Building SpawnBuilding(IUniverse universe)
        {
            if (Player == null)
                throw new ArgumentNullException(nameof(Player)); // should not happen
            return universe.SpawnUnit<T>(Player, Place);
        }
    }
}
