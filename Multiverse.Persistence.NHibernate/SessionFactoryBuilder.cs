using Microsoft.Extensions.Logging;
using NHibernate.Bytecode;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Json;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHCfg = NHibernate.Cfg;

namespace Multiverse.Persistence.NHibernate
{
    public static class SessionFactoryBuilder
    {
        public static NHCfg.Configuration CreateConfiguration()
        {
            return new NHCfg.Configuration();
        }

        public static void CreateSchema(this NHCfg.Configuration configuration)
        {
            var schemaExport = new SchemaExport(configuration);
            schemaExport.Execute(false, true, false);
        }

        public static NHCfg.Configuration SetSQLiteDatabase(this NHCfg.Configuration configuration, string connectionString)
        {
            //PostgreSQL82Dialect, NpgsqlExtendedDriver

            configuration.SetProperty(NHCfg.Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName);
            configuration.SetProperty(NHCfg.Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName);
            configuration.SetProperty(NHCfg.Environment.ConnectionString, connectionString);
            configuration.SetProperty(NHCfg.Environment.UseSecondLevelCache, false.ToString());
            //configuration.SetProperty(NHCfg.Environment.ShowSql, true.ToString());
            configuration.SetProperty(NHCfg.Environment.ProxyFactoryFactoryClass, typeof(StaticProxyFactoryFactory).AssemblyQualifiedName);

            return configuration;
        }

        public static NHCfg.Configuration ConfigureMultiverse(this NHCfg.Configuration configuration, ILogger logger)
        {
            configuration.SetInterceptor(new CustomInterceptor(logger));

            JsonColumnTypeWorker.Configure(x => x.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto);

            var map = new ModelMapper();

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

            var mappings = map.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddMapping(mappings);

            return configuration;
        }
    }
}
