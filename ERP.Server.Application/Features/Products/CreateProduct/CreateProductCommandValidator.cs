using FluentValidation;

namespace ERP.Server.Application.Features.Products.CreateProduct;
public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).MinimumLength(3);
        RuleFor(x => x.TypeValue).GreaterThan(0).LessThan(3);
    }
}
