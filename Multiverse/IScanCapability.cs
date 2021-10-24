using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public interface IScanCapability
    {
        public IEnumerable<Place> GetRange(Place place);

        public ScriptingUnit? Scan(IUnit scanner, IUnit scanned);
    }
}
