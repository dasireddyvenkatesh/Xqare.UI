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
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(jwt.Claims, "jwt");

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }


        public void NotifyUserAuthentication()
            => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        public void NotifyUserLogout()
            => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
