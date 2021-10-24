using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public interface IRepositoryFactory
    {
        public void InitializeStorage();

        public IRepository Create(int worldId);
    }
}
