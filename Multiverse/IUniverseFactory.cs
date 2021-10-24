using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public interface IUniverseFactory
    {
        public IUniverse Create(IRepositoryFactoryFactory repositoryFactoryFactory, int worldId);
    }
}
