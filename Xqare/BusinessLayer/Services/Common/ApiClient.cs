using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using Xqare.BusinessLayer.Classes.Auth;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Interfaces.Auth;
using Xqare.BusinessLayer.Interfaces.Common;

namespace Xqare.BusinessLayer.Services.Common
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;
        private readonly JwtAuthStateProvider _authStateProvider;
        private readonly IJSRuntime _js;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        private volatile bool _refreshSessionRejected;

        public ApiClient(HttpClient http, ITokenService tokenService, JwtAuthStateProvider authStateProvider, IJSRuntime js)
        {
            _http = http;
            _tokenService = tokenService;
            _authStateProvider = authStateProvider;
            _js = js;
        }

        private async Task<string?> AttachTokenAsync(HttpRequestMessage request)
        {
            var token = await _tokenService.GetAccessTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return token;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url, HttpContent? content = null)
        {
            var request = new HttpRequestMessage(method, url);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            if (content != null)
                request.Content = content;
            return request;
        }

        private async Task<SessionRestoreOutcome> TryRefreshTokenAsync(string? accessTokenUsed)
        {
            // Web Locks serialize refresh-token rotation across tabs on this UI origin.
            // The access token remains tab-scoped in sessionStorage; each tab obtains
            // its own access token after it acquires the lock.
            using var dotNetReference = DotNetObjectReference.Create(this);

            try
            {
                var outcome = await _js.InvokeAsync<string>(
                    "sessionRefreshCoordinator.runExclusive",
                    dotNetReference,
                    nameof(RefreshTokenUnderBrowserLockAsync),
                    accessTokenUsed);

                return Enum.TryParse<SessionRestoreOutcome>(outcome, out var parsedOutcome)
                    ? parsedOutcome
                    : SessionRestoreOutcome.Unavailable;
            }
            catch (JSException)
            {
                // A failed browser-coordination call is not proof that the refresh
                // session is invalid. Leave the session intact and allow a retry.
                return SessionRestoreOutcome.Unavailable;
            }
            catch (OperationCanceledException)
            {
                return SessionRestoreOutcome.Unavailable;
            }
        }

        [JSInvokable]
        public async Task<string> RefreshTokenUnderBrowserLockAsync(string? accessTokenUsed)
            => (await TryRefreshTokenCoreAsync(accessTokenUsed)).ToString();

        private async Task<SessionRestoreOutcome> TryRefreshTokenCoreAsync(string? accessTokenUsed)
        {
            await _refreshLock.WaitAsync();
            try
            {
                // A rejected refresh cookie remains rejected for this client until
                // a successful sign-in explicitly resets this state. This stops
                // concurrent 401 responses from repeatedly posting refresh-token.
                if (_refreshSessionRejected)
                    return SessionRestoreOutcome.Rejected;

                // A different request may have refreshed the session while this
                // request was waiting. Reuse that token instead of rotating the
                // refresh cookie again.
                var currentToken = await _tokenService.GetAccessTokenAsync();
                var currentState = await _authStateProvider.GetAuthenticationStateAsync();
                var hasValidCurrentToken = currentState.User.Identity?.IsAuthenticated == true;

                if (hasValidCurrentToken &&
                    (string.IsNullOrWhiteSpace(accessTokenUsed) ||
                     !string.Equals(currentToken, accessTokenUsed, StringComparison.Ordinal)))
                {
                    return SessionRestoreOutcome.Restored;
                }

                using var request = CreateRequest(
                    HttpMethod.Post,
                    "api/auth/refresh-token",
                    new StringContent(string.Empty));

                // Bound session restoration so the protected-route UI can offer a
                // retry. A timeout/network failure is not proof that the session is invalid.
                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(12));
                using var response = await _http.SendAsync(request, timeout.Token);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _refreshSessionRejected = true;
                    return SessionRestoreOutcome.Rejected;
                }

                if (!response.IsSuccessStatusCode)
                    return SessionRestoreOutcome.Unavailable;

                var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                    timeout.Token);

                if (result == null || !result.IsSuccess || string.IsNullOrWhiteSpace(result.AccessToken))
                    return SessionRestoreOutcome.Unavailable;

                _refreshSessionRejected = false;
                await _tokenService.SetTokenAsync(result.AccessToken);
                _authStateProvider.NotifyUserAuthentication();
                return SessionRestoreOutcome.Restored;
            }
            catch (OperationCanceledException)
            {
                return SessionRestoreOutcome.Unavailable;
            }
            catch (HttpRequestException)
            {
                return SessionRestoreOutcome.Unavailable;
            }
            catch
            {
                return SessionRestoreOutcome.Unavailable;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        // Startup session restoration and 401 recovery use this exact same
        // serialized refresh operation.
        public Task<SessionRestoreOutcome> RestoreSessionAsync()
            => TryRefreshTokenAsync(accessTokenUsed: null);

        public void ResetRefreshState()
        {
            _refreshSessionRejected = false;
        }


        private async Task<ApiResponse<T>> SendAsync<T>(
            Func<HttpRequestMessage> requestFactory,
            bool retryAfterRefresh = true,
            bool attachAccessToken = true)
        {
            try
            {
                // After an explicit refresh rejection, do not send further protected
                // requests until a successful sign-in resets the client state.
                if (attachAccessToken && _refreshSessionRejected)
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.Unauthorized,
                        ErrorMessage = "Session expired. Please sign in again."
                    };
                }

                using var request = requestFactory();
                string? accessTokenUsed = null;
                if (attachAccessToken)
                    accessTokenUsed = await AttachTokenAsync(request);
                using var response = await _http.SendAsync(request);

                HttpResponseMessage responseToRead = response;
                HttpResponseMessage? retryResponse = null;

                // A missing bearer token means there is no authenticated request
                // to recover. Startup restoration is handled explicitly by the
                // protected-route restoration component; do not create a refresh
                // request for every anonymous endpoint call.
                if (response.StatusCode == HttpStatusCode.Unauthorized &&
                    retryAfterRefresh &&
                    !string.IsNullOrWhiteSpace(accessTokenUsed))
                {
                    var refreshOutcome = await TryRefreshTokenAsync(accessTokenUsed);
                    if (refreshOutcome == SessionRestoreOutcome.Rejected)
                    {
                        await _tokenService.ClearAsync();
                        _authStateProvider.NotifyUserLogout();
                        return new ApiResponse<T>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.Unauthorized,
                            ErrorMessage = "Session expired. Please sign in again."
                        };
                    }

                    if (refreshOutcome == SessionRestoreOutcome.Unavailable)
                    {
                        return new ApiResponse<T>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.ServiceUnavailable,
                            ErrorMessage = "Session validation is temporarily unavailable. Check your connection and retry."
                        };
                    }

                    using var retryRequest = requestFactory();
                    if (attachAccessToken)
                        await AttachTokenAsync(retryRequest);
                    retryResponse = await _http.SendAsync(retryRequest);
                    responseToRead = retryResponse;

                    if (retryResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await _tokenService.ClearAsync();
                        _authStateProvider.NotifyUserLogout();
                    }
                }

                using (retryResponse)
                {
                    var json = await responseToRead.Content.ReadAsStringAsync();
                    var result = new ApiResponse<T>
                    {
                        StatusCode = responseToRead.StatusCode,
                        RawJson = json,
                        IsSuccess = responseToRead.IsSuccessStatusCode
                    };

                    if (!responseToRead.IsSuccessStatusCode)
                    {
                        result.ErrorMessage = json;
                        return result;
                    }

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        result.Data = JsonSerializer.Deserialize<T>(
                            json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? default!;
                    }

                    return result;
                }
            }
            catch
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = "The request could not be completed. Please try again."
                };
            }
        }

        public Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url)
            => SendAsync<TResponse>(() => CreateRequest(HttpMethod.Get, url));

        public Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(() => CreateRequest(HttpMethod.Post, url, JsonContent.Create(data)));

        // Anonymous challenge requests must not attach a stale bearer token or
        // recursively attempt refresh when the short-lived challenge is rejected.
        public Task<ApiResponse<TResponse>> PostAnonymousAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(
                () => CreateRequest(HttpMethod.Post, url, JsonContent.Create(data)),
                retryAfterRefresh: false,
                attachAccessToken: false);

        public Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(() => CreateRequest(HttpMethod.Put, url, JsonContent.Create(data)));

        public Task<ApiResponse<bool>> DeleteAsync(string url)
            => SendAsync<bool>(() => CreateRequest(HttpMethod.Delete, url));

        public Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url)
            => SendAsync<TResponse>(() => CreateRequest(HttpMethod.Delete, url));

        public Task<ApiResponse<TResponse>> DeleteAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(() => CreateRequest(HttpMethod.Delete, url, JsonContent.Create(data)));

        // Refresh and logout must not recursively trigger another refresh request.
        public Task<ApiResponse<TResponse>> CookieCredential<TResponse>(string url)
            => SendAsync<TResponse>(
                () => CreateRequest(HttpMethod.Post, url, new StringContent(string.Empty)),
                retryAfterRefresh: false,
                attachAccessToken: false);
    }
}
