using MediatR;

namespace OrderPickingService.Infrastructure.Outbox.Publishing;

public sealed class PublishEventCommand<T> : IRequest<Result>
{
    public required T Content { get; init; }
}