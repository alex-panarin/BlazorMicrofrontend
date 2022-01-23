using Blazor.Auth.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Auth.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TokenProvider(ILocalStorageService localStorage,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
        }
        public async ValueTask<string> GetTokenAsync()
        {
            var token = await _localStorage.GetItemAsync("token");
            if(string.IsNullOrWhiteSpace(token))
                return string.Empty;

            if(IsTokenValid(token))
                return token;
            var refreshToken = await _localStorage.GetItemAsync("refresh");
            if(string.IsNullOrWhiteSpace(refreshToken))
                return token;
            var newToken = await RefreshTokenAsync(refreshToken);
            await _localStorage.SetItemAsync("token", newToken);

            return newToken;
        }

        private async ValueTask<string> RefreshTokenAsync(string token)
        {
            var url = _configuration.GetSection("Auth:BaseAddress").Value + "/token/refresh";
            var response = await  _httpClient.PostAsync(url, new StringContent(token));
            if(!response.IsSuccessStatusCode)
                return string.Empty;
            return await response.Content.ReadAsStringAsync();
        }

        private bool IsTokenValid(string token)
        {
            var claims = ServiceUtils.ParseClaimsFromJwt(token).ToArray();
            if (claims.Length == 0)
                return false;
            
            return claims
                .Where(c => c.Type.ToLower() == "exp")
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v) && DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(v)) < DateTime.UtcNow)
                .Any();
        }

        
    }
}
