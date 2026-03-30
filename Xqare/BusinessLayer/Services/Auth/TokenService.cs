using Xqare.BusinessLayer.Interfaces.Auth;

namespace Xqare.BusinessLayer.Classes.Auth
{
    public class TokenService : ITokenService
    {
        private string? _accessToken;

        public Task<string?> GetAccessTokenAsync()
            => Task.FromResult(_accessToken);

        public Task SetTokenAsync(string accessToken)
        {
            _accessToken = accessToken;
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            _accessToken = null;
            return Task.CompletedTask;
        }
    }
}
