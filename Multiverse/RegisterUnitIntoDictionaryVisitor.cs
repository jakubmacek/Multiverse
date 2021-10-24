using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse
{
    public class RegisterUnitIntoDictionaryVisitor : IUnitTypeVisitor
    {
        private readonly IUniverse _universe;
        private readonly IDictionary<string, UnitType> _units;

        public RegisterUnitIntoDictionaryVisitor(IUniverse universe, IDictionary<string, UnitType> units)
        {
            _universe = universe;
            _units = units;
        }

        public void Visit<T>() where T : Unit
        {
            var type = typeof(T);
            var displayNameAttribute = type.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().FirstOrDefault();
            var displayName = displayNameAttribute != null ? displayNameAttribute.DisplayName : type.Name;
            var unitType = new UnitType(type.Name, type.Name, type);
            _units.Add(unitType.ConstantName, unitType);
        }
    }
}
