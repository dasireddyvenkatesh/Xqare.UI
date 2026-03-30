using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface IAuthService
    {
        Task InitializeAsync();
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> VerifyOtpAsync(VerifyOtpRequest request);
        Task<bool> ResendOtpAsync(string email);
        Task LoginWithGoogle();
        Task<(bool success, bool requiresMfa)> HandleExternalLoginCallbackAsync(string url);
        Task Logout();
    }
}
