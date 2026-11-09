using FluentValidation;
@using Xqare.BusinessLayer.Constants.Messages
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(CommonValidationMessages.EmailRequired)
                .MaximumLength(256).WithMessage(CommonValidationMessages.EmailTooLong)
                .Must(EmailValidationRules.IsValid).WithMessage(CommonValidationMessages.EmailInvalid);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(LoginValidationMessages.PasswordRequired)
                .MaximumLength(1024).WithMessage(LoginValidationMessages.PasswordTooLong);
        }
    }
}
