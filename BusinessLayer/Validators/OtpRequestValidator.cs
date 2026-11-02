using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class OtpRequestValidator : AbstractValidator<VerifyEmailOtpRequest>
    {
        public OtpRequestValidator()
        {
            RuleFor(x => x.OtpCode)
                .NotEmpty()
                .Length(6)
                .Matches("^[0-9]*$")
                .WithMessage("OTP must be 6 digits");

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
