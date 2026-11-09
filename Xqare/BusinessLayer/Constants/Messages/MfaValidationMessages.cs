namespace Xqare.BusinessLayer.Constants.Messages;

public static class MfaValidationMessages
{
    public const string MethodRequired = "Choose a verification method.";
    public const string MethodUnsupported = "Choose a supported verification method.";
    public const string CodeRequired = "A verification code is required.";
    public const string CodeTooLong = "The verification code is too long.";
    public const string InvalidAuthenticatorOrRecoveryCode = "Enter a valid six-digit authenticator code or recovery code.";
}
