using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Multiverse.WebApp.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Multiverse.WebApp
{
    public class Program
    {
#pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddScoped(s => new HttpClientHandler()
            //{
            //    ClientCertificateOptions = ClientCertificateOption.Manual,
            //    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            //    {
            //        return true;
            //    }
            //});
            builder.Services.AddScoped(s => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IAlertService, AlertService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();

            builder.Services.AddScoped(s => new ApiClient("https://localhost:20011", s.GetRequiredService<HttpClient>()) { AuthorizationService = s.GetRequiredService<IAuthorizationService>() });

            var host = builder.Build();

            var accountService = host.Services.GetRequiredService<IAccountService>();
            await accountService.Initialize();

            await host.RunAsync();
        }
#pragma warning restore AsyncFixer01 // Unnecessary async/await usage
    }
}
