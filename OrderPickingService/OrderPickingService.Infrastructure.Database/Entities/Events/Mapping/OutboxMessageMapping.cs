using OrderPickingService.Services.Repositories.Abstractions.Dtos;

namespace OrderPickingService.Infrastructure.Database.Entities.Events;

internal static class OutboxMessageMapping
{
    public static OutboxMessageEntity ToOutboxMessageEntity(this CreateOutboxMessageDto dto)
    {
        return new OutboxMessageEntity
        {
            EventType = dto.EventType,
            Message = dto.Message,
            Attempts = 0
        };
    }
    
    public static OutboxMessageDto ToOutboxMessageDto(this OutboxMessageEntity entity)
    {
        return new OutboxMessageDto(
            entity.Id,
            entity.EventType,
            entity.Message,
            entity.ProcessedAt,
            entity.Attempts);
    }
}