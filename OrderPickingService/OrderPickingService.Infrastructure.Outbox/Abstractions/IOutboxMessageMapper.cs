using MediatR;
using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Infrastructure.Outbox;

public interface IOutboxMessageMapper
{
    IRequest<Result> Map(OutboxMessageDto outboxMessage);    
}