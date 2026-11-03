using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpRecoveryCodesRegenerateRequestValidator : AbstractValidator<TotpRecoveryCodesRegenerateRequest>
    {
        public TotpRecoveryCodesRegenerateRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Authenticator code is required.")
                .Matches(@"^\d{6}$").WithMessage("Enter the six-digit authenticator code.");
        }
    }
}
