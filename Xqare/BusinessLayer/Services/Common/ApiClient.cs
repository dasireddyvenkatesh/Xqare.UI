using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using MudBlazor;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Interfaces.Auth;
using Xqare.BusinessLayer.Interfaces.Common;

namespace Xqare.BusinessLayer.Services.Common
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;
        private readonly NavigationManager _nav;
        private readonly ISnackbar Snackbar;
        private bool _isRefreshing;

        public ApiClient(HttpClient http, ITokenService tokenService, NavigationManager nav, ISnackbar snackbar)
        {
            _http = http;
            _tokenService = tokenService;
            _nav = nav;
            Snackbar = snackbar;
        }

        private async Task AttachTokenAsync()
        {
            _http.DefaultRequestHeaders.Authorization = null;

            var token = await _tokenService.GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            if (_isRefreshing) return false;
            _isRefreshing = true;

            try
            {

                var response = await _http.SendAsync(CreateRequest(HttpMethod.Post, "api/auth/refresh-token", new StringContent("")));

                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content
                    .ReadFromJsonAsync<LoginResponse>(
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.AccessToken == null) return false;

                await _tokenService.SetTokenAsync(result.AccessToken);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private async Task<ApiResponse<T>> SendAsync<T>(Func<Task<HttpResponseMessage>> httpCall)
        {
            try
            {

                await AttachTokenAsync();

                var response = await httpCall();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {

                    var refreshed = await TryRefreshTokenAsync();

                    if (!refreshed)
                    {
                        await _tokenService.ClearAsync();
                        return new ApiResponse<T>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.Unauthorized,
                            ErrorMessage = "Session expired"
                        };
                    }

                    await AttachTokenAsync();
                    response = await httpCall();
                }

                string json = await response.Content.ReadAsStringAsync();

                var result = new ApiResponse<T>
                {
                    StatusCode = response.StatusCode,
                    RawJson = json,
                    IsSuccess = response.IsSuccessStatusCode,
                };

                if (!response.IsSuccessStatusCode)
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
            catch (Exception ex)
            {
                Snackbar.Add($"API call failed: {ex.Message}", Severity.Error);

                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url, HttpContent? content = null)
        {
            var request = new HttpRequestMessage(method, url);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            if (content != null) request.Content = content;
            return request;
        }

        public Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url)
            => SendAsync<TResponse>(() => _http.GetAsync(url));

        public Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(() => _http.PostAsJsonAsync(url, data));

        public Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data)
            => SendAsync<TResponse>(() => _http.PutAsJsonAsync(url, data));

        public Task<ApiResponse<bool>> DeleteAsync(string url)
            => SendAsync<bool>(() => _http.DeleteAsync(url));

        public Task<ApiResponse<TResponse>> CookieCredential<TResponse>(string url)
            => SendAsync<TResponse>(() => _http.SendAsync(CreateRequest(HttpMethod.Post, url,
                new StringContent(""))));
    }
}
