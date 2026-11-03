using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpEnrollmentConfirmRequestValidator : AbstractValidator<TotpEnrollmentConfirmRequest>
    {
        public TotpEnrollmentConfirmRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Authenticator code is required.")
                .Matches(@"^\d{6}$").WithMessage("Enter the six-digit authenticator code.");
        }
    }
}
