using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class R : Resources
    {
        public static Resource Wood = new Resource(1, "wood", "wood");

        public static IEnumerable<Resource> All
        {
            get
            {
                yield return BuildingWork;
                yield return Wood;
            }
        }
    }
}
