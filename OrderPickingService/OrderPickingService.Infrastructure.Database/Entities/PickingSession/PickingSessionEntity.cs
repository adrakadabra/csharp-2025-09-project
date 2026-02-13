using System.ComponentModel.DataAnnotations.Schema;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.Database.Entities.Picker;
using OrderPickingService.Infrastructure.Database.Entities.Order;

namespace OrderPickingService.Infrastructure.Database.Entities.PickingSession;

internal sealed class PickingSessionEntity : BaseEntity
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
}