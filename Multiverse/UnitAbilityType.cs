using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    [Flags]
    public enum UnitAbilityType
    {
        Other = 1,
        Movement = 2,
        Attack = 4,
        UnitCreation = 8,
        ResourceGathering = 16,
    }
}
