using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingUnitAbilities : List<ScriptingUnitAbility>
    {
        public ScriptingUnitAbilities(IEnumerable<IUnitAbility> abilities)
        {
            foreach (var ability in abilities)
                Add(new ScriptingUnitAbility(ability));
        }

        public ScriptingUnitAbility[] withType(int type)
        {
            return this.Where(x => (x.type & type) != 0).ToArray();
        }
    }
}
