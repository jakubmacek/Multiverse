using System;

namespace Multiverse
{
    public class Player : IPlayer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        //public virtual string UserName { get; set; }

        //public virtual string Password { get; set; }

        //public virtual List<AuthorizationToken> AuthorizationTokens { get; set; }

        //public virtual List<AccessRight>

        public override string ToString()
        {
            return $"Player({Name}#{Id})";
        }
    }
}
