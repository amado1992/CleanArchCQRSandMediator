using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        private readonly IStringLocalizer<CreateBrandCommandValidator> _localizer;
        public CreateBrandCommandValidator(IStringLocalizer<CreateBrandCommandValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer["Validation_Name_Required"])
                .MinimumLength(2).WithMessage(_localizer["Validation_Name_MinLength", 2])
                .MaximumLength(100).WithMessage(_localizer["Validation_Name_MaxLength", 100]);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(_localizer["Validation_Description_MaxLength", 500]);

            RuleFor(x => x.TenantId)
                .GreaterThan(0).WithMessage(_localizer["Validation_Organization_GreaterThan"]);
        }
    }
}
