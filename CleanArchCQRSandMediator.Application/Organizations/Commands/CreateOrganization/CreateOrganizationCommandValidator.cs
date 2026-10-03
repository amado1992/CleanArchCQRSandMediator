using FluentValidation;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        // GitHub-style slug rules: letters, numbers, and hyphens; must not start or end with a hyphen.
        private static readonly System.Text.RegularExpressions.Regex SlugRegex = new(
            @"^[a-zA-Z0-9][a-zA-Z0-9-]{1,37}[a-zA-Z0-9]$",
            System.Text.RegularExpressions.RegexOptions.Compiled);

        public CreateOrganizationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("The name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("The description cannot exceed 500 characters.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .Matches(SlugRegex)
                .WithMessage("The slug can only contain letters, numbers, and hyphens; it cannot start or end with a hyphen and must be between 3 and 39 characters long.");
        }
    }
}
