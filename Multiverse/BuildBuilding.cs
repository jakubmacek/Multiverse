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

            BuildTick(buildingSite);

            if (buildingSite.BuiltTicks >= buildingSite.TicksToBuild)
            {
                var building = buildingSite.SpawnBuilding(universe);
                universe.RemoveUnit(buildingSite);
            }

            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success);
        }

        private void BuildTick(BuildingSite buildingSite)
        {
            if (buildingSite.BuiltTicks >= buildingSite.TicksToBuild)
                return;

            foreach (var buildResource in buildingSite.RequiredResourcesPerTick)
                if (buildingSite.GetResourceAmount(buildResource.ResourceId) < buildResource.Amount)
                    return;

            foreach (var buildResource in buildingSite.RequiredResourcesPerTick)
                buildingSite.SetResourceAmount(buildResource.ResourceId, buildingSite.GetResourceAmount(buildResource.ResourceId) - buildResource.Amount);

            buildingSite.BuiltTicks++;
        }
    }
}
