using Blazor.Auth.Providers;
using Blazor.Auth.Repositories;
using Blazor.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Auth.Bootstrap
{
    public static class Extensions
    {
        public static void AddAuthInfrastructure(this IServiceCollection service)
        {
            service.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
            service.AddScoped<ILocalStorageService, LocalStorageService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddScoped<ITokenProvider, TokenProvider>();
            service.AddAuthorizationCore();
        }
    }
}
