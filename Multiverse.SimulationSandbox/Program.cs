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

            const string templateFilePath = "template.sqlite3";
            const string filePath = "sandbox1.sqlite3";

            ConsoleWindow.Maximize();

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            System.IO.File.Copy(templateFilePath, filePath);

            using (var repositoryFactoryFactory = new NHibernateRepositoryFactoryFactory(() =>
            {
                var configuration = new NHibernate.Cfg.Configuration();
                configuration.SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(NHibernate.Dialect.SQLiteDialect).AssemblyQualifiedName);
                configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(NHibernate.Driver.SQLite20Driver).AssemblyQualifiedName);
                configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, "Version=3;BinaryGuid=False;Data Source=" + filePath);
                configuration.SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, false.ToString());
                //configuration.SetProperty(NHibernate.Cfg.Environment.ShowSql, true.ToString());
                configuration.SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(NHibernate.Bytecode.StaticProxyFactoryFactory).AssemblyQualifiedName);
                return configuration;
            }))
            {
                using (var universe = new SimpleUniverseFactory().Create(repositoryFactoryFactory, 1))
                {
                    universe.EnsureInitialWorldState();
                    var repository = universe.Repository;
                    var world = universe.World;

                    void play()
                    {
                        DumpWorld(universe, repository, world);
                        //System.Threading.Thread.Sleep(5000);
                        Console.ReadKey();
                        Console.Clear();
                    }

                    var gaia = repository.GetPlayer(Guid.Empty) ?? throw new ArgumentNullException("gaia");
                    var player1 = new Player() { Id = Guid.NewGuid(), Name = "PlayerOne" };
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
                    var place_0_0 = universe.GetPlace(0, 0);
                    var place_0_1 = universe.GetPlace(0, 1);

                    var forest1 = universe.SpawnUnit<Forest>(gaia, place_0_1);
                    forest1.SetResourceAmount(R.Wood, 5000);

                    var settler1 = universe.SpawnUnit<Settler>(player1, place_0_0);
                    settler1.Script = settlerScript;
                    settler1.SetResourceAmount(R.Wood, 123);
                    repository.Save(settler1);

                    play();
                    universe.Tick(); //ziskam pouziti schopnosti
                    play();
                    universe.Tick(); //ziskam pouziti schopnosti
                    play();
                    universe.Tick(); //ziskam pouziti schopnosti
                    play();

                    //UnitAbilityUseResult harvestWoodResult;
                    //do
                    //{
                    //    harvestWoodResult = universe.UseAbility<HarvestWood>(settler1, new UnitAbilityUse(forest1, settler1.Place));
                    //    play();
                    //} while (harvestWoodResult.Amount > 0);
                }
            }
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

            //TODO unit groups
            //TODO scripts
        }
    }
}
