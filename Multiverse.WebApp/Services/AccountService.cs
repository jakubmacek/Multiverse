using Microsoft.AspNetCore.Components;
using Multiverse.WebApp.Models;
using System.Threading.Tasks;

namespace Multiverse.WebApp.Services
{
    public interface IAccountService
    {
        PlayerInfo Player { get; }

        Task Initialize();
        Task Login(Login model);
        Task Logout();
    }

    public class AccountService : IAccountService
    {
        private const string AuthorizationTokenStorageKey = "Authorization";

        private readonly ApiClient _apiClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        private readonly IAuthorizationService _authorizationService;

        public PlayerInfo Player { get; private set; }

        public AccountService(
            ApiClient apiClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            IAuthorizationService authorizationService
        ) {
            _apiClient = apiClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _authorizationService = authorizationService;
        }

        public async Task Initialize()
        {
            _authorizationService.Token = await _localStorageService.GetItem<string>(AuthorizationTokenStorageKey);
            if (_authorizationService.IsLoggedIn)
                Player = await _apiClient.MeAsync();
        }

        public async Task Login(Login model)
        {
            var authorization = await _apiClient.AuthorizeUserAsync(model.Name, model.Password, model.PlayerId);
            await _localStorageService.SetItem(AuthorizationTokenStorageKey, authorization.Token);
            Player = await _apiClient.MeAsync();
        }

        public async Task Logout()
        {
            _authorizationService.Token = string.Empty;
            await _localStorageService.SetItem(AuthorizationTokenStorageKey, _authorizationService.Token);
            _navigationManager.NavigateTo("login");
        }
    }
}