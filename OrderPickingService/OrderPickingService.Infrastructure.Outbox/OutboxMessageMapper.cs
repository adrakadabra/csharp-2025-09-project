using MediatR;
using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Infrastructure.Outbox;

public class OutboxMessageMapper : IOutboxMessageMapper
{
    private readonly Dictionary<string, IConcreteOutboxMessageMapper> _mappersByType;

    public OutboxMessageMapper(IEnumerable<IConcreteOutboxMessageMapper> mappers)
    {
        _mappersByType = mappers.ToDictionary(mapper => mapper.OutboxMessageType);
    }

    public IRequest<Result> Map(OutboxMessageDto outboxMessage)
    {
        if(!_mappersByType.TryGetValue(outboxMessage.EventType, out var mapper))
            throw new Exception($"Unknown event type: {outboxMessage.EventType}");
        
        return mapper.Map(outboxMessage);
    }
}