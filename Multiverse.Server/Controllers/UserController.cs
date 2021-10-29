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

        [HttpPost("{name}/authorizations")]
        public ActionResult<object> Authorize(string name, string password, int playerId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var user = session.Get<User>(name);
                if (user == null)
                    return NotFound();

                var userPlayer = user.Players.FirstOrDefault(x => x.PlayerId == playerId);
                if (userPlayer.PlayerId == 0)
                    return StatusCode(401);

                var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (passwordVerification == PasswordVerificationResult.Failed)
                    return StatusCode(401);

                if (passwordVerification == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.Password = _passwordHasher.HashPassword(user, password);
                    session.Save(user);
                    session.Flush();
                }

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

                return new
                {
                    token = tokenString,
                    expiresAt = tokenDescriptor.Expires
                };
            }
        }
    }
}
