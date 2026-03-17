namespace OrderPickingService.Domain.Entities;

public sealed class PickedItem
{
    public long Id { get; private set; }
    public long PickingSessionId { get; private set; }
    public long OrderItemId { get; private set; }
    public DateTime PickedAt { get; private set; }
    public string? Note { get; private set; }
    
    public static PickedItem Create(long pickingSessionId, long orderItemId, string? note)
    {
        return new PickedItem(0, pickingSessionId, orderItemId, DateTime.UtcNow, note);
    }

    public static PickedItem Load(long id, long pickingSessionId, long orderItemId, DateTime pickedAt, string? note)
    {
        return new PickedItem(id, pickingSessionId, orderItemId, pickedAt, note);
    }
    
    private PickedItem(long id, long pickingSessionId, long orderItemId, DateTime pickedAt, string? note)
    {
        Id = id;
        PickingSessionId = pickingSessionId;
        OrderItemId = orderItemId;
        PickedAt = pickedAt;
        Note = note;
    }
}