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
            const string templateFilePath = "template.sqlite3";
            const string filePath = "sandbox1.sqlite3";

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            System.IO.File.Copy(templateFilePath, filePath);

            //CreateEmptyUniverse(templateFilePath);
            RunUniverse(filePath, 3, CreateInitialState);
        }

        private static void CreateInitialState(IUniverse universe)
        {
            var repository = universe.Repository;

            var gaia = repository.GetPlayer(SimpleUniverse.SimpleUniverse.GaiaPlayerId) ?? throw new ArgumentNullException("gaia");
            var player1 = new Player() { Id = 1, Name = "PlayerOne" };
            repository.Save(player1);
            var settlerScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "Settler AI",
                Player = player1,
                Type = ScriptEngineType.Lua,
                Source = System.IO.File.ReadAllText("../../../SettlerScript.lua"),
            };
            repository.Save(settlerScript);
            var place_0_0 = new Place(0, 0);
            var place_1_0 = new Place(1, 0);

            var forest1 = universe.SpawnUnit<Forest>(gaia, place_1_0);
            forest1.SetResourceAmount(R.Wood, 5000);

            var settler1 = universe.SpawnUnit<Settler>(player1, place_0_0);
            settler1.Script = settlerScript;
            settler1.SetResourceAmount(R.Wood, 123);
            repository.Save(settler1);
        }

        private static void RunUniverse(string filePath, int howManyTicks, Action<IUniverse> initialState)
        {
            using (var repositoryFactoryFactory = CreateRepositoryFactoryFactory(filePath))
            {
                using (var universe = new SimpleUniverseFactory().Create(repositoryFactoryFactory, 1))
                {
                    universe.EnsureInitialWorldState();
                    var repository = universe.Repository;
                    var world = universe.World;

                    while (howManyTicks-- > 0)
                    {
                        DumpWorld(universe, repository, world);
                        //System.Threading.Thread.Sleep(5000);
                        universe.Tick(); //ziskam pouziti schopnosti
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        private static void CreateEmptyUniverse(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            using (var repositoryFactoryFactory = CreateRepositoryFactoryFactory(filePath))
            {
                using (var universe = new SimpleUniverseFactory().Create(repositoryFactoryFactory, 1))
                {
                    universe.EnsureInitialWorldState();
                }
            }
        }

        private static NHibernateRepositoryFactoryFactory CreateRepositoryFactoryFactory(string filePath)
        {
            return new NHibernateRepositoryFactoryFactory(() =>
            {
                var configuration = new NHibernate.Cfg.Configuration();
                configuration.SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(NHibernate.Dialect.SQLiteDialect).AssemblyQualifiedName);
                configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(NHibernate.Driver.SQLite20Driver).AssemblyQualifiedName);
                configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, "Version=3;BinaryGuid=False;Data Source=" + filePath);
                configuration.SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, false.ToString());
                //configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, true.ToString());
                configuration.SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(NHibernate.Bytecode.StaticProxyFactoryFactory).AssemblyQualifiedName);
                return configuration;
            });
        }

        private static ILogger CreateLogger()
        {
#pragma warning disable DF0010 // Marks undisposed local variables.
#pragma warning disable DF0000 // Marks undisposed anonymous objects from object creations.
            var configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>("", null);
            var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(new[] { configureNamedOptions }, Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory, Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());
            var loggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider(optionsMonitor) }, new LoggerFilterOptions { MinLevel = LogLevel.Information });
            var logger = loggerFactory.CreateLogger("database");
#pragma warning restore DF0000 // Marks undisposed anonymous objects from object creations.
#pragma warning restore DF0010 // Marks undisposed local variables.
            return logger;
        }

        static void DumpWorld(IUniverse universe, IRepository repository, World world)
        {
            Console.WriteLine($"World #{world.Id} ({world.Universe}) at time {world.Timestamp}.");

            foreach (var unit in repository.Units)
            {
                Console.WriteLine();
                Console.WriteLine($"{unit.Name} ({unit.GetType().Name} of player {unit.Player?.Name}) at {unit.Place.X}, {unit.Place.Y}");
                if (unit.Script != null)
                    Console.WriteLine($"\tscript '{unit.Script.Name}' with data '{unit.PlayerData.Value}'");
                if (unit.Indestructible)
                    Console.Write("\tindestructible");
                else
                    Console.Write($"\tHP {unit.Health} / {unit.MaxHealth}");
                if (unit.Immovable)
                    Console.Write("\timmovable");
                else
                    Console.Write($"\tMovement {unit.Movement} / {unit.MaxMovement}");
                Console.WriteLine();
                foreach (var a in unit.Abilities)
                    Console.WriteLine($"\tability '{a.Name}', uses {a.RemainingUses} / {a.MaxAvailableUses} (restore {a.UsesRestoredOnCooldown} each {a.CooldownTime}, on {a.CooldownTimestamp})");
                foreach (var r in unit.Resources)
                    Console.WriteLine($"\tresource '{universe.Resources[r.Key].Name}' ({r.Key}): {r.Value} / {unit.GetResourceCapacity(r.Key)}");
            }

            foreach (var unitGroup in repository.UnitGroups)
            {
                Console.WriteLine();
                Console.WriteLine($"Unit group '{unitGroup.Name}' of player {unitGroup.Player?.Name}");
                if (!string.IsNullOrEmpty(unitGroup.PlayerData.Value))
                    Console.WriteLine($"\tplayer data '{unitGroup.PlayerData.Value}'");
                foreach (var unit in unitGroup.Units)
                    Console.WriteLine($"\t{unit.Name} ({unit.GetType().Name} of player {unit.Player?.Name}) at {unit.Place.X}, {unit.Place.Y}");
            }
        }
    }
}
