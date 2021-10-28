using Multiverse.Persistence.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Json;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.Server.Persistence
{
    public class SessionFactoryCreator
    {
        public static ISessionFactory Create(Configuration configuration)
        {
            JsonColumnTypeWorker.Configure(x => x.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto);

            //configuration.SetInterceptor(new CustomInterceptor(logger));

            var map = new ModelMapper();

            map.AfterMapClass += (mi, type, c) =>
            {
                c.Table("`" + type.Name.ToLower() + "`");
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
            map.AddMapping<UserMap>();

            var mappings = map.CompileMappingForAllExplicitlyAddedEntities();

            configuration.AddMapping(mappings);

            return configuration.BuildSessionFactory();
        }

        public static Configuration CreateConfiguration(string connectionString)
        {
            var configuration = new Configuration();
            configuration.SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(NHibernate.Dialect.PostgreSQL83Dialect).AssemblyQualifiedName);
            configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(NHibernate.Driver.NpgsqlExtendedDriver).AssemblyQualifiedName);
            configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectionString);
            configuration.SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, false.ToString());
            //configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, true.ToString());
            configuration.SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(NHibernate.Bytecode.StaticProxyFactoryFactory).AssemblyQualifiedName);
            return configuration;
        }
    }
}
