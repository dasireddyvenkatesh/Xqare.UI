using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpDisableRequestValidator : AbstractValidator<TotpDisableRequest>
    {
        public TotpDisableRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required.").MaximumLength(128);
            RuleFor(x => x.Code).NotEmpty().WithMessage("Authenticator code is required.")
                .Matches(@"^\d{6}$").WithMessage("Enter the six-digit authenticator code.");
        }
    }
}
