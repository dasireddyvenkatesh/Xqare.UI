using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .MaximumLength(128);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(128)
                .Matches("[A-Z]").WithMessage("Include at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Include at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Include at least one number.")
                .Matches(@"[\W_]").WithMessage("Include at least one symbol.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm your new password.")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.")
                .MaximumLength(128);
        }
    }
}
