namespace Xqare.BusinessLayer.Constants.Messages;

public static class ExternalLoginMessages
{
    public const string VerificationExpired = "The verification request has expired. Please sign in again.";
    public const string Success = "Signed in successfully.";
    public const string GoogleExchangeFailed = "Google sign-in could not be completed. Please try again.";
    public const string Failed = "External sign-in failed. Please try again.";
    public const string SignInExchangeMissing = "The sign-in exchange is missing. Please try again.";
    public const string ProviderUnsupported = "This sign-in provider is not supported.";
    public const string ServiceUrlMissing = "The authentication service URL is not configured.";
}
