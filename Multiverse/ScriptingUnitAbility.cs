using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingUnitAbility
    {
        private IUnitAbility _ability;

        public string name => _ability.Name;

        public int type => (int)_ability.Type;

        public long cooldownTime => _ability.CooldownTime;

        public long cooldownTimestamp => _ability.CooldownTimestamp;

        public int maxAvailableUses => _ability.MaxAvailableUses;

        public int remainingUses => _ability.RemainingUses;

        public int usesRestoredOnCooldown => _ability.UsesRestoredOnCooldown;

        public ScriptingUnitAbility(IUnitAbility ability)
        {
            _ability = ability;
        }
    }
}
