using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
#pragma warning disable IDE1006 // Naming Styles camelCase names for scripting
    public class ScriptingUnitSelf : ScriptingUnit
    {
        public string? playerData
        {
            get { return _unit.PlayerData.Value; }
            set { _unit.PlayerData = new PlayerData() { Value = value }; }
        }

        public ScriptingUnitSelf(IUnit unit)
            : base(unit, true, true, true, true)
        {
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
