using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class UnitAbility : IUnitAbility
    {
        public abstract int UsesRestoredOnCooldown { get; }

        public abstract int MaxAvailableUses { get; }

        public abstract ulong CooldownTime { get; }

        public abstract UnitAbilityType Type { get; }

        public virtual int RemainingUses { get; set; }

        public virtual ulong CooldownTimestamp { get; set; }

        public abstract UnitAbilityUseResult Use(IUniverse universe, IUnit unit, UnitAbilityUse use);
    }
}
