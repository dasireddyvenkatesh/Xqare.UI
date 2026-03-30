using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("One uppercase required")
                .Matches("[a-z]").WithMessage("One lowercase required")
                .Matches("[0-9]").WithMessage("One number required")
                .Matches(@"[\W_]").WithMessage("One symbol required");
        }
    }
}
