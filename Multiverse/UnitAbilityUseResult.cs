using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct UnitAbilityUseResult
    {
        public readonly UnitAbilityUseResultType Type;

        public readonly int Amount;

        //TODO Rozsirit, aby bylo mozne vratit vysledky rozhlizeni. Tedy seznam informaci o Unit napriklad. Ty informace by mely byt omezene - nektere jednotky mohou pri rozlizeni poznat i jake ma protejsek zdravi a schopnosti, jine ne.
        //TODO Mozna tedy bude potreba zmenit ze struct na class.

        public UnitAbilityUseResult(UnitAbilityUseResultType type, int amount = 0)
        {
            Type = type;
            Amount = amount;
        }
    }
}
