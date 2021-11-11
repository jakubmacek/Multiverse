using System.Collections.Generic;

namespace Multiverse
{
    public class Map
    {
        public int MinX { get; init; }

        public int MaxX { get; init; }

        public int MinY { get; init; }

        public int MaxY { get; init; }

        public List<MapPlace> Places { get; init; }

        public Map(int minX, int maxX, int minY, int maxY, List<MapPlace> places)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            Places = places;
        }
    }
}
