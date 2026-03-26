using System.Text.Json;
using Common.Messages.PickingCompleted;
using MediatR;
using OrderPickingService.Services.Messages;
using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Infrastructure.Outbox.Publishing.Mapping;

public class PickingCompletedOutboxMessageMapper : IConcreteOutboxMessageMapper
{
    public string OutboxMessageType => OutboxEventTypes.PickingCompleted;
    public IRequest<Result> Map(OutboxMessageDto outboxMessage)
    {
        var result = JsonSerializer.Deserialize<PickingCompletedMessage>(outboxMessage.Message);

        if (result is null)
            throw new Exception("Could not deserialize picking completed outbox message");
        
        return new PublishEventCommand<PickingCompletedMessage>{ Content = result};
    }
}