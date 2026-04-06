using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface IAuthService
    {
        Task InitializeAsync();
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<VerifyEmailOtpResponse> VerifyOtpAsync(VerifyEmailOtpRequest request);
        Task<bool> ResendOtpAsync(string email);
        Task LoginWithGoogle();
        Task LoginWithGitHub();
        Task LoginWithLinkedIn();
        Task LoginWithTwitter();
        Task<(bool success, bool requiresMfa)> HandleExternalLoginCallbackAsync(string url);
        Task Logout();
    }
}
