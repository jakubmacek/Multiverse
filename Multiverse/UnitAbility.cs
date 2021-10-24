using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class UnitAbility : IUnitAbility
    {
        [Newtonsoft.Json.JsonIgnore]
        public abstract string Name { get; }

        [Newtonsoft.Json.JsonIgnore]
        public abstract int UsesRestoredOnCooldown { get; }

        [Newtonsoft.Json.JsonIgnore]
        public abstract int MaxAvailableUses { get; }

        [Newtonsoft.Json.JsonIgnore]
        public abstract ulong CooldownTime { get; }

        [Newtonsoft.Json.JsonIgnore]
        public abstract UnitAbilityType Type { get; }

        [Newtonsoft.Json.JsonIgnore]
        public abstract int ActionPointCost { get; }

        public virtual int RemainingUses { get; set; }

        public virtual ulong CooldownTimestamp { get; set; }

        public abstract UnitAbilityUseResult Use(Universe universe, Unit unit, UnitAbilityUse use);
    }
}
