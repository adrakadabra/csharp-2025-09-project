using MediatR;
using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Infrastructure.Outbox;

public interface IConcreteOutboxMessageMapper
{
    string OutboxMessageType { get; }
    IRequest<Result> Map(OutboxMessageDto outboxMessage);    
}