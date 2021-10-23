using System;
using System.Collections.Generic;

namespace Multiverse
{
    public interface IUnitGroup
    {
        Guid Id { get; set; }

        string Name { get; set; }

        IPlayer Player { get; set; }

        PlayerData PlayerData { get; set; }

        ISet<IUnit> Units { get; set; }

        IWorld World { get; set; }
    }
}