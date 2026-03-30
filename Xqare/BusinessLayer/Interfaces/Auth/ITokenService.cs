namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface ITokenService
    {
        Task SetTokenAsync(string accessToken);
        Task<string> GetAccessTokenAsync();
        Task ClearAsync();
    }
}
