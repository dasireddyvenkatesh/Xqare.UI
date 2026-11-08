using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<SessionRestoreOutcome> InitializeAsync();
        Task InitializeCrossTabSessionAsync();
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<OtpOperationResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<OtpOperationResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> VerifyRegistrationOtpAsync(VerifyRegisterEmailOtpRequest request);
        Task<LoginResponse> VerifyMfaChallengeAsync(VerifyMfaChallengeRequest request);
        Task<TotpStatusResponse> GetTotpStatusAsync();
        Task<TotpEnrollmentStartResponse> StartTotpEnrollmentAsync(TotpEnrollmentStartRequest request);
        Task<TotpEnrollmentConfirmResponse> ConfirmTotpEnrollmentAsync(TotpEnrollmentConfirmRequest request);
        Task<TotpEnrollmentConfirmResponse> RegenerateTotpRecoveryCodesAsync(TotpRecoveryCodesRegenerateRequest request);
        Task<OtpOperationResponse> DisableTotpAsync(TotpDisableRequest request);
        Task<OtpOperationResponse> ChangePasswordAsync(ChangePasswordRequest request);
        Task<ConnectedAccountsResponse> GetConnectedAccountsAsync();
        Task<OtpOperationResponse> ConnectExternalAccountAsync(string provider);
        Task<OtpOperationResponse> DisconnectExternalAccountAsync(string provider, DisconnectExternalLoginRequest request);
        Task<ActiveSessionsResponse> GetActiveSessionsAsync();
        Task<SessionActionResponse> RevokeSessionAsync(Guid sessionId);
        Task<SessionActionResponse> RevokeOtherSessionsAsync();
        Task<OtpOperationResponse> ResendOtpAsync(string email);
        Task LoginWithGoogle();
        Task LoginWithGitHub();
        Task LoginWithLinkedIn();
        Task LoginWithTwitter();
        Task Logout();
    }
}
