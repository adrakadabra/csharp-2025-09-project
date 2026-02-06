namespace OrderPickingService.Infrastructure.Database.Entities;

internal sealed class Order // корень аргрегата
{
    public long Id { get; set; }
    public long ExternalId { get; set; }
    public DateTime ExternalCreatedAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public List<PickingSession> PickingSessions { get; set; }
}