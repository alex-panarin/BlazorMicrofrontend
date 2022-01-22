using Blazor.Auth.Models;
using Blazor.Auth.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Auth.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthRepository(HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var url = _configuration.GetSection("Auth:LoginUrl").Value;
            var response = await _httpClient.PostAsJsonAsync(url, request);
            
            if (!response.IsSuccessStatusCode)
            {
                return new LoginResponse { IsSuccess = false, Error = await response.Content.ReadAsStringAsync()};
            }
            var loginResponse = await response.Content.ReadFromJsonAsync<ResponseLogin>(ServiceUtils.JsonOptions);
            return new LoginResponse { IsSuccess = true, Token = loginResponse.Token, RefreshToken = loginResponse.RefreshToken};
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var url = _configuration.GetSection("Auth:RegisterUrl").Value;
            var value = new CreateRequest { Email = request.Email, Password = request.Password };
            var response = await _httpClient.PostAsJsonAsync(url, value, ServiceUtils.JsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                return new RegisterResponse { IsSuccess = false, Error = await response.Content.ReadAsStringAsync() };
            }

            return new RegisterResponse { IsSuccess = true};
        }
        class CreateRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        class ResponseLogin
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
