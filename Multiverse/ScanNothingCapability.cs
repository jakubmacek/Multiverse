using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScanNothingCapability : IScanCapability
    {
        public IEnumerable<Place> GetRange(Place place)
        {
            return new Place[0];
        }

        public ScriptingUnit Scan(IUnit scanner, IUnit scanned)
        {
            return new ScriptingUnit(scanned, true, true, true, true);
        }
    }
}
