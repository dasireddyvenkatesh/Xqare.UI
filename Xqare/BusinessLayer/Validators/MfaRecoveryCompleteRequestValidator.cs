using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class MfaRecoveryCompleteRequestValidator : AbstractValidator<MfaAccountRecoveryCompleteRequest>
    {
        public MfaRecoveryCompleteRequestValidator()
        {
            RuleFor(x => x.RecoveryGrant)
                .NotEmpty().WithMessage(MfaValidationMessages.RecoverySessionExpired)
                .MaximumLength(128);

            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(MfaValidationMessages.AuthenticatorCodeRequired)
                .Matches(@"^\d{6}$").WithMessage(MfaValidationMessages.InvalidAuthenticatorCode);
        }
    }
}