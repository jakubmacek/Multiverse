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
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, ISessionFactory sessionFactory, IOptions<AuthenticationSettings> authenticationSettings)
        {
            _logger = logger;
            _sessionFactory = sessionFactory;
            _authenticationSettings = authenticationSettings.Value;
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

                if (string.IsNullOrEmpty(password))
                    return StatusCode(401);
                //TODO implement real password checking, with hashing and salting

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_authenticationSettings.JwtSecret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userPlayer.PlayerId.ToString()),
                        new Claim("userName", user.Name, ClaimValueTypes.String),
                        new Claim("playerId", userPlayer.PlayerId.ToString(), ClaimValueTypes.Integer),
                    }),
                    //Expires = DateTime.UtcNow.AddHours(1),
                    Expires = DateTime.UtcNow.AddYears(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new
                {
                    token = tokenString,
                    expiresAt = tokenDescriptor.Expires
                };

                //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                //var random = new Random();
                //var tokenString = new string(Enumerable.Repeat(chars, 50).Select(s => s[random.Next(s.Length)]).ToArray());

                //var accessToken = new UserAccessToken()
                //{
                //    Token = tokenString,
                //    ExpiresAt = DateTime.UtcNow.AddHours(1),
                //    User = user,
                //    PlayerId = userPlayer.PlayerId,
                //};
                //session.Save(accessToken);
                //session.Flush();

                //return new
                //{
                //    token = accessToken.Token,
                //    expiresAt = accessToken.ExpiresAt
                //};
            }
        }
    }
}
