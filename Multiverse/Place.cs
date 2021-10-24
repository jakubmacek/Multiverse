using System;

namespace Multiverse
{
    public readonly struct Place
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

        public static bool operator ==(Place lhs, Place rhs)
        {
            return (lhs.X == rhs.X) && (lhs.Y == rhs.Y);
        }

        public static bool operator !=(Place lhs, Place rhs)
        {
            return (lhs.X != rhs.X) || (lhs.Y != rhs.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"Point({X}, {Y})";
        }
    }
}
