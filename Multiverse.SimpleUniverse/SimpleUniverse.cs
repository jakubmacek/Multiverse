using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public sealed class SimpleUniverse : Universe
    {
        public SimpleUniverse(UniversePersistence persistence)
            : base(persistence, CreateResources())
        {
        }

        private static IEnumerable<Resource> CreateResources()
        {
            yield return new Resource(ResourceIds.BuildingWork);
            yield return new Resource(ResourceIds.Wood);
        }

        public override IUnit CreateUnit<T>(IPlayer player, Place place)
        {
            var typeOfT = typeof(T);
            if (typeOfT == typeof(Settler))
                return new Settler();
            else if (typeOfT == typeof(Forest))
                return new Forest();
            else
                throw new ArgumentException("This unit type is not known in the universe.");
        }
    }
}
