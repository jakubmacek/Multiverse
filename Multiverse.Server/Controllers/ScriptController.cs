using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multiverse.Server.Controllers
{
    [ApiController]
    [Route("worlds/{worldId}/scripts")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ScriptController : ControllerBase
    {
        private readonly RunningWorlds _runningWorlds;

        public ScriptController(RunningWorlds runningWorlds)
        {
            _runningWorlds = runningWorlds;
        }

        public class PlayerScript
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Type { get; set; }

            public string? Source { get; set; }

            public PlayerScript(IScript script)
            {
                Id = script.Id;
                Name = script.Name ?? "?";
                Type = script.Type.ToString();
                Source = script.Source;
            }
        }

        [HttpGet(Name = "GetScripts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult<List<PlayerScript>> GetScripts(int worldId)
        {
            if (User.Identity?.Name == null)
                return Unauthorized();

            var runningWorld = _runningWorlds[worldId];
            var universe = runningWorld.Universe;
            var playerId = int.Parse(User.Identity.Name);

            return universe.Repository.Scripts
                .Where(x => x.Player!.Id == playerId)
                .ToList()
                .Select(x => new PlayerScript(x) {Source = null}) // list should be shorter, without source code
                .ToList();
        }

        [HttpGet("{id}", Name = "GetScript")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public ActionResult<PlayerScript> GetScript(int worldId, Guid id)
        {
            if (User.Identity?.Name == null)
                return Unauthorized();

            var runningWorld = _runningWorlds[worldId];
            var universe = runningWorld.Universe;
            var playerId = int.Parse(User.Identity.Name);

            var script = universe.Repository.Scripts
                .FirstOrDefault(x => x.Player!.Id == playerId && x.Id == id);
            if (script == null)
                return NotFound();
            return new PlayerScript(script);
        }

        [HttpPost(Name = "CreateScript")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public ActionResult<Guid> CreateScript(int worldId, string name, string type, string source)
        {
            if (User.Identity?.Name == null)
                return Unauthorized();

            if (!Enum.TryParse(typeof(ScriptEngineType), type, true, out var scriptEngineTypeObject))
                return BadRequest("Invalid type. Allowed are: " + string.Join(", ", Enum.GetNames(typeof(ScriptEngineType))));

            var runningWorld = _runningWorlds[worldId];
            var universe = runningWorld.Universe;
            var playerId = int.Parse(User.Identity.Name);

            var script = new Script
            {
                Id = Guid.NewGuid(),
                Player = new Player() {Id = playerId},
                Type = (ScriptEngineType) scriptEngineTypeObject!,
                Name = name,
                Source = source
            };
            universe.Repository.Save(script);
            return script.Id;
        }

        [HttpPut("{id:guid}", Name = "UpdateScript")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult UpdateScript(int worldId, Guid id, string name, string source)
        {
            if (User.Identity?.Name == null)
                return Unauthorized();

            var runningWorld = _runningWorlds[worldId];
            var universe = runningWorld.Universe;
            var playerId = int.Parse(User.Identity.Name);

            var script = universe.Repository.Scripts
                .FirstOrDefault(x => x.Player!.Id == playerId && x.Id == id);
            if (script == null)
                return NotFound();

            script.Name = name;
            script.Source = source;
            universe.Repository.Save(script);
            return NoContent();
        }
    }
}