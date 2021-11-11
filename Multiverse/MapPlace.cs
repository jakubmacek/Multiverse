using System.Collections.Generic;

namespace Multiverse
{
    public struct MapPlace
    {
        public int X { get; init; }

        public int Y { get; init; }

        public List<MapUnit> Units { get; init; }

        public MapPlace(int x, int y, List<MapUnit> units)
        {
            X = x;
            Y = y;
            Units = units;
        }

        public override string ToString()
        {
            return $"MapPlace({X}, {Y})";
        }
    }
}
