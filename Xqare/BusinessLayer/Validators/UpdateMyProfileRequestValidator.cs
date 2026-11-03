using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class UpdateMyProfileRequestValidator : AbstractValidator<UpdateMyProfileRequest>
    {
        public UpdateMyProfileRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.").MaximumLength(80);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.").MaximumLength(80);
            RuleFor(x => x.PhoneNumber).MaximumLength(32);
            RuleFor(x => x.Bio).MaximumLength(600);
        }
    }
}
