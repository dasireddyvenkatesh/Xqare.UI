namespace Xqare.BusinessLayer.Constants.Messages;

public static class LoginMessages
{
    public const string InvalidCredentialsFallback = "We couldn't sign you in. Check your details and try again.";
    public const string MissingSession = "Login did not return a valid session. Please try again.";
    public const string MfaVerificationFailed = "Verification failed.";
    public const string MfaMissingSession = "MFA verification did not return a valid session.";
    public const string Success = "Login successful.";
    public const string AuthenticatorCodePrompt = "Enter the six-digit code from your authenticator app.";
}
