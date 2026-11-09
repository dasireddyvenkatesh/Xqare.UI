using FluentValidation;
using Xqare.BusinessLayer.DTO;
using Xqare.BusinessLayer.Constants.Messages;

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
                    .WithMessage(PartnershipValidationMessages.CompanyNameRequired)
                .MinimumLength(2)
                    .WithMessage(PartnershipValidationMessages.CompanyNameTooShort)
                .MaximumLength(100)
                    .WithMessage(PartnershipValidationMessages.CompanyNameTooLong);

            RuleFor(x => x.ContactPerson)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(PartnershipValidationMessages.ContactPersonRequired)
                .MaximumLength(100)
                    .WithMessage(PartnershipValidationMessages.ContactPersonTooLong)
                .Matches(NameValidationRules.Pattern)
                    .WithMessage(PartnershipValidationMessages.ContactPersonInvalid);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(PartnershipValidationMessages.WorkEmailRequired)
                .MaximumLength(256)
                    .WithMessage(PartnershipValidationMessages.WorkEmailTooLong)
                .EmailAddress()
                    .WithMessage(PartnershipValidationMessages.WorkEmailInvalid);

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                    .WithMessage(PartnershipValidationMessages.PhoneLength)
                .Matches(@"^[6-9]\d{9}$")
                    .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                    .WithMessage(PartnershipValidationMessages.PhoneInvalid);

            RuleFor(x => x.CompanyWebsite)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(2048)
                    .WithMessage(PartnershipValidationMessages.WebsiteTooLong)
                .Must(value =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return true;

                    return Uri.TryCreate(value.Trim(), UriKind.Absolute, out var uri) &&
                           (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
                })
                    .When(x => !string.IsNullOrWhiteSpace(x.CompanyWebsite))
                    .WithMessage(PartnershipValidationMessages.WebsiteInvalid);

            RuleFor(x => x.CompanySize)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(PartnershipValidationMessages.CompanySizeRequired)
                .Must(value => CompanySizes.Contains(value))
                    .WithMessage(PartnershipValidationMessages.CompanySizeInvalid);

            RuleFor(x => x.Industry)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(PartnershipValidationMessages.IndustryRequired)
                .Must(value => Industries.Contains(value))
                    .WithMessage(PartnershipValidationMessages.IndustryInvalid);

            RuleFor(x => x.Roles)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                    .WithMessage(PartnershipValidationMessages.RolesRequired)
                .MaximumLength(300)
                    .WithMessage(PartnershipValidationMessages.RolesTooLong);

            RuleFor(x => x.Details)
                .Cascade(CascadeMode.Stop)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                    .WithMessage(PartnershipValidationMessages.DetailsRequired)
                .MinimumLength(10)
                    .WithMessage(PartnershipValidationMessages.DetailsTooShort)
                .MaximumLength(2000)
                    .WithMessage(PartnershipValidationMessages.DetailsTooLong);
        }
    }
}
