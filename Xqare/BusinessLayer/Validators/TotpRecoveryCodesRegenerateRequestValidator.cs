using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpRecoveryCodesRegenerateRequestValidator : AbstractValidator<TotpRecoveryCodesRegenerateRequest>
    {
        public TotpRecoveryCodesRegenerateRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage(MfaValidationMessages.AuthenticatorCodeRequired)
                .Matches(@"^\d{6}$").WithMessage(MfaValidationMessages.InvalidAuthenticatorCode);
        }
    }
}
