using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.SimpleUniverse
{
    public class SimpleUniverseFactory : IUniverseFactory
    {
        public IUniverse Create(IRepositoryFactoryFactory repositoryFactoryFactory, int worldId)
        {
            return new SimpleUniverse(repositoryFactoryFactory, worldId);
        }
    }
}
