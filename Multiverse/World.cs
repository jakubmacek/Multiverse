using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class World : IWorld
    {
        public virtual int Id { get; set; }

        public virtual string Universe { get; set; }

        public virtual ulong Timestamp { get; set; }
    }
}
