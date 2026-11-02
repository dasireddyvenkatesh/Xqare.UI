using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface IAuthService
    {
        Task InitializeAsync();
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<OtpOperationResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<OtpOperationResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> VerifyRegistrationOtpAsync(VerifyEmailOtpRequest request);
        Task<LoginResponse> VerifyEmailOtpAsync(VerifyEmailOtpRequest request);
        Task<LoginResponse> VerifyMfaOtpAsync(VerifyMfaOtpRequest request);
        Task<LoginResponse> VerifyMfaChallengeAsync(VerifyMfaChallengeRequest request);
        Task<TotpStatusResponse> GetTotpStatusAsync();
        Task<TotpEnrollmentStartResponse> StartTotpEnrollmentAsync(TotpEnrollmentStartRequest request);
        Task<TotpEnrollmentConfirmResponse> ConfirmTotpEnrollmentAsync(TotpEnrollmentConfirmRequest request);
        Task<TotpEnrollmentConfirmResponse> RegenerateTotpRecoveryCodesAsync(TotpRecoveryCodesRegenerateRequest request);
        Task<OtpOperationResponse> DisableTotpAsync(TotpDisableRequest request);
        Task<OtpOperationResponse> ResendOtpAsync(string email);
        Task<OtpOperationResponse> ResendMfaOtpAsync(Guid userId);
        Task LoginWithGoogle();
        Task LoginWithGitHub();
        Task LoginWithLinkedIn();
        Task LoginWithTwitter();
        Task Logout();
    }
}
