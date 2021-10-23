using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class SimpleUniverseFactory : UniverseFactory
    {
        public Universe Create(UniversePersistence persistence)
        {
            var world = persistence.World;
            if (world.Universe != nameof(SimpleUniverse))
                throw new ArgumentException($"This world is in '{world.Universe}' universe.", nameof(persistence));
            return new SimpleUniverse(persistence);
        }
    }
}
