using Blazor.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazor.Auth.Providers
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly AuthenticationState _anonymousState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        public JwtAuthStateProvider(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenProvider.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymousState;
            }

            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ServiceUtils.ParseClaimsFromJwt(token), "jwt")));
            return authState;
        }

        public void MarkUserAsAuthenticated(IEnumerable<Claim> claims)
        {
            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth")));
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public void MarkUserAsLoggedOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
        }
    }
}
