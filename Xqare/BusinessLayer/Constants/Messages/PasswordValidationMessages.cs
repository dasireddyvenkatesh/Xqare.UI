namespace Xqare.BusinessLayer.Constants.Messages;

public static class PasswordValidationMessages
{
    public const string CurrentPasswordRequired = "Current password is required.";
    public const string NewPasswordRequired = "New password is required.";
    public const string MinimumLength = "Password must be at least 8 characters.";
    public const string UppercaseRequired = "One uppercase required";
    public const string LowercaseRequired = "One lowercase required";
    public const string NumberRequired = "One number required";
    public const string SymbolRequired = "One symbol required";
    public const string MaxLengthExceeded = "Password cannot exceed 128 characters.";
    public const string PasswordMismatch = "Passwords do not match";
    public const string PasswordMismatchSentence = "Passwords do not match.";
    public const string UppercaseRequiredSentence = "Include at least one uppercase letter.";
    public const string LowercaseRequiredSentence = "Include at least one lowercase letter.";
    public const string NumberRequiredSentence = "Include at least one number.";
    public const string SymbolRequiredSentence = "Include at least one symbol.";
    public const string ConfirmRequired = "Confirm your new password.";
}
