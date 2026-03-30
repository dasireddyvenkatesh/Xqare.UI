using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class PartnershipInquiryValidator : AbstractValidator<PartnershipInquiryRequest>
    {
        public PartnershipInquiryValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .MinimumLength(2);

            RuleFor(x => x.ContactPerson)
                .NotEmpty();

            RuleFor(x => x.Phone)
                .Matches(@"^[6-9]\d{9}$")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Enter a valid 10-digit mobile number");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.CompanySize)
                .NotEmpty();

            RuleFor(x => x.Industry)
                .NotEmpty();

            RuleFor(x => x.Roles)
                .NotEmpty();

            RuleFor(x => x.Details)
                .MinimumLength(10);
        }
    }
}
