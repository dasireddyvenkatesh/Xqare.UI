namespace Xqare.BusinessLayer.Constants.Messages;

public static class MfaValidationMessages
{
    public const string MethodRequired = "Choose a verification method.";
    public const string MethodUnsupported = "Choose a supported verification method.";
    public const string CodeRequired = "A verification code is required.";
    public const string CodeTooLong = "The verification code is too long.";
    public const string InvalidAuthenticatorOrRecoveryCode = "Enter a valid six-digit authenticator code or recovery code.";
    public const string EmailRequired = "Email is required.";
    public const string EmailInvalid = "Enter a valid email address.";
    public const string InvalidEmailCode = "Enter the six-digit email verification code.";
    public const string InvalidEmailOtp = "Enter the six-digit code from your email.";
    public const string RecoverySessionExpired = "Your recovery session has expired. Start again.";
    public const string AuthenticatorCodeRequired = "Authenticator code is required.";
    public const string InvalidAuthenticatorCode = "Enter the six-digit authenticator code.";
    public const string AuthenticatorCodeSixDigits = "Authenticator code must contain exactly six digits.";
}
