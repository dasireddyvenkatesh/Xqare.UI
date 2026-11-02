using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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

        private readonly object _initializeLock = new();
        private Task? _initializationTask;

        // Concurrent callers share one active restore attempt. Once it finishes,
        // a later protected-route visit can try again (for example after token expiry).
        public Task InitializeAsync()
        {
            lock (_initializeLock)
            {
                if (_initializationTask is { IsCompleted: false })
                    return _initializationTask;

                return _initializationTask = InitializeCoreAsync();
            }
        }

        private async Task InitializeCoreAsync()
        {
            try
            {
                // A still-valid access token already exists in this tab's session storage.
                // Use it immediately; do not make every application startup wait for a
                // refresh-token API round-trip. The API remains authoritative and the
                // normal 401 handling path can refresh an expired/revoked token later.
                var isAuthenticated = (await _authStateProvider.GetAuthenticationStateAsync())
                    .User.Identity?.IsAuthenticated == true;

                if (isAuthenticated)
                    return;

                // Use ApiClient's shared refresh lock. This is the same operation
                // used when an API request receives 401, so startup and request
                // recovery cannot rotate the refresh cookie concurrently.
                var restored = await _api.RestoreSessionAsync();

                if (restored)
                    return;

                // Missing/invalid/expired refresh cookies should fail closed.
                await _tokenService.ClearAsync();
                if (_authStateProvider is JwtAuthStateProvider logoutProvider)
                    logoutProvider.NotifyUserLogout();
            }
            finally
            {
                lock (_initializeLock)
                {
                    _initializationTask = null;
                }
            }
        }

        public async Task<OtpOperationResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await _api.PostAsync<ForgotPasswordRequest, OtpOperationResponse>(
                "api/auth/forgot-password", request);

            if (response.IsSuccess && response.Data != null)
                return response.Data;

            return new OtpOperationResponse
            {
                IsSuccess = false,
                Message = response.Data?.Message ?? ExtractBackendMessage(response.ErrorMessage)
                    ?? "The request could not be completed. Please try again."
            };
        }

        public async Task<OtpOperationResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _api.PostAsync<ResetPasswordRequest, OtpOperationResponse>(
                "api/auth/reset-password", request);

            if (response.IsSuccess && response.Data != null)
            {
                if (response.Data.IsSuccess)
                {
                    await _tokenService.ClearAsync();
                    if (_authStateProvider is JwtAuthStateProvider jwt)
                        jwt.NotifyUserLogout();
                }

                return response.Data;
            }

            return new OtpOperationResponse
            {
                IsSuccess = false,
                Message = response.Data?.Message ?? ExtractBackendMessage(response.ErrorMessage)
                    ?? "The request could not be completed. Please try again."
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {

            request.DeviceId = await _device.GetDeviceIdAsync();
            request.DeviceName = await _device.GetDeviceNameAsync();

            var response = await _api.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);

            if (!response.IsSuccess || response.Data == null)
                return new LoginResponse { IsSuccess = false, Message = response.ErrorMessage ?? "The request could not be completed. Please try again." };

            if (!response.Data.IsSuccess)
                return response.Data;

            // Successful credential validation may still require email verification or MFA.
            // Never persist an absent token or treat a verification-pending response as authenticated.
            if (response.Data.RequiresEmailVerification ||
                response.Data.RequiresMfa ||
                string.IsNullOrWhiteSpace(response.Data.AccessToken))
            {
                return response.Data;
            }

            await ApplySessionAsync(response.Data);
            return response.Data;

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {

            request.DeviceId = await _device.GetDeviceIdAsync();
            request.DeviceName = await _device.GetDeviceNameAsync();

            var response = await _api.PostAsync<RegisterRequest, RegisterResponse>("api/auth/register", request);

            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new RegisterResponse { IsSuccess = false, Message = response.ErrorMessage ?? "The request could not be completed. Please try again." };

            return response.Data;

        }

        public async Task<OtpOperationResponse> ResendOtpAsync(string email)
        {
            var response = await _api.PostAsync<EmailOtpRequest, OtpOperationResponse>(
                "api/auth/resend-email-otp", new EmailOtpRequest { Email = email });

            if (response.IsSuccess && response.Data != null)
                return response.Data;

            return new OtpOperationResponse
            {
                IsSuccess = false,
                Message = response.Data?.Message ?? ExtractBackendMessage(response.ErrorMessage)
                    ?? "The request could not be completed. Please try again."
            };
        }

        private static string? ExtractBackendMessage(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return null;

            try
            {
                using var document = JsonDocument.Parse(payload);
                var root = document.RootElement;

                foreach (var name in new[] { "message", "Message", "title", "Title" })
                {
                    if (root.ValueKind == JsonValueKind.Object &&
                        root.TryGetProperty(name, out var value) &&
                        value.ValueKind == JsonValueKind.String &&
                        !string.IsNullOrWhiteSpace(value.GetString()))
                    {
                        return value.GetString();
                    }
                }
            }
            catch (JsonException)
            {
                // The response was not a structured API error; do not surface raw payloads.
            }

            return null;
        }

        public Task<LoginResponse> VerifyRegistrationOtpAsync(VerifyEmailOtpRequest request)
            => VerifyEmailOtpCoreAsync("api/auth/verify-register-email-otp", request);

        public Task<LoginResponse> VerifyEmailOtpAsync(VerifyEmailOtpRequest request)
            => VerifyEmailOtpCoreAsync("api/auth/verify-email-otp", request);

        private async Task<LoginResponse> VerifyEmailOtpCoreAsync(string endpoint, VerifyEmailOtpRequest request)
        {
            request.Email = request.Email?.Trim() ?? string.Empty;
            request.DeviceId = await _device.GetDeviceIdAsync();
            request.DeviceName = await _device.GetDeviceNameAsync();

            var response = await _api.PostAsync<VerifyEmailOtpRequest, LoginResponse>(endpoint, request);
            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new LoginResponse { IsSuccess = false, Message = response.ErrorMessage ?? "The request could not be completed. Please try again." };

            await ApplySessionAsync(response.Data);
            return response.Data;
        }

        public async Task<LoginResponse> VerifyMfaOtpAsync(VerifyMfaOtpRequest request)
        {
            request.DeviceId = await _device.GetDeviceIdAsync();
            request.DeviceName = await _device.GetDeviceNameAsync();

            var response = await _api.PostAsync<VerifyMfaOtpRequest, LoginResponse>("api/auth/verify-mfa-otp", request);
            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new LoginResponse { IsSuccess = false, Message = response.ErrorMessage ?? "The request could not be completed. Please try again." };

            await ApplySessionAsync(response.Data);
            return response.Data;
        }

        public async Task<OtpOperationResponse> ResendMfaOtpAsync(Guid userId)
        {
            var response = await _api.PostAsync<MfaOtpRequest, OtpOperationResponse>(
                "api/auth/request-mfa-otp", new MfaOtpRequest { Id = userId });

            if (response.IsSuccess && response.Data != null)
                return response.Data;

            return new OtpOperationResponse
            {
                IsSuccess = false,
                Message = response.Data?.Message ?? ExtractBackendMessage(response.ErrorMessage)
                    ?? "The request could not be completed. Please try again."
            };
        }

        public async Task<LoginResponse> VerifyMfaChallengeAsync(VerifyMfaChallengeRequest request)
        {
            var response = await _api.PostAsync<VerifyMfaChallengeRequest, LoginResponse>(
                "api/auth/verify-mfa-challenge", request);
            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new LoginResponse
                {
                    IsSuccess = false,
                    Message = response.ErrorMessage ?? "The request could not be completed. Please try again."
                };
            await ApplySessionAsync(response.Data);
            return response.Data;
        }

        public async Task<TotpStatusResponse> GetTotpStatusAsync()
        {
            var response = await _api.GetAsync<TotpStatusResponse>("api/auth/totp/status");
            return response.Data ?? new TotpStatusResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to retrieve authenticator status."
            };
        }

        public async Task<TotpEnrollmentStartResponse> StartTotpEnrollmentAsync(TotpEnrollmentStartRequest request)
        {
            var response = await _api.PostAsync<TotpEnrollmentStartRequest, TotpEnrollmentStartResponse>(
                "api/auth/totp/enrollment/start", request);
            return response.Data ?? new TotpEnrollmentStartResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to start authenticator setup."
            };
        }

        public async Task<TotpEnrollmentConfirmResponse> ConfirmTotpEnrollmentAsync(TotpEnrollmentConfirmRequest request)
        {
            var response = await _api.PostAsync<TotpEnrollmentConfirmRequest, TotpEnrollmentConfirmResponse>(
                "api/auth/totp/enrollment/confirm", request);
            return response.Data ?? new TotpEnrollmentConfirmResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to verify the authenticator code."
            };
        }

        public async Task<TotpEnrollmentConfirmResponse> RegenerateTotpRecoveryCodesAsync(TotpRecoveryCodesRegenerateRequest request)
        {
            var response = await _api.PostAsync<TotpRecoveryCodesRegenerateRequest, TotpEnrollmentConfirmResponse>(
                "api/auth/totp/recovery-codes/regenerate", request);
            return response.Data ?? new TotpEnrollmentConfirmResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to regenerate recovery codes."
            };
        }

        public async Task<OtpOperationResponse> DisableTotpAsync(TotpDisableRequest request)
        {
            var response = await _api.PostAsync<TotpDisableRequest, OtpOperationResponse>(
                "api/auth/totp/disable", request);
            return response.Data ?? new OtpOperationResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to disable the authenticator."
            };
        }

        private async Task ApplySessionAsync(LoginResponse response)
        {
            if (!response.IsSuccess || string.IsNullOrWhiteSpace(response.AccessToken) ||
                response.RequiresEmailVerification || response.RequiresMfa)
            {
                return;
            }

            await _tokenService.SetTokenAsync(response.AccessToken);
            if (_authStateProvider is JwtAuthStateProvider jwt)
                jwt.NotifyUserAuthentication();
        }

        public async Task Logout()
        {
            try
            {
                await _api.CookieCredential<string>("api/auth/logout");
            }
            finally
            {
                await _tokenService.ClearAsync();
                if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
                    jwtAuthStateProvider.NotifyUserLogout();

                _nav.NavigateTo("/", false);
            }
        }

        public async Task LoginWithGoogle()
        {
            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/google-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task LoginWithGitHub()
        {
            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/github-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task LoginWithLinkedIn()
        {
            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/linkedin-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task LoginWithTwitter()
        {
            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/twitter-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }


    }
}
