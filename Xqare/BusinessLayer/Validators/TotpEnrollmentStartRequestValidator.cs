using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class TotpEnrollmentStartRequestValidator : AbstractValidator<TotpEnrollmentStartRequest>
    {
        public TotpEnrollmentStartRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(PasswordValidationMessages.CurrentPasswordRequired).MaximumLength(128);
            RuleFor(x => x.CurrentTotpCode)
                .Must(code => string.IsNullOrWhiteSpace(code) || (code.Length == 6 && code.All(char.IsDigit)))
                .WithMessage(MfaValidationMessages.AuthenticatorCodeSixDigits);
        }
    }
}
