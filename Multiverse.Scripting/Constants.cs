using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Scripting
{
    public class Constants : IScriptingLibrary
    {
        private readonly IUniverse _universe;

        public Constants(IUniverse universe)
        {
            _universe = universe;
        }

        public void Register(IScriptingEngine engine)
        {
            foreach (var resource in _universe.Resources.Values)
                engine.RegisterObject(resource.ConstantName, resource.Id);

            foreach (var unitType in _universe.UnitTypes.Values)
                engine.RegisterObject(unitType.ConstantName, unitType.ConstantName);

            //TODO ability type flags
        }
    }
}
