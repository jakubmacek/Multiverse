using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public readonly struct Resource : IEquatable<Resource>
    {
        public readonly int Id;

        public readonly string ConstantName;

        public readonly string Name;

        public Resource(int id, string constantName, string name)
        {
            Id = id;
            ConstantName = constantName;
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is Resource resource && Equals(resource);
        }

        public bool Equals(Resource other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
