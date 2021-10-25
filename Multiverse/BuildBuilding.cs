using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class BuildBuilding : UnitAbility
    {
        public override string Name => "Build";

        public override int UsesRestoredOnCooldown => 1;

        public override int MaxAvailableUses => 1;

        public override ulong CooldownTime => 1;

        public override UnitAbilityType Type => UnitAbilityType.UnitCreation;

        public abstract int BuildingWorkPerUse { get; }

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            var buildingSite = use.TargetUnit as BuildingSite;
            if (buildingSite == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit);

            buildingSite.AddResource(Resources.BuildingWork, BuildingWorkPerUse);

            Finalize(universe, buildingSite);

            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success);
        }

        private void Finalize(Universe universe, BuildingSite buildingSite)
        {
            foreach (var buildResource in buildingSite.RequiredResources)
                if (buildingSite.GetResourceAmount(buildResource.ResourceId) < buildResource.Amount)
                    return;

            var building = buildingSite.SpawnBuilding(universe);
            universe.RemoveUnit(buildingSite);
        }
    }
}
