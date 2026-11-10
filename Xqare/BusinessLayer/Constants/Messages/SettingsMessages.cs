namespace Xqare.BusinessLayer.Constants.Messages;

public static class SettingsMessages
{
    public const string ProviderConnected = "The provider has been connected.";
    public const string AuthenticatorStatusUnavailable = "Authenticator status is unavailable.";
    public const string ConnectedAccountsLoadFailed = "Unable to retrieve connected sign-in methods. Please retry.";
    public const string ProviderConnectFailed = "Unable to start provider connection. Please try again.";
    public const string DisconnectPasswordRequired = "Enter your current XQARE password to disconnect this provider.";
    public const string ProviderDisconnectFailed = "Unable to disconnect this provider. Please try again.";
    public const string PasswordSetupLinkFailed = "Unable to request a password setup link. Please try again.";
    public const string AuthenticatorStatusLoadFailed = "Unable to retrieve authenticator status. Please retry.";
    public const string ActiveSessionsLoadFailed = "Unable to retrieve active sessions. Please retry.";
    public const string AuthenticatorCodeRequired = "Enter your current six-digit authenticator code.";
}
