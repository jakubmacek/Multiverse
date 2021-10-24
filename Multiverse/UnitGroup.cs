using System;
using System.Collections.Generic;

namespace Multiverse
{
    public class UnitGroup : IUnitGroup
    {
        public virtual Guid Id { get; set; }

        public virtual World? World { get; set; }

        public virtual Player? Player { get; set; }

        public virtual string? Name { get; set; }

        public virtual PlayerData PlayerData { get; set; }

        public virtual ISet<Unit> Units { get; set; } = new HashSet<Unit>();
    }
}
