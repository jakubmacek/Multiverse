using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multiverse.Server.Controllers
{
    [ApiController]
    [Route("worlds")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class WorldController : ControllerBase
    {
        private const int MapDistance = 5;
        private readonly RunningWorlds _runningWorlds;
        //private readonly ILogger<WorldController> _logger;

        public WorldController(/*ILogger<WorldController> logger, */RunningWorlds runningWorlds)
        {
            //_logger = logger;
            _runningWorlds = runningWorlds;
        }

        public class RunningWorldInfo
        {
            public int Id { get; init; }

            public string Universe { get; init; }

            public DateTime NextTickOn { get; init; }

            public RunningWorldInfo(RunningWorld running)
            {
                Id = running.Id;
                Universe = running.Universe.GetType().FullName ?? "???";
                NextTickOn = running.NextTickOn;
            }
        }

        [HttpGet("{id}", Name = "GetWorld")]
        public RunningWorldInfo Get(int id)
        {
            return new RunningWorldInfo(_runningWorlds[id]);
        }

        [HttpGet(Name = "GetWorlds")]
        public List<AvailableWorld> GetWorlds()
        {
            return _runningWorlds.GetAvailableWorlds();
        }

        [HttpGet("{worldId}/resources", Name = "GetResources")]
        public ActionResult<List<WorldResource>> GetMap(int worldId)
        {
            if (User.Identity?.Name == null)
                return Unauthorized();

            var runningWorld = _runningWorlds[worldId];
            var universe = runningWorld.Universe;
            return universe.Resources.Values.Select(x => new WorldResource(x)).ToList();
        }

        [HttpGet("{worldId}/map/{centerX}x{centerY}", Name = "GetMap")]
        public ActionResult<Map> GetMap(int worldId, int centerX, int centerY)
        {
            if (User.Identity?.Name == null)
                return Unauthorized();

            var runningWorld = _runningWorlds[worldId];
            var universe = runningWorld.Universe;
            var playerId = int.Parse(User.Identity.Name);
            return universe.ScanMap(centerX, centerY, MapDistance, playerId);
        }
    }
}
