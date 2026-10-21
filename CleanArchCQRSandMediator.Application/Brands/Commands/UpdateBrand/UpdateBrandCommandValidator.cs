using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        private readonly IStringLocalizer<UpdateBrandCommandValidator> _localizer;
        public UpdateBrandCommandValidator(IStringLocalizer<UpdateBrandCommandValidator> localizer)
        {
            _localizer = localizer;
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_localizer["Validation_Brand_GreaterThan"]);

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
