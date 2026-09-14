using FluentValidation;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("The username is required.")
                .Matches(@"^[a-zA-Z0-9][a-zA-Z0-9-]{1,37}[a-zA-Z0-9]$")
                .WithMessage("The username can only contain letters, numbers, and hyphens, " +
                             "It cannot start or end with a hyphen, and must be between 3 and 39 characters long.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email address is required.")
                .EmailAddress().WithMessage("The email format is invalid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password is required.")
                .MinimumLength(6).WithMessage("The password must be at least 6 characters long.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The name is mandatory.");

            RuleFor(x => x.FirstSurname)
                .NotEmpty().WithMessage("The first surname is mandatory.");

            RuleFor(x => x.SecondSurname)
                .NotEmpty().WithMessage("The second surname is mandatory.");
        }
    }
}
