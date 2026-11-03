using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    // Used only by the email-verification step during sign-in.
    // TOTP and recovery codes use VerifyMfaChallengeRequestValidator.
    public sealed class LoginMfaVerificationRequestValidator : AbstractValidator<VerifyEmailOtpRequest>
    {
        public LoginMfaVerificationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Enter a valid email address.")
                .MaximumLength(256);

            RuleFor(x => x.OtpCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("A verification code is required.")
                .Matches(@"^[0-9]{6}$").WithMessage("Enter the six-digit email verification code.");
        }
    }
}
