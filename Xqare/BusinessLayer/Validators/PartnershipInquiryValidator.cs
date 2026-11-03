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
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                    .WithMessage("Company name is required.")
                .MinimumLength(2)
                    .WithMessage("Company name must contain at least 2 characters.")
                .MaximumLength(100)
                    .WithMessage("Company name cannot exceed 100 characters.");

            RuleFor(x => x.ContactPerson)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Contact person is required.")
                .MaximumLength(100)
                    .WithMessage("Contact person cannot exceed 100 characters.")
                .Matches(NameValidationRules.Pattern)
                    .WithMessage("Use letters only, with spaces, apostrophes or hyphens.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Work email is required.")
                .MaximumLength(256)
                    .WithMessage("Work email cannot exceed 256 characters.")
                .EmailAddress()
                    .WithMessage("Enter a valid work email address.");

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                    .WithMessage("Phone number must contain exactly 10 digits.")
                .Matches(@"^[6-9]\d{9}$")
                    .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                    .WithMessage("Enter a valid 10-digit Indian mobile number.");

            RuleFor(x => x.CompanyWebsite)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(2048)
                    .WithMessage("Company website cannot exceed 2048 characters.")
                .Must(value =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return true;

                    return Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri) &&
                           (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
                })
                    .When(x => !string.IsNullOrWhiteSpace(x.CompanyWebsite))
                    .WithMessage("Enter a valid website URL starting with http:// or https://.");

            RuleFor(x => x.CompanySize)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Select your company size.")
                .Must(value => CompanySizes.Contains(value))
                    .WithMessage("Select a valid company size.");

            RuleFor(x => x.Industry)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Select your industry.")
                .Must(value => Industries.Contains(value))
                    .WithMessage("Select a valid industry.");

            RuleFor(x => x.Roles)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                    .WithMessage("Tell us which roles you are hiring for.")
                .MaximumLength(300)
                    .WithMessage("Roles cannot exceed 300 characters.");

            RuleFor(x => x.Details)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                    .WithMessage("Additional details are required.")
                .MinimumLength(10)
                    .WithMessage("Additional details must contain at least 10 characters.")
                .MaximumLength(2000)
                    .WithMessage("Additional details cannot exceed 2000 characters.");
        }
    }
}
