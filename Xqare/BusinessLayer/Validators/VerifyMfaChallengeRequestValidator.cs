using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class VerifyMfaChallengeRequestValidator : AbstractValidator<VerifyMfaChallengeRequest>
    {
        private static readonly string[] AllowedFactors = { "auto", "totp", "recovery" };

        public VerifyMfaChallengeRequestValidator()
        {
            RuleFor(x => x.Factor)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Choose a verification method.")
                .Must(value => AllowedFactors.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase))
                .WithMessage("Choose a supported verification method.");

            RuleFor(x => x.ChallengeToken).MaximumLength(512);

            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("A verification code is required.")
                .MaximumLength(36).WithMessage("The verification code is too long.")
                .Must((request, value) =>
                {
                    var code = value?.Trim() ?? string.Empty;
                    var factor = request.Factor.Trim().ToLowerInvariant();

                    if (factor == "recovery")
                    {
                        var normalized = new string(code.Where(char.IsLetterOrDigit).ToArray());
                        return normalized.Length >= 20 && normalized.Length <= 36;
                    }

                    if (factor == "auto")
                    {
                        var normalized = new string(code.Where(char.IsLetterOrDigit).ToArray());
                        return (code.Length == 6 && code.All(char.IsDigit)) ||
                               (normalized.Length >= 20 && normalized.Length <= 36);
                    }

                    return code.Length == 6 && code.All(char.IsDigit);
                })
                .WithMessage("Enter a valid six-digit authenticator code or recovery code.");
        }
    }
}
