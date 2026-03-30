using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Interfaces.Auth;
using Xqare.BusinessLayer.Interfaces.Common;

namespace Xqare.BusinessLayer.Classes.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IApiClient _api;
        private readonly IDeviceService _device;
        private readonly ITokenService _tokenService;
        private readonly NavigationManager _nav;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IConfiguration _configuration;

        public AuthService(IDeviceService device, IApiClient api, ITokenService tokenService,
                            NavigationManager nav, AuthenticationStateProvider authStateProvider,
                            IConfiguration configuration)
        {
            _device = device;
            _api = api;
            _tokenService = tokenService;
            _nav = nav;
            _authStateProvider = authStateProvider;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {

            var response = await _api.PostAsync<object, RefreshTokenResponse>(
                "api/auth/refresh-token", default!);

            if (response.IsSuccess && response.Data != null)
            {
                await _tokenService.SetTokenAsync(response.Data.AccessToken);

                if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
                {
                    jwtAuthStateProvider.NotifyUserAuthentication();
                }
            }
            else
            {
                await _tokenService.ClearAsync();

                if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
                    jwtAuthStateProvider.NotifyUserLogout();
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {

            request.DeviceId = await _device.GetDeviceIdAsync();
            request.DeviceName = await _device.GetDeviceNameAsync();

            var response = await _api.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);

            if (!response.IsSuccess || response.Data == null)
                return response.Data!;

            await _tokenService.SetTokenAsync(response.Data.AccessToken);

            if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
            {
                jwtAuthStateProvider.NotifyUserAuthentication();
            }

            return response.Data;

        }

        public async Task<bool> ResendOtpAsync(string email)
        {
            var response = await _api.PostAsync<string, bool>("api/auth/resend-otp", email);

            return true;
        }

        public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
        {
            //var response = await _http.PostAsJsonAsync("api/auth/verify-otp", request);
            return true;
        }

        public async Task Logout()
        {
            string sessionId = string.Empty;
            await _tokenService.ClearAsync();
            await _api.PostAsync<string, bool>("api/auth/logout", sessionId);
            if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
            {
                jwtAuthStateProvider.NotifyUserLogout();
            }

            var returnUrl = Uri.EscapeDataString(_nav.Uri);

            _nav.NavigateTo($"/login?returnUrl={returnUrl}", false);
        }

        public async Task LoginWithGoogle()
        {
            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/google-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task<(bool success, bool requiresMfa)> HandleExternalLoginCallbackAsync(string url)
        {
            var uri = _nav.ToAbsoluteUri(url);
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (!query.ContainsKey("data"))
                return (false, false);

            var json = Uri.UnescapeDataString(query["data"]!);

            var response = JsonSerializer.Deserialize<LoginResponse>(json);

            if (response == null || !response.IsSuccess)
                return (false, false);

            if (!string.IsNullOrEmpty(response.AccessToken))
            {
                await _tokenService.SetTokenAsync(response.AccessToken);

                if (_authStateProvider is JwtAuthStateProvider jwt)
                    jwt.NotifyUserAuthentication();

                _nav.NavigateTo("/dashboard", true);
            }

            return (true, response.RequiresMfa);
        }
    }
}
