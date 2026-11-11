namespace Xqare.BusinessLayer.Constants.Messages;

public static class RegistrationValidationMessages
{
    public const string FullNameRequired = "Full name is required.";
    public const string FullNameTooLong = "Full name cannot exceed 100 characters.";
    public const string FullNameInvalid = "Use letters only, with spaces, apostrophes or hyphens.";
    public const string OtpInvalid = "OTP must be 6 digits.";
    public const string PasswordUppercaseRequired = "One uppercase required";
    public const string PasswordLowercaseRequired = "One lowercase required";
    public const string PasswordNumberRequired = "One number required";
    public const string PasswordSymbolRequired = "One symbol required";
    public const string PasswordTooLong = "Password cannot exceed 128 characters.";
    public const string PasswordMismatch = "Passwords do not match.";
    public const string FullNameHelper = "Please enter your legal name";
    public const string EmailHelper = "Please enter your personal email id";
    public const string PasswordRequirementsHelper = "At least 8 characters, including uppercase, lowercase, a number and a symbol.";
}
