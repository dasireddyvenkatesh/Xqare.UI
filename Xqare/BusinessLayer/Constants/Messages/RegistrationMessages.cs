namespace Xqare.BusinessLayer.Constants.Messages;

public static class RegistrationMessages
{
    public const string VerificationCodeSentPrompt = "Enter the verification code sent to your email.";
    public const string StartFailed = "Registration could not be started.";
    public const string CompletionFailed = "Registration could not be completed. Please try again.";
    public const string OtpVerificationFailed = "OTP verification failed.";
    public const string SessionMissing = "Email verified, but a login session was not returned. Please sign in.";
    public const string Success = "Registration completed successfully.";
    public const string ResendFailed = "We couldn't request another verification code. Please try again.";
}
