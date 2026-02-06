namespace OrderPickingService.Infrastructure.Database.Entities;

internal sealed class PickingSession
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long PickerId { get; set; } 
    public PickerEntity PickerEntity { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public PickingStatus PickingStatus { get; set; }
    public string Notes { get; set; }
    public List<PickedItem> PickedItems { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}