using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class ScanCapabilities : Multiverse.ScanCapabilities
    {
        public static readonly IScanCapability SeeGaiaAndOwn1 = new SeeGaiaAndOwnScanCapability(1);
    }
}
