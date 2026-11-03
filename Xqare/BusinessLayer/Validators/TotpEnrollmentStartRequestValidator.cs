using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpEnrollmentStartRequestValidator : AbstractValidator<TotpEnrollmentStartRequest>
    {
        public TotpEnrollmentStartRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required.").MaximumLength(128);
            RuleFor(x => x.CurrentTotpCode)
                .Must(code => string.IsNullOrWhiteSpace(code) || (code.Length == 6 && code.All(char.IsDigit)))
                .WithMessage("Authenticator code must contain exactly six digits.");
        }
    }
}
