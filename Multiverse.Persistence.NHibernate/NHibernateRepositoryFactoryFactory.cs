using NHibernate;
using NHibernate.Json;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using NHCfg = NHibernate.Cfg;

namespace Multiverse.Persistence.NHibernate
{
    public class NHibernateRepositoryFactoryFactory : IRepositoryFactoryFactory
    {
        private bool hasBeenDisposed;

        private Func<NHCfg.Configuration> createInitialConfiguration;

        private Dictionary<string, IRepositoryFactory> repositoryFactories = new Dictionary<string, IRepositoryFactory>();

        private List<ISessionFactory> sessionFactories = new List<ISessionFactory>();

        public NHibernateRepositoryFactoryFactory(Func<NHCfg.Configuration> createInitialConfiguration)
        {
            this.createInitialConfiguration = createInitialConfiguration;
        }

        protected NHCfg.Configuration CreateConfigurationForUniverse(string universeType, Action<IUnitTypeVisitor> enumerateUnitTypes)
        {
            JsonColumnTypeWorker.Configure(x => x.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto);

            var configuration = createInitialConfiguration();

            //configuration.SetInterceptor(new CustomInterceptor(logger));

            var map = new ModelMapper();

            map.AfterMapClass += (mi, type, c) =>
            {
                c.Table(type.Name.ToLower());
            };
            map.AfterMapProperty += (mi, propertyPath, m) =>
            {
                m.Column("`" + propertyPath.ToColumnName().ToLower() + "`");
            };
            //map.AfterMapOneToMany += (mi, propertyPath, m) =>
            //{
            //    m.Column("`" + propertyPath.ToColumnName().ToLower() + "`");
            //};

            map.AddMapping<WorldMap>();
            map.AddMapping<PlayerMap>();
            map.AddMapping<ScriptMap>();
            map.AddMapping<UnitGroupMap>();
            map.AddMapping<UnitMap>();
            map.AddMapping<MessageMap>();

            enumerateUnitTypes(new AddUnitsToModelMapper(map));

            var mappings = map.CompileMappingForAllExplicitlyAddedEntities();

            configuration.AddMapping(mappings);

            return configuration;
        }

        public IRepositoryFactory GetRepositoryFactoryForUniverse(string universeType, Action<IUnitTypeVisitor> enumerateUnitTypes)
        {
            if (repositoryFactories.TryGetValue(universeType, out var repositoryFactory))
                return repositoryFactory;

            var configuration = CreateConfigurationForUniverse(universeType, enumerateUnitTypes);
#pragma warning disable DF0010 // Marks undisposed local variables.
            var sessionFactory = configuration.BuildSessionFactory();
#pragma warning restore DF0010 // Marks undisposed local variables.
            sessionFactories.Add(sessionFactory);

            var newRepositoryFactory = new NHibernateRepositoryFactory(universeType, configuration, sessionFactory);
            repositoryFactories[universeType] = newRepositoryFactory;
            return newRepositoryFactory;
        }

        class AddUnitsToModelMapper : IUnitTypeVisitor
        {
            private ModelMapper map;

            public AddUnitsToModelMapper(ModelMapper map)
            {
                this.map = map;
            }

            public void Visit<T>() where T : Unit
            {
                map.AddMapping<UnitSubclassMap<T>>();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!hasBeenDisposed)
            {
                if (disposing)
                {
                    foreach (var sessionFactory in sessionFactories)
                        sessionFactory.Dispose();
                    sessionFactories.Clear();
                }

                hasBeenDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
