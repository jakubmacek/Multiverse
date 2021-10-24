using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
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
}
