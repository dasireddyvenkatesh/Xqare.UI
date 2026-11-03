using FluentValidation;
using Xqare.BusinessLayer.DTO;
using System.Text.RegularExpressions;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class UpdateMyProfileRequestValidator : AbstractValidator<UpdateMyProfileRequest>
    {
        private const string IndianMobilePattern = @"^(?:\\+?91[\\s-]?)?[6-9][0-9]{9}$";

        public UpdateMyProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(80).WithMessage("First name cannot exceed 80 characters.")
                .Matches(NameValidationRules.Pattern)
                .WithMessage("First name can contain letters, spaces, apostrophes and hyphens only.");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(80).WithMessage("Last name cannot exceed 80 characters.")
                .Matches(NameValidationRules.Pattern)
                .WithMessage("Last name can contain letters, spaces, apostrophes and hyphens only.");

            RuleFor(x => x.PhoneNumber)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                               Regex.IsMatch(value.Trim(), IndianMobilePattern))
                .WithMessage("Enter a valid Indian mobile number (10 digits, optionally with +91).");

            RuleFor(x => x.Bio).MaximumLength(600);
        }
    }
}
