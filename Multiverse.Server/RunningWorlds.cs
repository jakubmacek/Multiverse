using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Multiverse.Server
{
    public class RunningWorlds : IDisposable
    {
        private readonly NHibernate.ISessionFactory _sessionFactory;
        private readonly UniverseRegistrations _universeRegistrations;
        private readonly AllowedRunningWorlds _allowedWorlds;
        private readonly Timer _timer;
        private Dictionary<int, RunningWorld> _runningWorlds = new Dictionary<int, RunningWorld>();

        public RunningWorlds(UniverseRegistrations universeRegistrations, NHibernate.ISessionFactory sessionFactory, AllowedRunningWorlds allowedWorlds)
        {
            _universeRegistrations = universeRegistrations;
            _sessionFactory = sessionFactory;
            _allowedWorlds = allowedWorlds;
            _timer = new Timer(OnTimer, null, new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 5));
        }

        private void OnTimer(object? state)
        {
            foreach (var runningWorld in _runningWorlds.Values)
                runningWorld.TickIfItIsTimeTo();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public RunningWorld this[int worldId]
        {
            get
            {
                lock (_runningWorlds)
                {
                    if (_runningWorlds.TryGetValue(worldId, out var runningWorld))
                        return runningWorld;

                    if (!_allowedWorlds.Contains(worldId))
                        throw new ArgumentException($"World ID {worldId} is not allowed on this server.");

                    using (var session = _sessionFactory.OpenStatelessSession())
                    {
                        var world = session.Get<World>(worldId);
                        var universe = _universeRegistrations.CreateUniverse(world.Universe, world.Id);
                        runningWorld = new RunningWorld(universe);
                        _runningWorlds[worldId] = runningWorld;
                        return runningWorld;
                    }
                }
            }
        }

        public List<AvailableWorld> GetAvailableWorlds()
        {
            using (var session = _sessionFactory.OpenStatelessSession())
            {
                var allowedWorldIds = _allowedWorlds.ToArray();
                var worlds = session.Query<World>().Where(x => allowedWorldIds.Contains(x.Id)).ToList();
                var availableWorlds = new List<AvailableWorld>();
                foreach (var world in worlds)
                {
                    _runningWorlds.TryGetValue(world.Id, out var runningWorld);
                    availableWorlds.Add(new AvailableWorld
                    {
                        Id = world.Id,
                        Universe = world.Universe,
                        Timestamp = world.Timestamp,
                        Name = world.Name,
                        Running = runningWorld != null,
                    });
                }
                return availableWorlds;
            }
        }
    }
}
