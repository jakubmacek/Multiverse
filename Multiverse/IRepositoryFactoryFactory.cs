using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public interface IRepositoryFactoryFactory: IDisposable
    {
        public IRepositoryFactory GetRepositoryFactoryForUniverse(string universeType, Action<IUnitTypeVisitor> enumerateUnitTypes);
    }
}
