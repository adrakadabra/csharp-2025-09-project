using FluentValidation;
using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Api.Controllers.PickingSession.Actions.CompletePickingSession;

public sealed class CompletePickingSessionValidator : AbstractValidator<CompletePickingSessionDto>
{
    public CompletePickingSessionValidator()
    {
        RuleFor(session => session.PickingSessionId)
            .GreaterThan(0);

    }
}