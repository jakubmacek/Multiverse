using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public struct CubeCoordinates
    {
        public readonly int X;

        public readonly int Y;

        public readonly int Z;

        public CubeCoordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
