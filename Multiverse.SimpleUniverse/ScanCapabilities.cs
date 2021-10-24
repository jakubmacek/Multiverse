using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public static class ScanCapabilities
    {
        public static readonly IScanCapability Everything = new ScanEverythingCapability();

        public static readonly IScanCapability Nothing = new ScanNothingCapability();

        public static readonly IScanCapability SeeGaiaAndOwn = new SeeGaiaAndOwnScanCapability();
    }
}
