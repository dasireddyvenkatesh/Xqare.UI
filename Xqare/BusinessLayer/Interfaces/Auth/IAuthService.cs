using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface IAuthService
    {
        Task InitializeAsync();
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> VerifyRegistrationOtpAsync(VerifyEmailOtpRequest request);
        Task<LoginResponse> VerifyEmailOtpAsync(VerifyEmailOtpRequest request);
        Task<LoginResponse> VerifyMfaOtpAsync(VerifyMfaOtpRequest request);
        Task<OtpOperationResponse> ResendOtpAsync(string email);
        Task<OtpOperationResponse> ResendMfaOtpAsync(Guid userId);
        Task LoginWithGoogle();
        Task LoginWithGitHub();
        Task LoginWithLinkedIn();
        Task LoginWithTwitter();
        Task<(bool success, bool requiresMfa)> HandleExternalLoginCallbackAsync(string url);
        Task Logout();
    }
}
