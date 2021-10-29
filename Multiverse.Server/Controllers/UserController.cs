using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Multiverse.Server.Authorization;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multiverse.Server.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger<UserController> _logger;

        public UserController(
            ILogger<UserController> logger,
            ISessionFactory sessionFactory,
            IOptions<AuthenticationSettings> authenticationSettings,
            PasswordHasher<User> passwordHasher
        )
        {
            _logger = logger;
            _sessionFactory = sessionFactory;
            _authenticationSettings = authenticationSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public class AvailablePlayers
        {
            public List<UserPlayer> Players { get; set; }

            public AvailablePlayers(List<UserPlayer> players)
            {
                Players = players;
            }
        }

        [HttpPost("{name}/players", Name = "GetAvailablePlayers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public ActionResult<AvailablePlayers> GetAvailablePlayers(string name, string password)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var user = session.Get<User>(name);
                if (user == null)
                    return NotFound();

                var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (passwordVerification == PasswordVerificationResult.Failed)
                    return StatusCode(401);

                return new AvailablePlayers(user.Players);
            }
        }

        public class Authorization
        {
            public string Token { get; set; }

            public DateTime ExpiresAt { get; set; }

            public Authorization(string token, DateTime expiresAt)
            {
                Token = token;
                ExpiresAt = expiresAt;
            }
        }

        [HttpPost("{name}/authorizations", Name = "AuthorizeUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public ActionResult<Authorization> Authorize(string name, string password, int playerId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var user = session.Get<User>(name);
                if (user == null)
                    return NotFound();

                var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (passwordVerification == PasswordVerificationResult.Failed)
                    return StatusCode(401);

                if (passwordVerification == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.Password = _passwordHasher.HashPassword(user, password);
                    session.Save(user);
                    session.Flush();
                }

                var userPlayer = user.Players.FirstOrDefault(x => x.PlayerId == playerId);
                if (userPlayer.PlayerId == 0)
                    return StatusCode(401);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_authenticationSettings.JwtSecret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userPlayer.PlayerId.ToString()),
                        new Claim("userName", user.Name, ClaimValueTypes.String),
                        new Claim("playerId", userPlayer.PlayerId.ToString(), ClaimValueTypes.Integer),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                if (user.Role != null)
                    tokenDescriptor.Subject.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role));
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new Authorization(tokenString, tokenDescriptor.Expires.Value);
            }
        }
    }
}
