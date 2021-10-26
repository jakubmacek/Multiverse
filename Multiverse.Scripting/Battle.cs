using System;

namespace Multiverse.Scripting
{
    public class Battle : IScriptingLibrary
    {
        private readonly IUniverse _universe;

        public Battle(IUniverse universe)
        {
            _universe = universe;
        }

        public void Register(IScriptingEngine engine)
        {
            engine.RegisterObject("battle", new Implementation
            {
                start = Start,
            });
        }

        class Implementation
        {
            public Func<ScriptingUnitSelf, ScriptingUnit, ScriptingUnitAbilityUseResult>? start;
        }

        public ScriptingUnitAbilityUseResult Start(ScriptingUnitSelf self, ScriptingUnit target)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.NoSuchAbility));
            var targetUnit = _universe.Repository.GetUnit(target.idguid);
            if (targetUnit == null)
                return new ScriptingUnitAbilityUseResult(new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidTargetUnit));

            return new ScriptingUnitAbilityUseResult(_universe.StartBattle(unit, targetUnit));
        }
    }
}
