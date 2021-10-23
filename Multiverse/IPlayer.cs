using System;

namespace Multiverse
{
    public interface IPlayer
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}