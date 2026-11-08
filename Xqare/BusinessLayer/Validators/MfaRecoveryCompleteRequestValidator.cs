using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class MfaRecoveryCompleteRequestValidator : AbstractValidator<MfaAccountRecoveryCompleteRequest>
    {
        public MfaRecoveryCompleteRequestValidator()
        {
            RuleFor(x => x.RecoveryGrant)
                .NotEmpty().WithMessage("Your recovery session has expired. Start again.")
                .MaximumLength(128);

            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Authenticator code is required.")
                .Matches(@"^\d{6}$").WithMessage("Enter a valid six-digit authenticator code.");
        }
    }
}