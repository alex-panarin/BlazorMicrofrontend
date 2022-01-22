using Blazor.Auth.Models;
using Blazor.Auth.Providers;
using Blazor.Auth.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Blazor.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtAuthStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthRepository _repository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage, 
            IAuthRepository repository,
            ILogger<AuthService> logger)
        {
            _authenticationStateProvider = authenticationStateProvider  as JwtAuthStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var loginResponse = await _repository.LoginAsync(request);
                if (loginResponse.IsSuccess)
                {
                    await _localStorage.SetItemAsync("token", loginResponse.Token);
                    _authenticationStateProvider.MarkUserAsAuthenticated(request.Email);

                }
                return loginResponse;
            }
            catch (Exception x)
            {
                _logger?.LogError(x.Message);
                return new LoginResponse { IsSuccess = false, Error = x.Message };
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            _authenticationStateProvider.MarkUserAsLoggedOut();
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            try
            {
                var result = await _repository.RegisterAsync(request);
                return result;
            }
            catch (Exception x)
            {
                _logger?.LogError(x.Message);
                return new RegisterResponse { IsSuccess = false, Error =  x.Message };
            }
        }
    }
}
