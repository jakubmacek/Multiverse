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
                //filterByType = FilterByType,
            });
        }

        class Implementation
        {
            public Func<ScriptingUnitSelf, List<ScriptingUnit>>? scanAround;
            //public Func<string, List<ScriptingUnit>, List<ScriptingUnit>>? filterByType;
        }

        public List<ScriptingUnit> ScanAround(ScriptingUnitSelf self)
        {
            return _universe.ScanAroundUnit(self.idguid).Units;
        }
    }
}
