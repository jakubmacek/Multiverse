using System;

namespace Multiverse
{
    public class Player : IPlayer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; } = string.Empty;
    }
}
