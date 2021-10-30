using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Multiverse.WebApp.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Multiverse.WebApp.Services
{
    public interface IAccountService : INotifyPropertyChanged
    {
        AvailableWorld? SelectedWorld { get; }
        PlayerInfo Player { get; }

        Task Initialize();
        Task Login(Login model);
        Task Logout();
        void SetSelectedWorld(AvailableWorld world);
    }

    public class AccountService : IAccountService
    {
        private const string AuthorizationTokenStorageKey = "Authorization";

        private readonly ApiClient _apiClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AccountService> _logger;

        public event PropertyChangedEventHandler? PropertyChanged;

        public PlayerInfo? Player { get; private set; }

        public AvailableWorld? SelectedWorld { get; private set; }

        public AccountService(
            ILogger<AccountService> logger,
            ApiClient apiClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService,
            IAuthorizationService authorizationService
        ) {
            _logger = logger;
            _apiClient = apiClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _authorizationService = authorizationService;
        }

        public async Task Initialize()
        {
            _authorizationService.Token = await _localStorageService.GetItem<string>(AuthorizationTokenStorageKey);
            if (_authorizationService.IsLoggedIn)
            {
                try
                {
                    Player = await _apiClient.MeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        public async Task Login(Login model)
        {
            var authorization = await _apiClient.AuthorizeUserAsync(model.Name, model.Password, model.PlayerId);
            _authorizationService.Token = authorization.Token;
            await _localStorageService.SetItem(AuthorizationTokenStorageKey, _authorizationService.Token);
            Player = await _apiClient.MeAsync();
        }

        public async Task Logout()
        {
            _authorizationService.Token = string.Empty;
            await _localStorageService.SetItem(AuthorizationTokenStorageKey, _authorizationService.Token);
            _navigationManager.NavigateTo("login");
        }

        public void SetSelectedWorld(AvailableWorld world)
        {
            SelectedWorld = world;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWorld)));
        }
    }
}