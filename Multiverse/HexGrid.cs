using System;
using System.Collections.Generic;

namespace Multiverse
{
    public static class HexGrid
    {
        public static Place CubeToAxial(CubeCoordinates cube)
        {
            return new Place(cube.X, cube.Z);
        }

        public static CubeCoordinates AxialToCube(Place place)
        {
            return new CubeCoordinates(place.X, -place.X - place.Y, place.Y);
        }

        public static int Distance(CubeCoordinates a, CubeCoordinates b)
        {
            return Math.Max(Math.Abs(a.X - b.X), Math.Max(Math.Abs(a.Y - b.Y), Math.Abs(a.Z - b.Z)));
        }

        //public static int Distance(Place a, Place b)
        //{
        //    return Distance(AxialToCube(a), AxialToCube(b));
        //}

        public static int Distance(Place a, Place b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y + a.Y - b.X - b.Y) + Math.Abs(a.Y - b.Y)) / 2;
        }

        public static IEnumerable<Place> WithinDistance(Place origin, int distance)
        {
            for (var x = -distance; x <= distance; x++)
            {
                var maxy = Math.Min(distance, -x + distance);
                for (var y = Math.Max(-distance, -x - distance); y <= maxy; y++)
                {
                    //var z = -x - y
                    //results.append(cube_add(center, Cube(x, y, z)))
                    yield return new Place(origin.X + x, origin.Y + y);
                }
            }
        }

    }
}
