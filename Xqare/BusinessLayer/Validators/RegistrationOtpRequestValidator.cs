using FluentValidation;
@using Xqare.BusinessLayer.Constants.Messages
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class RegistrationOtpRequestValidator : AbstractValidator<VerifyRegisterEmailOtpRequest>
    {
        public RegistrationOtpRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(CommonValidationMessages.EmailRequired)
                .MaximumLength(256).WithMessage(CommonValidationMessages.EmailTooLong)
                .Must(EmailValidationRules.IsValid).WithMessage(CommonValidationMessages.EmailInvalid);

            RuleFor(x => x.OtpCode)
                .NotEmpty()
                .Length(6)
                .Matches(@"^[0-9]{6}$")
                .WithMessage(RegistrationValidationMessages.OtpInvalid);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage(RegistrationValidationMessages.PasswordUppercaseRequired)
                .Matches("[a-z]").WithMessage(RegistrationValidationMessages.PasswordLowercaseRequired)
                .Matches("[0-9]").WithMessage(RegistrationValidationMessages.PasswordNumberRequired)
                .Matches(@"[\W_]").WithMessage(RegistrationValidationMessages.PasswordSymbolRequired)
                .MaximumLength(128).WithMessage(RegistrationValidationMessages.PasswordTooLong);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage(RegistrationValidationMessages.PasswordMismatch)
                .MaximumLength(128);
        }
    }
}
