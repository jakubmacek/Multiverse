using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class SeeGaiaAndOwnScanCapability : IScanCapability
    {
        private readonly int _range;

        public SeeGaiaAndOwnScanCapability(int range)
        {
            _range = range;
        }

        public IEnumerable<Place> GetRange(Place place)
        {
            return new Place[1] { place }; //TODO dosah _range hexu
        }

        public ScriptingUnit Scan(IUnit scanner, IUnit scanned)
        {
            if (scanned.Player != null && scanner.Player != null)
                if (scanned.Player.Id == SimpleUniverse.GaiaPlayerId || scanned.Player.Id == scanner.Player.Id)
                    return new ScriptingUnit(scanned, true, true, true, true);
            return new ScriptingUnit(scanned, false, false, false, false);
        }
    }
}
