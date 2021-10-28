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

        public virtual string Universe { get; set; } = string.Empty;

        public virtual long Timestamp { get; set; }
    }
}
