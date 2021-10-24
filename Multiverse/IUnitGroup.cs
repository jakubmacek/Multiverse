using System;
using System.Collections.Generic;

namespace Multiverse
{
    public interface IUnitGroup
    {
        Guid Id { get; set; }

        string? Name { get; set; }

        Player? Player { get; set; }

        PlayerData PlayerData { get; set; }

        ISet<Unit> Units { get; set; }

        World? World { get; set; }
    }
}