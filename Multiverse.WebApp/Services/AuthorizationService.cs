namespace Multiverse.WebApp.Services
{
    public interface IAuthorizationService
    {
        bool IsLoggedIn { get; }
        string Token { get; set; }
    }

    public class AuthorizationService : IAuthorizationService
    {
        public string Token { get; set; } = string.Empty;

        public bool IsLoggedIn => !string.IsNullOrEmpty(Token);
    }
}
