using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class Building : Unit
    {
        public override bool Immovable => true;

        public override int MaxMovementPoints => 0;

        public override IScanCapability ScanCapability => ScanCapabilities.Nothing;
    }
}
