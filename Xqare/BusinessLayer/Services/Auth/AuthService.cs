using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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
        private readonly IJSRuntime _js;
        private DotNetObjectReference<AuthService>? _logoutListenerReference;
        private bool _logoutListenerRegistered;

        public AuthService(IDeviceService device, IApiClient api, ITokenService tokenService,
                            NavigationManager nav, AuthenticationStateProvider authStateProvider,
                            IConfiguration configuration, IJSRuntime js)
        {
            _device = device;
            _api = api;
            _tokenService = tokenService;
            _nav = nav;
            _authStateProvider = authStateProvider;
            _configuration = configuration;
            _js = js;
        }

        private readonly object _initializeLock = new();
        private Task<SessionRestoreOutcome>? _initializationTask;

        // Concurrent callers share one active restore attempt. Once it finishes,
        // a later protected-route visit can try again (for example after token expiry).
        public Task<SessionRestoreOutcome> InitializeAsync()
        {
            lock (_initializeLock)
            {
                if (_initializationTask is { IsCompleted: false })
                    return _initializationTask;

                return _initializationTask = InitializeCoreAsync();
            }
        }

        private async Task<SessionRestoreOutcome> InitializeCoreAsync()
        {
            try
            {
                await EnsureCrossTabLogoutListenerAsync();
                // A still-valid access token already exists in this tab's session storage.
                // Use it immediately; do not make every application startup wait for a
                // refresh-token API round-trip. The API remains authoritative and the
                // normal 401 handling path can refresh an expired/revoked token later.
                var isAuthenticated = (await _authStateProvider.GetAuthenticationStateAsync())
                    .User.Identity?.IsAuthenticated == true;

                if (isAuthenticated)
                    return SessionRestoreOutcome.Restored;

                // Use ApiClient's shared refresh lock. This is the same operation
                // used when an API request receives 401, so startup and request
                // recovery cannot rotate the refresh cookie concurrently.
                var restoreOutcome = await _api.RestoreSessionAsync();

                // Only an explicit 401 from the refresh endpoint proves that the
                // refresh session is missing, expired or revoked. Network failures,
                // timeouts and 5xx responses remain retryable and must not erase tokens.
                if (restoreOutcome == SessionRestoreOutcome.Rejected)
                {
                    await _tokenService.ClearAsync();
                    if (_authStateProvider is JwtAuthStateProvider logoutProvider)
                        logoutProvider.NotifyUserLogout();
                }

                return restoreOutcome;
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
            var response = await _api.PostAnonymousAsync<ForgotPasswordRequest, OtpOperationResponse>(
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
            var response = await _api.PostAnonymousAsync<ResetPasswordRequest, OtpOperationResponse>(
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

            var response = await _api.PostAnonymousAsync<LoginRequest, LoginResponse>("api/auth/login", request);

            if (!response.IsSuccess || response.Data == null)
                return new LoginResponse { IsSuccess = false, Message = response.Data?.Message ?? ExtractBackendMessage(response.ErrorMessage) ?? "The request could not be completed. Please try again." };

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

            var response = await _api.PostAnonymousAsync<RegisterRequest, RegisterResponse>("api/auth/register", request);

            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new RegisterResponse { IsSuccess = false, Message = ExtractBackendMessage(response.ErrorMessage) ?? "The request could not be completed. Please try again." };

            return response.Data;

        }

        public async Task<OtpOperationResponse> ResendOtpAsync(string email)
        {
            var response = await _api.PostAnonymousAsync<EmailOtpRequest, OtpOperationResponse>(
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

        public Task<LoginResponse> VerifyRegistrationOtpAsync(VerifyRegisterEmailOtpRequest request)
            => VerifyEmailOtpCoreAsync("api/auth/verify-register-email-otp", request);

        private async Task<LoginResponse> VerifyEmailOtpCoreAsync<TRequest>(string endpoint, TRequest request)
            where TRequest : VerifyEmailOtpRequest
        {
            request.Email = request.Email?.Trim() ?? string.Empty;
            request.DeviceId = await _device.GetDeviceIdAsync();
            request.DeviceName = await _device.GetDeviceNameAsync();

            var response = await _api.PostAnonymousAsync<TRequest, LoginResponse>(endpoint, request);
            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new LoginResponse { IsSuccess = false, Message = ExtractBackendMessage(response.ErrorMessage) ?? "The request could not be completed. Please try again." };

            await ApplySessionAsync(response.Data);
            return response.Data;
        }

        public async Task<LoginResponse> VerifyMfaChallengeAsync(VerifyMfaChallengeRequest request)
        {
            var response = await _api.PostAnonymousAsync<VerifyMfaChallengeRequest, LoginResponse>(
                "api/auth/verify-mfa-challenge", request);
            if (!response.IsSuccess || response.Data == null)
                return response.Data ?? new LoginResponse
                {
                    IsSuccess = false,
                    Message = ExtractBackendMessage(response.ErrorMessage)
                        ?? "MFA verification could not be completed. Please sign in again and retry."
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

        public async Task<ConnectedAccountsResponse> GetConnectedAccountsAsync()
        {
            var response = await _api.GetAsync<ConnectedAccountsResponse>("api/auth/connected-accounts");
            return response.Data ?? new ConnectedAccountsResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to retrieve connected sign-in methods."
            };
        }

        public async Task<OtpOperationResponse> ConnectExternalAccountAsync(string provider)
        {
            var route = provider?.Trim().ToLowerInvariant() switch
            {
                "google" => "google",
                "github" => "github",
                "linkedin" => "linkedin",
                "twitter" => "twitter",
                _ => null
            };

            if (route == null)
                return new OtpOperationResponse { IsSuccess = false, Message = "This sign-in provider is not supported." };

            var response = await _api.PostAsync<object, OtpOperationResponse>(
                $"api/auth/external-links/{route}/begin", new { });

            if (!response.IsSuccess || response.Data == null)
            {
                return new OtpOperationResponse
                {
                    IsSuccess = false,
                    Message = response.Data?.Message ?? ExtractBackendMessage(response.ErrorMessage)
                        ?? "Unable to start the account-linking process."
                };
            }

            if (!response.Data.IsSuccess)
                return response.Data;

            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.TrimEnd('/');
            if (string.IsNullOrWhiteSpace(apiBaseUrl))
            {
                return new OtpOperationResponse
                {
                    IsSuccess = false,
                    Message = "The authentication service URL is not configured."
                };
            }

            _nav.NavigateTo($"{apiBaseUrl}/api/auth/{route}-link", forceLoad: true);
            return response.Data;
        }
        public async Task<OtpOperationResponse> DisconnectExternalAccountAsync(
            string provider, DisconnectExternalLoginRequest request)
        {
            var route = provider?.Trim().ToLowerInvariant() switch
            {
                "google" => "google",
                "github" => "github",
                "linkedin" => "linkedin",
                "twitter" => "twitter",
                _ => null
            };

            if (route == null)
                return new OtpOperationResponse { IsSuccess = false, Message = "This sign-in provider is not supported." };

            var response = await _api.PostAsync<DisconnectExternalLoginRequest, OtpOperationResponse>(
                $"api/auth/external-links/{route}/disconnect", request);

            return response.Data ?? new OtpOperationResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to disconnect this provider."
            };
        }

        public async Task<OtpOperationResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = await _api.PostAsync<ChangePasswordRequest, OtpOperationResponse>(
                "api/auth/change-password", request);

            return response.Data ?? new OtpOperationResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to change your password."
            };
        }

        public async Task<ActiveSessionsResponse> GetActiveSessionsAsync()
        {
            var response = await _api.GetAsync<ActiveSessionsResponse>("api/auth/sessions");
            return response.Data ?? new ActiveSessionsResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to retrieve active sessions."
            };
        }

        public async Task<SessionActionResponse> RevokeSessionAsync(Guid sessionId)
        {
            var response = await _api.PostAsync<object, SessionActionResponse>(
                $"api/auth/sessions/{sessionId:D}/revoke", new { });

            return response.Data ?? new SessionActionResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to revoke the selected session."
            };
        }

        public async Task<SessionActionResponse> RevokeOtherSessionsAsync()
        {
            var response = await _api.PostAsync<object, SessionActionResponse>(
                "api/auth/sessions/revoke-others", new { });

            return response.Data ?? new SessionActionResponse
            {
                IsSuccess = false,
                Message = ExtractBackendMessage(response.ErrorMessage) ?? "Unable to revoke other sessions."
            };
        }

        private async Task ApplySessionAsync(LoginResponse response)
        {
            if (!response.IsSuccess || string.IsNullOrWhiteSpace(response.AccessToken) ||
                response.RequiresEmailVerification || response.RequiresMfa)
            {
                return;
            }

            _api.ResetRefreshState();
            await _tokenService.SetTokenAsync(response.AccessToken);
            if (_authStateProvider is JwtAuthStateProvider jwt)
                jwt.NotifyUserAuthentication();

            // Other tabs must discard any pre-logout access token and restore
            // independently from the newly established HttpOnly refresh cookie.
            await BroadcastCrossTabSessionEstablishedAsync();
        }

        public async Task Logout()
        {
            try
            {
                _logoutListenerReference ??= DotNetObjectReference.Create(this);
                await _js.InvokeAsync<string>(
                    "sessionRefreshCoordinator.runExclusiveAction",
                    _logoutListenerReference,
                    nameof(LogoutUnderBrowserLockAsync));
            }
            catch (JSException)
            {
                // If browser coordination is unavailable, still complete local/server logout.
                await _api.CookieCredential<string>("api/auth/logout");
            }
            finally
            {
                await _tokenService.ClearAsync();
                if (_authStateProvider is JwtAuthStateProvider jwtAuthStateProvider)
                    jwtAuthStateProvider.NotifyUserLogout();

                await BroadcastCrossTabLogoutAsync();
                _nav.NavigateTo("/", false);
            }
        }

        [JSInvokable]
        public async Task<string> LogoutUnderBrowserLockAsync()
        {
            await _api.CookieCredential<string>("api/auth/logout");
            return "completed";
        }

        // Register cross-tab logout handling independently from session restoration.
        // Every tab must listen even when it already has a valid access token.
        public Task InitializeCrossTabSessionAsync()
            => EnsureCrossTabLogoutListenerAsync();

        private async Task EnsureCrossTabLogoutListenerAsync()
        {
            if (_logoutListenerRegistered)
                return;

            _logoutListenerReference ??= DotNetObjectReference.Create(this);
            try
            {
                await _js.InvokeVoidAsync(
                    "sessionRefreshCoordinator.registerLogoutListener",
                    _logoutListenerReference);
                _logoutListenerRegistered = true;
            }
            catch (JSException)
            {
                // Cross-tab notification is an enhancement; local authentication
                // remains functional if BroadcastChannel is unavailable.
            }
        }

        private async Task BroadcastCrossTabLogoutAsync()
        {
            try
            {
                await _js.InvokeVoidAsync("sessionRefreshCoordinator.broadcastLogout");
            }
            catch (JSException)
            {
                // Never block local logout on cross-tab notification support.
            }
        }

        private async Task BroadcastCrossTabSessionEstablishedAsync()
        {
            try
            {
                await _js.InvokeVoidAsync("sessionRefreshCoordinator.broadcastSessionEstablished");
            }
            catch (JSException)
            {
                // Each tab can still restore on its next protected-route visit.
            }
        }

        [JSInvokable]
        public async Task HandleCrossTabSessionEstablishedAsync()
        {
            // Never copy or broadcast access tokens between tabs. Discard this
            // tab's stale token and independently restore from the HttpOnly cookie.
            await _tokenService.ClearAsync();
            _api.ResetRefreshState();
            if (_authStateProvider is JwtAuthStateProvider jwt)
                jwt.NotifyUserLogout();

            try
            {
                var outcome = await InitializeAsync();
                if (outcome != SessionRestoreOutcome.Restored)
                    return;

                // A public Login page does not automatically redirect when the
                // authentication state changes. Navigate only after this tab has
                // independently restored and validated its own access token.
                var currentUri = _nav.ToAbsoluteUri(_nav.Uri);
                var currentPath = _nav.ToBaseRelativePath(currentUri).Split('?', '#')[0].Trim('/');
                if (string.Equals(currentPath, "login", StringComparison.OrdinalIgnoreCase))
                {
                    var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(currentUri.Query);
                    query.TryGetValue("returnUrl", out var returnUrlValue);
                    var target = LocalReturnUrlHelper.GetSafeReturnUrl(returnUrlValue.ToString());
                    _nav.NavigateTo(target, replace: true);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Cross-tab session restoration failed: {ex.Message}");
            }
        }

        [JSInvokable]
        public async Task HandleCrossTabLogoutAsync()
        {
            await _tokenService.ClearAsync();
            _api.ResetRefreshState();
            if (_authStateProvider is JwtAuthStateProvider jwt)
                jwt.NotifyUserLogout();
        }

        private async Task PrepareForExternalSignInAsync()
        {
            // Social login from an already-authenticated tab is an account switch,
            // not an account-link operation. Revoke the old refresh session and clear
            // its tab-scoped access token before the provider callback establishes a new identity.
            try
            {
                await _api.CookieCredential<string>("api/auth/logout");
            }
            catch
            {
                // Local state is cleared even if the previous refresh session is already unavailable.
            }

            _api.ResetRefreshState();
            await _tokenService.ClearAsync();
            if (_authStateProvider is JwtAuthStateProvider jwt)
                jwt.NotifyUserLogout();
        }

        public async Task LoginWithGoogle()
        {
            await PrepareForExternalSignInAsync();

            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/google-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task LoginWithGitHub()
        {
            await PrepareForExternalSignInAsync();

            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/github-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task LoginWithLinkedIn()
        {
            await PrepareForExternalSignInAsync();

            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/linkedin-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }

        public async Task LoginWithTwitter()
        {
            await PrepareForExternalSignInAsync();

            string deviceId = await _device.GetDeviceIdAsync();
            string deviceName = await _device.GetDeviceNameAsync();

            string url = $"{_configuration["ApiSettings:BaseUrl"]}/api/auth/twitter-login?deviceId={deviceId}&deviceName={deviceName}";

            _nav.NavigateTo(url, true);
        }


    }
}
