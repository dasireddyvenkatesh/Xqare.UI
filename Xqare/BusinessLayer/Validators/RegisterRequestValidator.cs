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
                .Matches(NameValidationRules.Pattern).WithMessage("Use letters only, with spaces, apostrophes or hyphens.");


            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);

        }
    }
}
