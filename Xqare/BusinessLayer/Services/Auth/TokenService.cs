using System.Net.Http.Json;
using Xqare.BusinessLayer.Interfaces.Auth;

namespace Xqare.BusinessLayer.Classes.Auth
{
    public class TokenService : ITokenService
    {

        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public TokenService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task SetTokenAsync(string accessToken)
        {

            await _http.PostAsJsonAsync($"{BaseUrl()}/api/auth/settoken", accessToken);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            //var response = await _http.GetAsync($"{BaseUrl()}/api/auth/gettoken");

            //if (response.IsSuccessStatusCode)
            //{
            //    return await response.Content.ReadAsStringAsync();
            //}

            return default!;
        }

        public async Task ClearAsync()
        {
            await _http.DeleteAsync($"{BaseUrl()}/api/auth/deletetoken");
        }
        private string BaseUrl()
        {
            return _configuration["ApiSettings:BaseUrl"] ?? default!;
        }
    }
}
