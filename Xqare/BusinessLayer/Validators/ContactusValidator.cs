using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public class ContactusValidator : AbstractValidator<ContactUsRequest>
    {
        public ContactusValidator()
        {
            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ContactValidationMessages.FullNameRequired)
                .MaximumLength(100)
                .Matches(NameValidationRules.Pattern)
                .WithMessage(ContactValidationMessages.FullNameInvalid);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Subject).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(x => x.Message).NotEmpty().MinimumLength(20).MaximumLength(500);
        }
    }
}
