using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Server
{
    public class UniverseRegistrations
    {
        private readonly IRepositoryFactoryFactory _repositoryFactoryFactory;
        private readonly Dictionary<string, IUniverseRegistration> _registrations = new Dictionary<string, IUniverseRegistration>();

        public UniverseRegistrations(IRepositoryFactoryFactory repositoryFactoryFactory)
        {
            _repositoryFactoryFactory = repositoryFactoryFactory;
        }

        public void Register(IUniverseRegistration universeRegistration)
        {
            if (_registrations.ContainsKey(universeRegistration.Name))
                throw new ArgumentException($"Universe '{universeRegistration.Name}' is already registered.");
            _registrations.Add(universeRegistration.Name, universeRegistration);
        }

        public void RegisterFromAssembly(string assemblyPath)
        {
            RegisterFromAssembly(Assembly.LoadFile(assemblyPath));
        }

        public void RegisterFromAssembly(Assembly assembly)
        {
            assembly.GetTypes()
                .Where(x => x.IsClass)
                .Where(x => typeof(IUniverse).IsAssignableFrom(x))
                .Select(x => x.GetCustomAttribute<UniverseRegistrationAttribute>())
                .Where(x => x != null)
                .Select(x => x.CreateRegistration())
                .ToList()
                .ForEach(Register);
        }

        public IUniverse CreateUniverse(string universe, int worldId)
        {
            if (!_registrations.TryGetValue(universe, out var universeRegistration))
                throw new ArgumentException($"Universe '{universe}' for world '{worldId}' is not registered.", nameof(universe));

            return universeRegistration.Factory.Create(_repositoryFactoryFactory, worldId);
        }
    }
}
