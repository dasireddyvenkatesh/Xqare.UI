namespace Xqare.BusinessLayer.Constants.Messages;

public static class ProfileValidationMessages
{
    public const string FirstNameRequired = "First name is required.";
    public const string LastNameRequired = "Last name is required.";
    public const string FirstNameInvalid = "First name is required and can contain letters, spaces, apostrophes and hyphens only.";
    public const string FirstNameTooLong = "First name cannot exceed 80 characters.";
    public const string LastNameInvalid = "Last name is required and can contain letters, spaces, apostrophes and hyphens only.";
    public const string LastNameTooLong = "Last name cannot exceed 80 characters.";
    public const string PhoneInvalid = "Enter a valid 10-digit Indian mobile number.";
    public const string PhoneHelper = "Enter your 10-digit Indian mobile number only.";
}
