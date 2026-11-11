using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class MfaAccountRecoveryRequestValidator : AbstractValidator<MfaAccountRecoveryRequest>
    {
        public MfaAccountRecoveryRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(CommonValidationMessages.EmailRequired)
                .MaximumLength(256).WithMessage(CommonValidationMessages.EmailTooLong)
                .Must(EmailValidationRules.IsValid).WithMessage(CommonValidationMessages.EmailInvalid);

            RuleFor(x => x.Password)
                .MaximumLength(128).WithMessage(PasswordValidationMessages.InputTooLong);
        }
    }

    public sealed class MfaAccountRecoveryOtpValidator : AbstractValidator<MfaAccountRecoveryVerifyRequest>
    {
        public MfaAccountRecoveryOtpValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(CommonValidationMessages.EmailRequired)
                .MaximumLength(256).WithMessage(CommonValidationMessages.EmailTooLong)
                .Must(EmailValidationRules.IsValid).WithMessage(CommonValidationMessages.EmailInvalid);

            RuleFor(x => x.OtpCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(MfaValidationMessages.CodeRequired)
                .Matches(@"^\d{6}$").WithMessage(MfaValidationMessages.InvalidEmailOtp);
        }
    }
}