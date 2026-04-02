using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Common
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url);

        Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data);

        Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data);

        Task<ApiResponse<bool>> DeleteAsync(string url);

        Task CookieCredential<TResponse>(string url);
    }
}
