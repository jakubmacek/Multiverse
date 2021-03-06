using System;

namespace Multiverse
{
    public class Player : IPlayer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Player({Name}#{Id})";
        }
    }
}
