using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class MfaAccountRecoveryRequestValidator : AbstractValidator<MfaAccountRecoveryRequest>
    {
        public MfaAccountRecoveryRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email address is required.")
                .MaximumLength(256).WithMessage("Email address cannot exceed 256 characters.")
                .Must(EmailValidationRules.IsValid).WithMessage("Enter a valid email address.");

            RuleFor(x => x.Password)
                .MaximumLength(128).WithMessage("Password input is too long.");
        }
    }

    public sealed class MfaAccountRecoveryOtpValidator : AbstractValidator<MfaAccountRecoveryVerifyRequest>
    {
        public MfaAccountRecoveryOtpValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email address is required.")
                .MaximumLength(256).WithMessage("Email address cannot exceed 256 characters.")
                .Must(EmailValidationRules.IsValid).WithMessage("Enter a valid email address.");

            RuleFor(x => x.OtpCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Verification code is required.")
                .Matches(@"^\d{6}$").WithMessage("Enter the six-digit code from your email.");
        }
    }
}