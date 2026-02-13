using OrderPickingService.Infrastructure.Database.Entities.Order;

namespace OrderPickingService.Infrastructure.Database.Entities.PickingSession;

internal sealed class PickedItemEntity : BaseEntity
{
    public long Id { get; set; }
    public long PickingSessionId { get; set; }
    public PickingSessionEntity PickingSession { get; set; } = null!;
    public long OrderItemId { get; set; }
    public OrderItemEntity OrderItem { get; set; } = null!;
    public DateTime PickedAt { get; set; }
    public string? Note { get; set; }
    
    public PickedItemEntity() {}
}