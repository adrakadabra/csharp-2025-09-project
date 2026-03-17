using OrdersService.Api.Domain.Enums;

namespace OrdersService.Api.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public Guid OrderNumber { get; set; }
    public string UserId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public List<OrderItem> Items { get; set; } = [];
}