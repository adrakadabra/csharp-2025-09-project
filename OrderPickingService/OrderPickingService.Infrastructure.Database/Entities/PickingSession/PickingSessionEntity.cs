using System.ComponentModel.DataAnnotations.Schema;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.Database.Entities.Picker;
using OrderPickingService.Infrastructure.Database.Entities.Order;

namespace OrderPickingService.Infrastructure.Database.Entities.PickingSession;

public sealed class PickingSessionEntity : BaseEntity
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;
    public long PickerId { get; set; } 
    public PickerEntity Picker { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    [Column(TypeName = "picking_status")] 
    public PickingStatus PickingStatus { get; set; }
    public string? Notes { get; set; }
    public List<PickedItemEntity> PickedItems { get; set; }

    public PickingSessionEntity()
    {
        PickedItems = new List<PickedItemEntity>();
    }
    
    public static PickingSessionEntity Create(long orderId, long pickerId)
    {
        return new PickingSessionEntity()
        {
            Id = 0,
            OrderId = orderId,
            PickerId = pickerId,
            StartedAt = DateTime.UtcNow,
            FinishedAt = null,
            PickingStatus = PickingStatus.InProgress,
            Notes = null,
            PickedItems = new List<PickedItemEntity>()
        };
    }
    
    public static PickingSessionEntity Load(
        long id,
        long orderId,
        long pickerId,
        DateTime startedAt,
        DateTime? finishedAt,
        PickingStatus pickingStatus,
        string? notes,
        List<PickedItemEntity> pickedItems)
    {
        return new PickingSessionEntity()
        {
            Id = id,
            OrderId = orderId,
            PickerId = pickerId,
            StartedAt = startedAt,
            FinishedAt = finishedAt,
            PickingStatus = pickingStatus,
            Notes = notes,
            PickedItems = pickedItems
        };
    }
}