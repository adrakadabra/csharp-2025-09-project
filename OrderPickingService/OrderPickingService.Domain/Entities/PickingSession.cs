using OrderPickingService.Domain.Enums;

namespace OrderPickingService.Domain.Entities;

public sealed class PickingSession
{
    public long Id { get; private set; }
    public long OrderId { get; private set; }
    public long PickerId { get; private set; } 
    public DateTime StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public PickingStatus PickingStatus { get; private set; }
    public string? Notes { get; private set; }
    public List<PickedItem> PickedItems { get; private set; }

    public static PickingSession Create(long orderId, long pickerId)
    {
        return new PickingSession(
            0, 
            orderId, 
            pickerId, 
            DateTime.UtcNow, 
            null, 
            PickingStatus.InProgress, 
            null, 
            new List<PickedItem>());
    }
    
    public static PickingSession Load(
        long id,
        long orderId,
        long pickerId,
        DateTime startedAt,
        DateTime? finishedAt,
        PickingStatus pickingStatus,
        string? notes,
        List<PickedItem> pickedItems)
    {
        return new PickingSession(id, orderId, pickerId, startedAt, finishedAt, pickingStatus, notes, pickedItems);
    }
    
    public void PickItem(PickedItem pickedItem)
    {
        PickedItems.Add(pickedItem);
    }

    public void Complete(string? notes)
    {
        FinishedAt = DateTime.UtcNow;
        PickingStatus = PickingStatus.Completed;
        Notes = notes;
    }

    public void Cancel(string reason)
    {
        Notes = reason;
        FinishedAt = DateTime.UtcNow;
        PickingStatus = PickingStatus.Canceled;
    }
    
    private PickingSession(
        long id, 
        long orderId, 
        long pickerId, 
        DateTime startedAt, 
        DateTime? finishedAt, 
        PickingStatus pickingStatus, 
        string? notes, 
        List<PickedItem> pickedItems)
    {
        Id = id;
        OrderId = orderId;
        PickerId = pickerId;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
        PickingStatus = pickingStatus;
        Notes = notes;
        PickedItems = pickedItems;
    }
}