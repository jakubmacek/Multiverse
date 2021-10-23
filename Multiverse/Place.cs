using System;

namespace Multiverse
{
    public readonly struct Place : IEquatable<Place>
    {
        public readonly IWorld World;

        public readonly int X;

        public readonly int Y;

        public Place(IWorld world, int x, int y)
        {
            World = world;
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Place place && Equals(place);
        }

        public bool Equals(Place other)
        {
            return World.Id == other.World.Id &&
                   X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(World.Id, X, Y);
        }
    }
}
