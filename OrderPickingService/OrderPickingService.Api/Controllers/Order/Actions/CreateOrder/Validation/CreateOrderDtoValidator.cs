using FluentValidation;
using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Api.Controllers.Order.Actions.CreateOrder.Validation;

public sealed class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator(CreateOrderItemDtoValidator itemDtoValidator)
    {
        RuleFor(x => x.ExternalId)
            .GreaterThan(0);
        
        RuleFor(x => x.Items)
            .NotEmpty();
        
        RuleForEach(x => x.Items)
            .SetValidator(itemDtoValidator);
    }
}