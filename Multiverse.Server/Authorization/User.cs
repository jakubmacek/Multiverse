using System.Collections.Generic;

namespace Multiverse.Server.Authorization
{
    public class User
    {
        public virtual string Name { get; set; } = string.Empty;

        public virtual string? Password { get; set; }

        public virtual string? Role { get; set; }

        public virtual List<UserPlayer> Players { get; set; } = new List<UserPlayer>();
    }
}
