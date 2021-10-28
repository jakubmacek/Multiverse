using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Server.Authorization
{
    public class User
    {
        public virtual string Name { get; set; } = string.Empty;

        public virtual List<UserPlayer> Players { get; set; } = new List<UserPlayer>();
    }
}
