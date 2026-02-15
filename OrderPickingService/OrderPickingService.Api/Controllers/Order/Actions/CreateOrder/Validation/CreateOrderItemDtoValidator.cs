using FluentValidation;
using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Api.Controllers.Order.Actions.CreateOrder.Validation;

public sealed class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(x => x.ProductExternalId)
            .GreaterThan(0);
        
        RuleFor(x => x.ProductSku)
            .NotEmpty();
        
        RuleFor(x => x.ProductName)
            .NotEmpty();
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Category)
            .NotEmpty();
    }
}