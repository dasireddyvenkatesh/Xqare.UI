using System.Text.RegularExpressions;
using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class UpdateMyProfileRequestValidator : AbstractValidator<UpdateMyProfileRequest>
    {
        private const string IndianMobilePattern = @"^[6-9][0-9]{9}$";

        public UpdateMyProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .Must(NameValidationRules.IsValid)
                .WithMessage("First name is required and can contain letters, spaces, apostrophes and hyphens only.")
                .MaximumLength(80).WithMessage("First name cannot exceed 80 characters.");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .Must(NameValidationRules.IsValid)
                .WithMessage("Last name is required and can contain letters, spaces, apostrophes and hyphens only.")
                .MaximumLength(80).WithMessage("Last name cannot exceed 80 characters.");

            RuleFor(x => x.PhoneNumber)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                               Regex.IsMatch(value.Trim(), IndianMobilePattern))
                .WithMessage("Enter a valid 10-digit Indian mobile number.");

            RuleFor(x => x.Bio).MaximumLength(600);
        }
    }
}
