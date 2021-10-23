using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public readonly struct TransferResourceResult
    {
        public readonly TransferResourceResultType Type;

        public readonly int TransferredAmount;

        public readonly int RemainingAmount;

        public TransferResourceResult(TransferResourceResultType type, int transferredAmount, int remainingAmount)
        {
            Type = type;
            TransferredAmount = transferredAmount;
            RemainingAmount = remainingAmount;
        }
    }
}
