using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class Script : IScript
    {
        public virtual Guid Id { get; set; }

        public virtual IPlayer Player { get; set; }

        public virtual string Name { get; set; }

        public virtual ScriptEngineType Type { get; set; }

        public virtual string Source { get; set; }
    }
}
