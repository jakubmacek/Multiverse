using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace Multiverse.Server.Controllers
{
    [ApiController]
    [Route("players")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger, ISessionFactory sessionFactory)
        {
            _logger = logger;
            _sessionFactory = sessionFactory;
        }

        public class PlayerInfo
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public PlayerInfo(Player player)
            {
                Id = player.Id;
                Name = player.Name;
            }
        }

        [HttpGet("me")]
        public ActionResult<PlayerInfo> GetMyself()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                if (User.Identity == null || User.Identity.Name == null)
                    return NotFound();

                var player = session.Get<Player>(int.Parse(User.Identity.Name));
                if (player == null)
                    return NotFound();

                return new PlayerInfo(player);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PlayerInfo> Get(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var player = session.Get<Player>(id);
                if (player == null)
                    return NotFound();

                return new PlayerInfo(player);
            }
        }
    }
}
