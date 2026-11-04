namespace Xqare.BusinessLayer.Validators;

public static class EmailValidationRules
{
    public static bool IsValid(string? value)
        => !string.IsNullOrWhiteSpace(value) &&
           new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(value.Trim());
}
