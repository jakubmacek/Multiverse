using System;
using System.Collections.Generic;

namespace Multiverse
{
    public class UnitGroup : IUnitGroup
    {
        public virtual Guid Id { get; set; }

        public virtual IWorld World { get; set; }

        public virtual IPlayer Player { get; set; }

        public virtual string Name { get; set; }

        public virtual PlayerData PlayerData { get; set; }

        public virtual ISet<IUnit> Units { get; set; }
    }
}
