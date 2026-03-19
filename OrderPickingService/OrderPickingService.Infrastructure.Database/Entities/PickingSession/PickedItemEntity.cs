using OrderPickingService.Infrastructure.Database.Entities.Order;

namespace OrderPickingService.Infrastructure.Database.Entities.PickingSession;

public sealed class PickedItemEntity : BaseEntity
{
    public long Id { get; set; }
    public long PickingSessionId { get; set; }
    public PickingSessionEntity PickingSession { get; set; } = null!;
    public long OrderItemId { get; set; }
    public OrderItemEntity OrderItem { get; set; } = null!;
    public DateTime PickedAt { get; set; }
    public string? Note { get; set; }
    
    public PickedItemEntity() {}
    
    public static PickedItemEntity Create(long pickingSessionId, long orderItemId, string? note)
    {
        return new PickedItemEntity(){
            Id = 0, 
            PickingSessionId = pickingSessionId, 
            OrderItemId = orderItemId, 
            PickedAt = DateTime.UtcNow, 
            Note = note
        };
    }

    public static PickedItemEntity Load(long id, long pickingSessionId, long orderItemId, DateTime pickedAt, string? note)
    {
        return new PickedItemEntity(){
            Id = id, 
            PickingSessionId = pickingSessionId, 
            OrderItemId = orderItemId, 
            PickedAt = pickedAt, 
            Note = note
        };
    }
}