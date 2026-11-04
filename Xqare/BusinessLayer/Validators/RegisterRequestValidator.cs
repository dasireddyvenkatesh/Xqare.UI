using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.")
                .Must(NameValidationRules.IsValid).WithMessage("Use letters only, with spaces, apostrophes or hyphens.");


            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email address is required.")
                .MaximumLength(256).WithMessage("Email address cannot exceed 256 characters.")
                .Must(EmailValidationRules.IsValid).WithMessage("Enter a valid email address.");

        }
    }
}
