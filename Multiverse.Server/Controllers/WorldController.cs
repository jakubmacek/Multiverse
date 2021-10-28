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
        private readonly RunningWorlds _runningWorlds;
        private readonly ILogger<WorldController> _logger;

        public WorldController(ILogger<WorldController> logger, RunningWorlds runningWorlds)
        {
            _logger = logger;
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

        [HttpGet("{id}")]
        //[Authorization.Authorize]
        public RunningWorldInfo Get(int id)
        {
            return new RunningWorldInfo(_runningWorlds[id]);
        }
    }
}
