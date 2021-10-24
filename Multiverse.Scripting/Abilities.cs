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
            });
        }

        class Implementation
        {
            public Func<ScriptingUnitSelf, string, ScriptingUnit?, int, int, ScriptingUnitAbilityUseResult>? use;
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
    }
}
