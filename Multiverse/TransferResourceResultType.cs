using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public enum TransferResourceResultType
    {
        TransferredCompletely = 0,
        TransferredPartially = 1,
        OverCapacity = 2,
        NoCapacity = 3,
        NothingToTransfer = 4,
        CannotTransfer = 5,
    }
}
