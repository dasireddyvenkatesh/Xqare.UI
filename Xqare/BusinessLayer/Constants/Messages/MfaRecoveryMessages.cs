namespace Xqare.BusinessLayer.Constants.Messages;

public static class MfaRecoveryMessages
{
    public const string RequestFailed = "The request could not be completed. Please wait and try again.";
    public const string RequestConfirmation = "If the account is eligible, a verification code has been sent.";
    public const string StartFailed = "We couldn't start recovery. Please try again.";
    public const string InvalidOrExpiredCode = "The code is invalid or expired. Request a new code and try again.";
    public const string ProviderVerificationFailed = "The linked-provider verification could not be started. Please restart recovery.";
    public const string AuthorizationMissing = "The recovery authorization was not returned. Please start again.";
    public const string EmailVerificationFailed = "Email verification could not be completed. Please try again.";
    public const string ProviderContinuationFailed = "Unable to continue with the selected provider.";
    public const string ProviderOpenFailed = "Unable to open the selected provider. Please try again.";
    public const string SessionMissing = "Your recovery session is missing. Please start again.";
    public const string AuthenticatorSetupFailed = "Unable to prepare your new authenticator.";
    public const string AuthenticatorSetupRetryFailed = "Unable to prepare your new authenticator. Please try again.";
    public const string AuthenticatorVerificationFailed = "The authenticator code could not be verified.";
    public const string RecoveryCodesMissing = "The authenticator was updated, but recovery codes were not returned. Contact support before signing in.";
    public const string CompletionFailed = "Account recovery could not be completed. Please try again.";
    public const string CopyBlocked = "Copy was blocked by the browser. Select and copy the codes manually.";
}
