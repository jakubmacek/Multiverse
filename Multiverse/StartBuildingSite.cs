using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class StartBuildingSite : UnitAbility
    {
        public override string Name => "StartBuildingSite";

        public override int UsesRestoredOnCooldown => 1;

        public override int MaxAvailableUses => 1;

        public override ulong CooldownTime => 1;

        public override UnitAbilityType Type => UnitAbilityType.UnitCreation;

        public abstract IEnumerable<UnitType> AvailableBuildingSiteTypes { get; }

        public override UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use)
        {
            if (use.UnitType == null)
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidUnitType);
            if (!AvailableBuildingSiteTypes.Contains(use.UnitType))
                return new UnitAbilityUseResult(UnitAbilityUseResultType.InvalidUnitType);

            if (unit.Player != null)
                universe.SpawnUnit(use.UnitType, unit.Player, unit.Place);

            return new UnitAbilityUseResult(UnitAbilityUseResultType.Success);
        }
    }
}
