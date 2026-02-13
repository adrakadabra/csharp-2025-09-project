using System.ComponentModel.DataAnnotations.Schema;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.Database.Entities.PickingSession;

namespace OrderPickingService.Infrastructure.Database.Entities.Order;

internal sealed class OrderEntity : BaseEntity
{
    public long Id { get; set; }
    public long ExternalId { get; set; }
    
    [Column(TypeName = "order_status")]
    public OrderStatus OrderStatus { get; set; }
    public List<OrderItemEntity> OrderItems { get; set; }
    public List<PickingSessionEntity> PickingSessions { get; set; }

    public OrderEntity()
    {
        OrderItems = new List<OrderItemEntity>();
        PickingSessions = new List<PickingSessionEntity>();
    }

    public static OrderEntity Create(long externalId, List<OrderItemEntity> orderItems)
    {
        return new OrderEntity()
        {
            ExternalId = externalId,
            OrderStatus = OrderStatus.Available,
            OrderItems = orderItems
        };
    }
}