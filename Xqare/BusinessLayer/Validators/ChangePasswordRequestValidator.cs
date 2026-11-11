using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(PasswordValidationMessages.CurrentPasswordRequired)
                .MaximumLength(128);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(PasswordValidationMessages.NewPasswordRequired)
                .MinimumLength(8).WithMessage(PasswordValidationMessages.MinimumLength)
                .MaximumLength(128)
                .Matches("[A-Z]").WithMessage(PasswordValidationMessages.UppercaseRequiredSentence)
                .Matches("[a-z]").WithMessage(PasswordValidationMessages.LowercaseRequiredSentence)
                .Matches("[0-9]").WithMessage(PasswordValidationMessages.NumberRequiredSentence)
                .Matches(@"[\W_]").WithMessage(PasswordValidationMessages.SymbolRequiredSentence);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(PasswordValidationMessages.ConfirmRequired)
                .Equal(x => x.NewPassword).WithMessage(PasswordValidationMessages.PasswordMismatchSentence)
                .MaximumLength(128);
        }
    }
}
