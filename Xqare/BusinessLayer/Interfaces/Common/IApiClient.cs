using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Common
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url);

        Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data);

        Task<ApiResponse<TResponse>> PostAnonymousAsync<TRequest, TResponse>(string url, TRequest data);

        Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data);

        Task<ApiResponse<bool>> DeleteAsync(string url);

        Task<ApiResponse<TResponse>> CookieCredential<TResponse>(string url);

        // Shares the same serialized refresh flow used by requests that receive 401.
        Task<SessionRestoreOutcome> RestoreSessionAsync();
    }
}
