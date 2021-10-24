using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public interface IScriptingLibrary
    {
        public void Register(IScriptingEngine engine);
    }
}
