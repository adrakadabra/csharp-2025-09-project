using FluentValidation;
using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Api.Controllers.PickingSession.Actions.ClaimOrder;

public sealed class ClaimOrderDtoValidator : AbstractValidator<ClaimOrderDto>
{
    public ClaimOrderDtoValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0);
        
        RuleFor(x => x.PickerId)
            .GreaterThan(0);
    }
}