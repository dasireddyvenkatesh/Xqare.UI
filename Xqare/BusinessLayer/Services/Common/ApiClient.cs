using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        public ApiClient(HttpClient http, ITokenService tokenService, JwtAuthStateProvider authStateProvider)
        {
            _http = http;
            _tokenService = tokenService;
            _authStateProvider = authStateProvider;
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

        private async Task<bool> TryRefreshTokenAsync(string? accessTokenUsed)
        {
            await _refreshLock.WaitAsync();
            try
            {
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
                    return true;
                }

                using var request = CreateRequest(
                    HttpMethod.Post,
                    "api/auth/refresh-token",
                    new StringContent(string.Empty));

                using var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null || !result.IsSuccess || string.IsNullOrWhiteSpace(result.AccessToken))
                    return false;

                await _tokenService.SetTokenAsync(result.AccessToken);
                _authStateProvider.NotifyUserAuthentication();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        // Startup session restoration and 401 recovery use this exact same
        // serialized refresh operation.
        public Task<bool> RestoreSessionAsync()
            => TryRefreshTokenAsync(accessTokenUsed: null);

        private async Task<ApiResponse<T>> SendAsync<T>(
            Func<HttpRequestMessage> requestFactory,
            bool retryAfterRefresh = true,
            bool attachAccessToken = true)
        {
            try
            {
                using var request = requestFactory();
                string? accessTokenUsed = null;
                if (attachAccessToken)
                    accessTokenUsed = await AttachTokenAsync(request);
                using var response = await _http.SendAsync(request);

                HttpResponseMessage responseToRead = response;
                HttpResponseMessage? retryResponse = null;

                if (response.StatusCode == HttpStatusCode.Unauthorized && retryAfterRefresh)
                {
                    var refreshed = await TryRefreshTokenAsync(accessTokenUsed);
                    if (!refreshed)
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

        public Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(() => CreateRequest(HttpMethod.Put, url, JsonContent.Create(data)));

        public Task<ApiResponse<bool>> DeleteAsync(string url)
            => SendAsync<bool>(() => CreateRequest(HttpMethod.Delete, url));

        // Refresh and logout must not recursively trigger another refresh request.
        public Task<ApiResponse<TResponse>> CookieCredential<TResponse>(string url)
            => SendAsync<TResponse>(
                () => CreateRequest(HttpMethod.Post, url, new StringContent(string.Empty)),
                retryAfterRefresh: false,
                attachAccessToken: false);
    }
}
