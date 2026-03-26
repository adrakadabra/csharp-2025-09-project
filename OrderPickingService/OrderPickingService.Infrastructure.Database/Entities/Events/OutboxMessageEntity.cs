namespace OrderPickingService.Infrastructure.Database.Entities.Events;

public sealed class OutboxMessageEntity : BaseEntity
{
    public long Id { get; set; }
    public string EventType { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime? ProcessedAt { get; set; }
    public int Attempts { get; set; }

    public OutboxMessageEntity()
    {
    }
}