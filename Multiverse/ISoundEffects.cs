using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public interface ISoundEffects
    {
        ISoundEffect this[string key] { get; }
    }
}
