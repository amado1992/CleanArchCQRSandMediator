using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        private readonly IStringLocalizer<CreateOrganizationCommandValidator> _localizer;

        // GitHub-style slug rules: letters, numbers, and hyphens; must not start or end with a hyphen.
        private static readonly System.Text.RegularExpressions.Regex SlugRegex = new(
            @"^[a-zA-Z0-9][a-zA-Z0-9-]{1,37}[a-zA-Z0-9]$",
            System.Text.RegularExpressions.RegexOptions.Compiled);

        public CreateOrganizationCommandValidator(IStringLocalizer<CreateOrganizationCommandValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer["Validation_Name_Required"])
                .MinimumLength(3).WithMessage(_localizer["Validation_Name_MinLength", 3])
                .MaximumLength(100).WithMessage(_localizer["Validation_Name_MaxLength", 100]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(_localizer["Validation_Description_Required"])
                .MinimumLength(10).WithMessage(_localizer["Validation_Description_MinLength", 10])
                .MaximumLength(500).WithMessage(_localizer["Validation_Description_MaxLength", 500]);

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage(_localizer["Validation_Slug_Required"])
                .Matches(SlugRegex)
                .WithMessage(_localizer["Validation_Slug_Pattern"]);
        }
    }
}
