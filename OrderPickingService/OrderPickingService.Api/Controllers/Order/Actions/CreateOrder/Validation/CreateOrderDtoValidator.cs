using FluentValidation;
using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Api.Controllers.Order.Actions.CreateOrder.Validation;

public sealed class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator(CreateOrderItemDtoValidator itemDtoValidator)
    {
        
        RuleFor(x => x.OrderNumber)
            .NotEmpty();
        
        RuleFor(x => x.UserId)
            .NotEmpty();
        
        RuleFor(x => x.Items)
            .NotEmpty();
        
        RuleForEach(x => x.Items)
            .SetValidator(itemDtoValidator);
    }
}