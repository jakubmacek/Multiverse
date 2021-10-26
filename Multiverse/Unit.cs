using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public abstract class Unit : IEquatable<IUnit>, IUnit
    {
        public virtual Guid Id { get; set; }

        public virtual Player? Player { get; set; }

        public virtual World? World { get; set; }

        public virtual Place Place { get; set; }

        public virtual PlayerData PlayerData { get; set; }

        public virtual Script? Script { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        public virtual int Health { get; set; }

        public abstract bool Indestructible { get; }

        public abstract int MaxHealth { get; }

        public virtual int MovementPoints { get; set; }

        public abstract bool Immovable { get; }

        public abstract int MaxMovementPoints { get; }

        public virtual int ActionPoints { get; set; }

        public abstract int MaxActionPoints { get; }

        public virtual bool InBattle { get; set; }

        public abstract IScanCapability ScanCapability { get; }

        public virtual Dictionary<int, int> Resources { get; set; } = new Dictionary<int, int>();

        public virtual ICollection<IUnitAbility> Abilities { get; set; } = new List<IUnitAbility>();

        public virtual bool Dead => !Indestructible && Health == 0;

        public abstract int GetResourceCapacity(int resourceId);

        //public virtual ResourceCapacity GetResourceCapacity(Resource resource)
        //{
        //    return new ResourceCapacity(resource, GetResourceCapacity(resource.Id));
        //}

        public abstract IEnumerable<IUnitAbility> CreateAbilities();

        public virtual int GetResourceAmount(int resourceId)
        {
            if (Resources.TryGetValue(resourceId, out int amount))
                return amount;
            return 0;
        }

        public virtual void SetResourceAmount(int resourceId, int amount)
        {
            Resources[resourceId] = amount;
        }

        public virtual TransferResourceResult AddResource(Resource resource, int amount)
        {
            if (amount <= 0)
                return new TransferResourceResult(TransferResourceResultType.NothingToTransfer, 0, amount);

            var capacity = GetResourceCapacity(resource.Id);
            if (capacity == 0)
                return new TransferResourceResult(TransferResourceResultType.NoCapacity, 0, amount);

            int currentAmount;
            Resources.TryGetValue(resource.Id, out currentAmount);
            if (currentAmount >= capacity)
                return new TransferResourceResult(TransferResourceResultType.OverCapacity, 0, amount);

            var addedAmount = Math.Min(amount, capacity - currentAmount);
            SetResourceAmount(resource.Id, currentAmount + addedAmount);
            if (addedAmount == amount)
                return new TransferResourceResult(TransferResourceResultType.TransferredCompletely, addedAmount, 0);
            else
                return new TransferResourceResult(TransferResourceResultType.TransferredPartially, addedAmount, amount - addedAmount);
        }

        //public virtual UnitResource GetRemainingCapacity(Resource resource)
        //{
        //    var capacity = GetResourceCapacity(resource);
        //    if (capacity.MaxAmount == 0)
        //        return new UnitResource(resource, 0);

        //    int currentAmount;
        //    Resources.TryGetValue(resource.Id, out currentAmount);
        //    return new UnitResource(resource, Math.Max(0, capacity.MaxAmount - currentAmount));
        //}

        public virtual TransferResourceResult RemoveResource(Resource resource, int amount)
        {
            if (amount <= 0)
                return new TransferResourceResult(TransferResourceResultType.NothingToTransfer, 0, amount);

            int currentAmount;
            Resources.TryGetValue(resource.Id, out currentAmount);
            var removedAmount = Math.Min(amount, currentAmount);
            if (removedAmount == 0)
                return new TransferResourceResult(TransferResourceResultType.NothingToTransfer, 0, amount);

            SetResourceAmount(resource.Id, currentAmount - removedAmount);
            if (removedAmount == amount)
                return new TransferResourceResult(TransferResourceResultType.TransferredCompletely, removedAmount, 0);
            else
                return new TransferResourceResult(TransferResourceResultType.TransferredPartially, removedAmount, amount - removedAmount);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Unit);
        }

        public virtual bool Equals(IUnit? other)
        {
            return other != null &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
