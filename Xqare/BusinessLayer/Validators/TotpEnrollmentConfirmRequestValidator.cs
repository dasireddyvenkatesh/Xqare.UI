using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpEnrollmentConfirmRequestValidator : AbstractValidator<TotpEnrollmentConfirmRequest>
    {
        public TotpEnrollmentConfirmRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage(MfaValidationMessages.AuthenticatorCodeRequired)
                .Matches(@"^\d{6}$").WithMessage(MfaValidationMessages.InvalidAuthenticatorCode);
        }
    }
}
