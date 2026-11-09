using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty().MaximumLength(512);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage(PasswordValidationMessages.UppercaseRequired)
                .Matches("[a-z]").WithMessage(PasswordValidationMessages.LowercaseRequired)
                .Matches("[0-9]").WithMessage(PasswordValidationMessages.NumberRequired)
                .Matches(@"[\W_]").WithMessage(PasswordValidationMessages.SymbolRequired)
                .MaximumLength(128).WithMessage(PasswordValidationMessages.MaxLengthExceeded);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password).WithMessage(PasswordValidationMessages.PasswordMismatch)
                .MaximumLength(128);
        }
    }
}
