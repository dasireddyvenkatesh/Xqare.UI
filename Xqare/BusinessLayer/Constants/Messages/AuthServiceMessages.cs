namespace Xqare.BusinessLayer.Constants.Messages;

public static class AuthServiceMessages
{
    public const string MfaVerificationFailed = "MFA verification could not be completed. Please sign in again and retry.";
    public const string RecoveryStartFailed = "Unable to start account recovery. Please try again.";
    public const string RecoveryCodeConfirmationFailed = "The verification code could not be confirmed.";
    public const string AuthenticatorReplacementStartFailed = "Unable to start authenticator replacement.";
    public const string RecoveryCompletionFailed = "Unable to complete account recovery.";
    public const string ServiceUrlMissing = "The authentication service URL is not configured.";
    public const string LinkedProviderVerificationFailed = "Unable to begin linked-provider verification.";
    public const string AuthenticatorStatusFailed = "Unable to retrieve authenticator status.";
    public const string AuthenticatorSetupFailed = "Unable to start authenticator setup.";
    public const string AuthenticatorVerificationFailed = "Unable to verify the authenticator code.";
    public const string RecoveryCodesRegenerationFailed = "Unable to regenerate recovery codes.";
    public const string AuthenticatorDisableFailed = "Unable to disable the authenticator.";
    public const string ConnectedMethodsLoadFailed = "Unable to retrieve connected sign-in methods.";
    public const string AccountLinkStartFailed = "Unable to start the account-linking process.";
    public const string ProviderDisconnectFailed = "Unable to disconnect this provider.";
    public const string PasswordChangeFailed = "Unable to change your password.";
    public const string SessionsLoadFailed = "Unable to retrieve active sessions.";
    public const string SessionRevokeFailed = "Unable to revoke the selected session.";
    public const string OtherSessionsRevokeFailed = "Unable to revoke other sessions.";
}
