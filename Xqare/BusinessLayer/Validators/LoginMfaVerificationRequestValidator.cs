using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class LoginMfaVerificationRequestValidator : AbstractValidator<VerifyEmailOtpRequest>
    {
        public LoginMfaVerificationRequestValidator(Func<bool> isRecoveryCodeStep)
        {
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("A verification code is required.")
                .Must(code =>
                {
                    var value = code?.Trim() ?? string.Empty;
                    if (isRecoveryCodeStep())
                    {
                        var normalized = new string(value.Where(char.IsLetterOrDigit).ToArray());
                        return normalized.Length >= 20 && normalized.Length <= 36;
                    }

                    return value.Length == 6 && value.All(char.IsDigit);
                })
                .WithMessage("Enter a valid six-digit code or recovery code.");
        }
    }
}
