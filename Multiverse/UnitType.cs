using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class UnitType : IEquatable<UnitType?>
    {
        public string ConstantName { get; init; }

        public string Name { get; init; }

        public Type Type { get; init; }

        public UnitType(string constantName, string name, Type type)
        {
            ConstantName = constantName;
            Name = name;
            Type = type;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UnitType);
        }

        public override int GetHashCode()
        {
            return ConstantName.GetHashCode();
        }

        public bool Equals(UnitType? other)
        {
            return other != null &&
                   ConstantName == other.ConstantName;
        }
    }
}
