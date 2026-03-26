using MediatR;
using OrderPickingService.Services.Messages;

namespace OrderPickingService.Infrastructure.Outbox.Publishing;

public sealed class PublishEventCommandHandler<T>(IMessagePublisher messagePublisher) : IRequestHandler<PublishEventCommand<T>, Result>
{
    public async Task<Result> Handle(PublishEventCommand<T> request, CancellationToken cancellationToken)
    {
        try
        {
            await messagePublisher.PublishAsync(request.Content, cancellationToken);
        }
        catch (Exception)
        {
            return new Result { Success = false };
        }
        
        return new Result { Success = true };
    }
}