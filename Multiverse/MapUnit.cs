using System;
using System.Collections.Generic;
using System.Linq;

namespace Multiverse
{
    public class MapUnit
    {
        public Guid Id { get; init; }

        public int PlayerId { get; init; }

        public string Type { get; init; }

        public string Name { get; init; }

        public int X { get; init; }

        public int Y { get; init; }

        public bool? Indestructible { get; init; }

        public int? Health { get; init; }

        public int? MaxHealth { get; init; }

        public bool? Immovable { get; init; }

        public int? Movement { get; init; }

        public int? MaxMovement { get; init; }

        public Dictionary<int, int>? Capacities { get; init; }

        public Dictionary<int, int>? Resources { get; init; }

        public List<MapUnitAbility>? Abilities { get; init; }

        public string? PlayerData { get; init; }

        public MapUnit(ScriptingUnit scriptingUnit, IEnumerable<Resource> universeResources)
        {
            Id = scriptingUnit.idguid;
            PlayerId = scriptingUnit.playerId;
            Type = scriptingUnit.type;
            Name = scriptingUnit.name;
            X = scriptingUnit.x;
            Y = scriptingUnit.y;
            Indestructible = scriptingUnit.indestructible;
            Health = scriptingUnit.health;
            MaxHealth = scriptingUnit.maxHealth;
            Immovable = scriptingUnit.immovable;
            Movement = scriptingUnit.movement;
            MaxMovement = scriptingUnit.maxMovement;

            if (scriptingUnit.capacities != null)
                Capacities = new Dictionary<int, int>(universeResources.Select(r => new KeyValuePair<int, int>(r.Id, scriptingUnit.capacities[r.Id])).Where(x => x.Value > 0));

            if (scriptingUnit.resources != null)
                Resources = new Dictionary<int, int>(universeResources.Select(r => new KeyValuePair<int, int>(r.Id, scriptingUnit.resources[r.Id])).Where(x => x.Value > 0));

            if (scriptingUnit.abilities != null)
                Abilities = scriptingUnit.abilities.Select(x => new MapUnitAbility(x)).ToList();

            if (scriptingUnit is ScriptingUnitSelf self)
                PlayerData = self.playerData;
        }
    }
}
