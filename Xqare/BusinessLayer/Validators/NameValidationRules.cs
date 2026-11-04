using System.Text.RegularExpressions;

namespace Xqare.BusinessLayer.Validators;

public static class NameValidationRules
{
    // Unicode letters (including combining marks), spaces, apostrophes and hyphens.
    public const string Pattern = @"^[\p{L}\p{M}]+(?:[ '\u2019-][\p{L}\p{M}]+)*$";

    public static bool IsValid(string? value)
        => !string.IsNullOrWhiteSpace(value) &&
           Regex.IsMatch(value.Trim(), Pattern, RegexOptions.CultureInvariant);
}
