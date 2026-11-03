using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class ContactusValidator : AbstractValidator<ContactUsRequest>
    {
        public ContactusValidator()
        {
            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100)
                .Matches(NameValidationRules.Pattern)
                .WithMessage("Use letters only, with spaces, apostrophes or hyphens.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Subject).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(x => x.Message).NotEmpty().MinimumLength(20).MaximumLength(500);
        }
    }
}
