using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class ScriptingUnit
    {
        protected IUnit _unit;

        private readonly bool _seeHealthAndMovement;

        public Guid idguid => _unit.Id;
        
        public string id => _unit.Id.ToString();

        public string type => _unit.GetType().Name;

        public string name => _unit.Name;

        public int x => _unit.Place.X;

        public int y => _unit.Place.Y;

        public bool? indestructible => _seeHealthAndMovement ? _unit.Indestructible : null;

        public int? health => _seeHealthAndMovement ? _unit.Health : null;

        public int? maxHealth => _seeHealthAndMovement ? _unit.MaxHealth : null;

        public bool? immovable => _seeHealthAndMovement ? _unit.Immovable : null;

        public int? movement => _seeHealthAndMovement ? _unit.MovementPoints : null;

        public int? maxMovement => _seeHealthAndMovement ? _unit.MaxMovementPoints : null;

        public ScriptingUnitResourceCapacities? capacities { get; init; }

        public ScriptingUnitResources? resources { get; init; }

        public ScriptingUnitAbilities? abilities { get; init; }

        public ScriptingUnit(IUnit unit, bool seeHealthAndMovement, bool seeResourceCapacities, bool seeResources, bool seeAbilities)
        {
            _unit = unit;
            _seeHealthAndMovement = seeHealthAndMovement;
            if (seeResourceCapacities)
                capacities = new ScriptingUnitResourceCapacities(unit);
            if (seeResources)
                resources = new ScriptingUnitResources(unit.Resources);
            if (seeAbilities)
                abilities = new ScriptingUnitAbilities(unit.Abilities);
        }
    }
}
