using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Scripting
{
    public class Debugging : IScriptingLibrary
    {
        private readonly IUniverse _universe;

        class Implementation
        {
            public Action<string>? error;
        }

        public Debugging(IUniverse universe)
        {
            _universe = universe;
        }

        public void Register(IScriptingEngine engine)
        {
            engine.RegisterObject("worldTimestamp", _universe.World.Timestamp);

            engine.RegisterObject("debugging", new Implementation
            {
                error = Error
            });
        }

        private void Error(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
