using FluentValidation;
using ShopAppP518.Apps.AdminApp.Dtos.ProductDto;

namespace ShopAppP518.Apps.AdminApp.Validators.ProductValidator
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("not empty").MaximumLength(40)
                .WithMessage("max length is 40 ");
            RuleFor(s => s.SalePrice).NotEmpty().WithMessage("not empty")
                .GreaterThan(100).WithMessage("should be greater than 100");
            RuleFor(s => s.CostPrice).NotEmpty().WithMessage("not empty")
                .GreaterThan(100).WithMessage("should be greater than 100");

            RuleFor(p => p)
                .Custom((p, context) =>
                {
                    if (p.SalePrice < p.CostPrice)
                    {
                        context.AddFailure("CostPrice", "it must be lower than SalePrice");
                    }
                });
        }
    }
}
