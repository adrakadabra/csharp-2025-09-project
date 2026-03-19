using FluentValidation;
using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Api.Controllers.PickingSession.Actions.PickItem;

public sealed class PickItemDtoValidator : AbstractValidator<PickItemDto>
{
    public PickItemDtoValidator()
    {
        RuleFor(item => item.Sku)
            .NotNull()
            .NotEmpty();
        
        RuleFor(item => item.PickingSessionId)
            .NotNull()
            .GreaterThan(0);
    }
}