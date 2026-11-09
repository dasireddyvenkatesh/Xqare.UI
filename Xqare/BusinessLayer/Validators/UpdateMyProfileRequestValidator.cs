using System.Text.RegularExpressions;
using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

namespace Xqare.BusinessLayer.Validators
{
    public sealed class UpdateMyProfileRequestValidator : AbstractValidator<UpdateMyProfileRequest>
    {
        private const string IndianMobilePattern = @"^[6-9][0-9]{9}$";

        public UpdateMyProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .Must(NameValidationRules.IsValid)
                .WithMessage(ProfileValidationMessages.FirstNameInvalid)
                .MaximumLength(80).WithMessage(ProfileValidationMessages.FirstNameTooLong);

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .Must(NameValidationRules.IsValid)
                .WithMessage(ProfileValidationMessages.LastNameInvalid)
                .MaximumLength(80).WithMessage(ProfileValidationMessages.LastNameTooLong);

            RuleFor(x => x.PhoneNumber)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                               Regex.IsMatch(value.Trim(), IndianMobilePattern))
                .WithMessage(ProfileValidationMessages.PhoneInvalid);

            RuleFor(x => x.Bio).MaximumLength(600);
        }
    }
}
