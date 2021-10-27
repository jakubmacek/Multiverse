using System;
using System.Collections.Generic;

namespace Multiverse
{
    public interface IUnit : IEquatable<IUnit>
    {
        Guid Id { get; set; }

        string Name { get; set; }

        Player? Player { get; set; }

        World? World { get; set; }

        Place Place { get; set; }

        bool Dead { get; }

        bool Indestructible { get; }

        int Health { get; set; }

        int MaxHealth { get; }

        bool Immovable { get; }

        int MovementPoints { get; set; }

        int MaxMovementPoints { get; }

        int ActionPoints { get; set; }

        int MaxActionPoints { get; }

        Battle? InBattle { get; set; }

        Script? Script { get; set; }

        PlayerData PlayerData { get; set; }

        IScanCapability ScanCapability { get; }

        Dictionary<int, int> Resources { get; }

        ICollection<IUnitAbility> Abilities { get; }

        TransferResourceResult AddResource(Resource resource, int amount);

        IEnumerable<IUnitAbility> CreateAbilities();

        //UnitResource GetRemainingCapacity(Resource resource);

        int GetResourceAmount(int resourceId);

        //ResourceCapacity GetResourceCapacity(Resource resource);

        int GetResourceCapacity(int resourceId);

        TransferResourceResult RemoveResource(Resource resource, int amount);

        void SetResourceAmount(int resourceId, int amount);
    }
}