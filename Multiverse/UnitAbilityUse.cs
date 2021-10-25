using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct UnitAbilityUse
    {
        public readonly Unit? TargetUnit;

        public readonly Place TargetPlace;

        public string? Name;

        public readonly Resource? Resource;

        public int Amount;

        public UnitType? UnitType;

        public UnitAbilityUse(Unit? targetUnit, Place targetPlace, string? name = null, Resource? resource = null, int amount = 0, UnitType? unitType = null)
        {
            TargetUnit = targetUnit;
            TargetPlace = targetPlace;
            Name = name;
            Resource = resource;
            Amount = amount;
            UnitType = unitType;
        }
    }
}
