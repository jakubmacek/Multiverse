using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Scripting
{
    public class Scanning : IScriptingLibrary
    {
        private readonly IUniverse _universe;

        public Scanning(IUniverse universe)
        {
            _universe = universe;
        }

        public void Register(IScriptingEngine engine)
        {
            engine.RegisterObject("scanning", new Implementation
            {
                scanAround = ScanAround,
                scanUnit = ScanUnit,
            });
        }

        class Implementation
        {
            public Func<ScriptingUnitSelf, ScriptingUnit[]>? scanAround;
            public Func<ScriptingUnitSelf, ScriptingUnit, ScriptingUnit>? scanUnit;
            //public Func<string, List<ScriptingUnit>, List<ScriptingUnit>>? filterByType;
        }

        public ScriptingUnit[] ScanAround(ScriptingUnitSelf self)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return new ScriptingUnit[0];

            return _universe.ScanAround(unit).Units.ToArray();
        }

        public ScriptingUnit ScanUnit(ScriptingUnitSelf self, ScriptingUnit target)
        {
            var unit = _universe.Repository.GetUnit(self.idguid);
            if (unit == null)
                return target;
            var targetUnit = _universe.Repository.GetUnit(target.idguid);
            if (targetUnit == null)
                return target;

            return _universe.ScanUnit(unit, targetUnit) ?? target;
        }
    }
}
