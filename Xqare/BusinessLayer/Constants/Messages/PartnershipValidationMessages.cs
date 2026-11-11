namespace Xqare.BusinessLayer.Constants.Messages;

public static class PartnershipValidationMessages
{
    public const string CompanyNameRequired = "Company name is required.";
    public const string CompanyNameTooShort = "Company name must contain at least 2 characters.";
    public const string CompanyNameTooLong = "Company name cannot exceed 100 characters.";
    public const string ContactPersonRequired = "Contact person is required.";
    public const string ContactPersonTooLong = "Contact person cannot exceed 100 characters.";
    public const string ContactPersonInvalid = "Use letters only, with spaces, apostrophes or hyphens.";
    public const string WorkEmailRequired = "Work email is required.";
    public const string WorkEmailTooLong = "Work email cannot exceed 256 characters.";
    public const string WorkEmailInvalid = "Enter a valid work email address.";
    public const string PhoneLength = "Phone number must contain exactly 10 digits.";
    public const string PhoneInvalid = "Enter a valid 10-digit Indian mobile number.";
    public const string WebsiteTooLong = "Company website cannot exceed 2048 characters.";
    public const string WebsiteInvalid = "Enter a valid website URL starting with http:// or https://.";
    public const string CompanySizeRequired = "Select your company size.";
    public const string CompanySizeInvalid = "Select a valid company size.";
    public const string IndustryRequired = "Select your industry.";
    public const string IndustryInvalid = "Select a valid industry.";
    public const string RolesRequired = "Tell us which roles you are hiring for.";
    public const string RolesTooLong = "Roles cannot exceed 300 characters.";
    public const string DetailsRequired = "Additional details are required.";
    public const string DetailsTooShort = "Additional details must contain at least 10 characters.";
    public const string DetailsTooLong = "Additional details cannot exceed 2000 characters.";
}
