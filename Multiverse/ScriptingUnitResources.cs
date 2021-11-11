using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingUnitResources
    {
        private readonly IDictionary<int, int> _resources;

        public int this[int id]
        {
            get
            {
                if (_resources.TryGetValue(id, out int v))
                    return v;
                return 0;
            }
        }

        public ScriptingUnitResources(IDictionary<int, int> resources)
        {
            _resources = resources;
        }
    }
}
