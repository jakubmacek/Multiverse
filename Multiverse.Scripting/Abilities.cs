using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Scripting
{
    public class Abilities : IScriptingLibrary
    {
        private readonly IUniverse _universe;

        public Abilities(IUniverse universe)
        {
            _universe = universe;
        }

        public void Register(IScriptingEngine engine)
        {
            engine.RegisterObject("abilities", new Implementation
            {
                use = Use,
                startBuildingSite = StartBuildingSite,
                build = Build,
                transferResource = TransferResource,
            });
        }

        class Implementation
        {
            public Func<ScriptingUnitSelf, string, ScriptingUnit?, int, int, ScriptingUnitAbilityUseResult>? use;
            public Func<ScriptingUnitSelf, string, ScriptingUnitAbilityUseResult>? startBuildingSite;
            public Func<ScriptingUnitSelf, ScriptingUnit, ScriptingUnitAbilityUseResult>? build;
            public Func<ScriptingUnitSelf, ScriptingUnit, int, int, ScriptingUnitAbilityUseResult>? transferResource;
        }

        public ScriptingUnitAbilityUseResult Use(ScriptingUnitSelf self, string abilityName, ScriptingUnit? targetUnit = null, int targetPlaceX = 0, int targetPlaceY = 0)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility));

            var targetUnitUnit = targetUnit == null ? null : _universe.Repository.GetUnit(targetUnit.idguid);
            var use = new UnitAbilityUse(targetUnitUnit, new Place(targetPlaceX, targetPlaceY));

            return new ScriptingUnitAbilityUseResult(_universe.UseAbility(unit, abilityName, use));
        }

        public ScriptingUnitAbilityUseResult StartBuildingSite(ScriptingUnitSelf self, string unitType)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility));
            UnitType? unitTypeType;
            if (!_universe.UnitTypes.TryGetValue(unitType, out unitTypeType) || unitTypeType == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidUnitType));

            return new ScriptingUnitAbilityUseResult(_universe.UseAbility<StartBuildingSite>(unit, new UnitAbilityUse(null, unit.Place, null, null, 0, unitTypeType)));
        }

        public ScriptingUnitAbilityUseResult Build(ScriptingUnitSelf self, ScriptingUnit buildingSite)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility));
            var buildingSiteUnit = _universe.Repository.GetUnit(buildingSite.idguid);
            if (buildingSiteUnit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit));

            return new ScriptingUnitAbilityUseResult(_universe.UseAbility<BuildBuilding>(unit, new UnitAbilityUse(buildingSiteUnit, buildingSiteUnit.Place)));
        }

        public ScriptingUnitAbilityUseResult TransferResource(ScriptingUnitSelf self, ScriptingUnit target, int resourceId, int amount)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility));
            var targetUnit = _universe.Repository.GetUnit(target.idguid);
            if (targetUnit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit));
            Resource resource;
            if (!_universe.Resources.TryGetValue(resourceId, out resource))
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidResource));

            return new ScriptingUnitAbilityUseResult(
                _universe.UseAbility<TransferResource>(
                    unit,
                    new UnitAbilityUse(targetUnit, targetUnit.Place, null, resource, amount)
                )
            );
        }
    }
}
