using System;
using System.Collections.Generic;

namespace Multiverse
{
    public interface IUnit : IEquatable<IUnit>
    {
        Guid Id { get; set; }

        string Name { get; set; }

        IPlayer Player { get; set; }

        Place Place { get; set; }

        bool Indestructible { get; }

        int Health { get; set; }

        int MaxHealth { get; }

        bool Immovable { get; }

        int Movement { get; set; }

        int MaxMovement { get; }

        IScript Script { get; set; }

        PlayerData PlayerData { get; set; }

        Dictionary<int, int> Resources { get; set; }

        ICollection<IUnitAbility> Abilities { get; set; }

        TransferResourceResult AddResource(Resource resource, int amount);

        IEnumerable<IUnitAbility> CreateAbilities();

        UnitResource GetRemainingCapacity(Resource resource);

        UnitResource GetResourceAmount(Resource resource);

        ResourceCapacity GetResourceCapacity(Resource resource);

        TransferResourceResult RemoveResource(Resource resource, int amount);

        void SetResourceAmount(Resource resource, int amount);
    }
}