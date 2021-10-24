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

        public abstract int TicksToBuild { get; }

        public virtual int BuiltTicks { get; set; }

        public override int MaxActionPoints => 0;

        public override IScanCapability ScanCapability => ScanCapabilities.Nothing;

        public abstract IEnumerable<ResourceAmount> RequiredResourcesPerTick { get; }

        public IEnumerable<ResourceAmount> RequiredResourcesTotal => RequiredResourcesPerTick.Select(x => new ResourceAmount(x.ResourceId, x.Amount * TicksToBuild));

        public override IEnumerable<IUnitAbility> CreateAbilities()
        {
            return new IUnitAbility[0];
        }

        public override int GetResourceCapacity(int resourceId)
        {
            foreach (var resourceAmount in RequiredResourcesPerTick)
                if (resourceAmount.ResourceId == resourceId)
                    return (TicksToBuild - BuiltTicks) * resourceAmount.Amount;
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
