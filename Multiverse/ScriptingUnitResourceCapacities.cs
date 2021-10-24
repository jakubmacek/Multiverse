using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingUnitResourceCapacities
    {
        private readonly IUnit _unit;

        public int this[int id]
        {
            get
            {
                return _unit.GetResourceCapacity(id);
            }
        }

        public ScriptingUnitResourceCapacities(IUnit unit)
        {
            _unit = unit;
        }
    }
}
