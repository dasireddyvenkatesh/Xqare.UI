using FluentValidation;
using Xqare.BusinessLayer.DTO;

namespace Xqare.BusinessLayer.Validators
{
    public class PartnershipInquiryValidator : AbstractValidator<PartnershipInquiryRequest>
    {
        private static readonly string[] CompanySizes =
        {
            "1-50", "51-200", "201-500", "501-1000", "1000+"
        };

        private static readonly string[] Industries =
        {
            "Information Technology", "Finance", "Healthcare", "Retail",
            "Education", "Manufacturing", "Consulting", "Other"
        };

        public PartnershipInquiryValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.ContactPerson)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Contact person is required.")
                .MaximumLength(100)
                .Matches(NameValidationRules.Pattern)
                .WithMessage("Use letters only, with spaces, apostrophes or hyphens.");

            RuleFor(x => x.Phone)
                .MaximumLength(10)
                .Matches(@"^[6-9]\d{9}$")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Enter a valid 10-digit mobile number.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Work email is required.")
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.CompanyWebsite)
                .MaximumLength(2048)
                .Must(value =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return true;

                    return Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri) &&
                           (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
                })
                .WithMessage("Enter a valid website URL starting with http:// or https://.");

            RuleFor(x => x.CompanySize)
                .NotEmpty().WithMessage("Select your company size.")
                .Must(value => CompanySizes.Contains(value))
                .WithMessage("Select a valid company size.");

            RuleFor(x => x.Industry)
                .NotEmpty().WithMessage("Select your industry.")
                .Must(value => Industries.Contains(value))
                .WithMessage("Select a valid industry.");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Tell us which roles you are hiring for.")
                .MaximumLength(300);

            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Additional details are required.")
                .MinimumLength(10)
                .MaximumLength(2000);
        }
    }
}