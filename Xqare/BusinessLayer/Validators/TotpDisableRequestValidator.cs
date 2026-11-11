using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpDisableRequestValidator : AbstractValidator<TotpDisableRequest>
    {
        public TotpDisableRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(PasswordValidationMessages.CurrentPasswordRequired).MaximumLength(128);
            RuleFor(x => x.Code).NotEmpty().WithMessage(MfaValidationMessages.AuthenticatorCodeRequired)
                .Matches(@"^\d{6}$").WithMessage(MfaValidationMessages.InvalidAuthenticatorCode);
        }
    }
}
