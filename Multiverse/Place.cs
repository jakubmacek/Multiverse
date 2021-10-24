using System;

namespace Multiverse
{
    public readonly struct Place : IEquatable<Place>
    {
        public readonly int X;

        public readonly int Y;

        public Place(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Place place && Equals(place);
        }

        public bool Equals(Place other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
