using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xqare.BusinessLayer.Interfaces.Auth;

namespace Xqare.BusinessLayer.Classes.Auth
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;

        public JwtAuthStateProvider(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return Anonymous();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // UI-only expiry check. The API remains authoritative for signature and claims validation.
                if (jwt.ValidTo <= DateTime.UtcNow.AddSeconds(15))
                {
                    await _tokenService.ClearAsync();
                    return Anonymous();
                }

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt")));
            }
            catch
            {
                await _tokenService.ClearAsync();
                return Anonymous();
            }
        }

        private static AuthenticationState Anonymous()
            => new(new ClaimsPrincipal(new ClaimsIdentity()));

        public void NotifyUserAuthentication()
            => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        public void NotifyUserLogout()
            => NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
    }
}
