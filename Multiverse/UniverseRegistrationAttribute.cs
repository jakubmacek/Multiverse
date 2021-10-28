using System;

namespace Multiverse
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UniverseRegistrationAttribute : Attribute
    {
        public string Name { get; init; }

        public Type UniverseType { get; init; }

        public Type FactoryType { get; init;  }

        public UniverseRegistrationAttribute(Type universeType, Type factoryType)
        {
            if (universeType.FullName == null)
                throw new ArgumentException("Missing universeType.FullName.", nameof(universeType));
            Name = universeType.FullName;
            UniverseType = universeType;
            FactoryType = factoryType;
        }

        public IUniverseRegistration CreateRegistration()
        {
            var factory = Activator.CreateInstance(FactoryType);
            if (factory == null)
                throw new Exception($"Cannot create universe factory for '{Name}'.");
            return new UniverseRegistration(
                Name,
                (IUniverseFactory)factory
            );
        }
    }
}
