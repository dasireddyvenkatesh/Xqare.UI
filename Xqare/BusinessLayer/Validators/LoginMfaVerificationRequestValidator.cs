using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    // Used only by the email-verification step during sign-in.
    // TOTP and recovery codes use VerifyMfaChallengeRequestValidator.
    public sealed class LoginMfaVerificationRequestValidator : AbstractValidator<VerifyEmailOtpRequest>
    {
        public LoginMfaVerificationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(MfaValidationMessages.EmailRequired)
                .EmailAddress().WithMessage(MfaValidationMessages.EmailInvalid)
                .MaximumLength(256);

            RuleFor(x => x.OtpCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(MfaValidationMessages.CodeRequired)
                .Matches(@"^[0-9]{6}$").WithMessage(MfaValidationMessages.InvalidEmailCode);
        }
    }
}
