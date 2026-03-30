namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface ITokenService
    {
        Task<string?> GetAccessTokenAsync();

        Task SetTokenAsync(string accessToken);

        Task ClearAsync();
    }
}
