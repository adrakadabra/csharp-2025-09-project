namespace OrderPickingService.Infrastructure.Database.Dtos;

internal sealed class PickedItem
{
    public long Id { get; set; }
    public long PickingSessionId { get; set; }
    public long OrderItemId { get; set; }
    public DateTime PickedAt { get; set; }
    public string Note { get; set; }
}