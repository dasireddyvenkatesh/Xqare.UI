using FluentValidation;
using Xqare.BusinessLayer.Constants.Messages;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(RegistrationValidationMessages.FullNameRequired)
                .MaximumLength(100).WithMessage(RegistrationValidationMessages.FullNameTooLong)
                .Must(NameValidationRules.IsValid).WithMessage(RegistrationValidationMessages.FullNameInvalid);


            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(CommonValidationMessages.EmailRequired)
                .MaximumLength(256).WithMessage(CommonValidationMessages.EmailTooLong)
                .Must(EmailValidationRules.IsValid).WithMessage(CommonValidationMessages.EmailInvalid);

        }
    }
}
