using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class RegistrationOtpRequestValidator : AbstractValidator<VerifyRegisterEmailOtpRequest>
    {
        public RegistrationOtpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.OtpCode)
                .NotEmpty()
                .Length(6)
                .Matches(@"^[0-9]{6}$")
                .WithMessage("OTP must be 6 digits.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("One uppercase required")
                .Matches("[a-z]").WithMessage("One lowercase required")
                .Matches("[0-9]").WithMessage("One number required")
                .Matches(@"[\W_]").WithMessage("One symbol required")
                .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.")
                .MaximumLength(128);
        }
    }
}
