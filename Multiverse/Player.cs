using System;

namespace Multiverse
{
    public class Player : IPlayer
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
    }
}
