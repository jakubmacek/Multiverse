using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Multiverse;
using Multiverse.Persistence.NHibernate;
using Multiverse.SimpleUniverse;

namespace Multiverse.SimulationSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>("", null);
            var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(new[] { configureNamedOptions }, Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory, Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());
            var loggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider(optionsMonitor) }, new LoggerFilterOptions { MinLevel = LogLevel.Information });
            var logger = loggerFactory.CreateLogger("database");

            const string filePath = "sandbox1.sqlite3";
            var configuration = SessionFactoryBuilder.CreateConfiguration()
                .SetSQLiteDatabase("Version=3;Data Source=" + filePath)
                .ConfigureMultiverse(logger);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            configuration.CreateSchema();
            using (var sessionFactory = configuration.BuildSessionFactory())
            {
                var session = sessionFactory.OpenSession();
                session.FlushMode = NHibernate.FlushMode.Auto;

                var world = new World() { Id = 1, Universe = nameof(SimpleUniverse.SimpleUniverse), Timestamp = 0 };
                session.Save(world);
                session.Flush();

                //var w = session.Get<World>(1);

                using (var persistence = new Repository(session, world))
                {
                    var universe = new SimpleUniverseFactory().Create(persistence);
                    persistence.SaveWorld();

                    //var gaia = new Player() { Id = Guid.Empty, Name = "Gaia" };
                    //var player1 = new Player() { Id = Guid.NewGuid(), Name = "PlayerOne" };
                    //var place_0_0 = persistence.GetPlace(0, 0);

                    //var forest1 = universe.SpawnUnit<Forest>(gaia, place_0_0);
                    //forest1.SetResourceAmount(new Resource(SimpleUniverse.ResourceIds.Wood), 5000);

                    //var settler1 = universe.SpawnUnit<Settler>(player1, place_0_0);

                    //universe.Tick(); //ziskam pouziti schopnosti

                    //var harvestWoodResult1 = universe.UseAbility<HarvestWood>(settler1, new UnitAbilityUse(forest1, settler1.Place));
                }
            }
        }
    }
}
