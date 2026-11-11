using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(CommonValidationMessages.EmailRequired)
                .MaximumLength(256).WithMessage(CommonValidationMessages.EmailTooLong)
                .Must(EmailValidationRules.IsValid).WithMessage(CommonValidationMessages.EmailInvalid);
        }
    }
}
