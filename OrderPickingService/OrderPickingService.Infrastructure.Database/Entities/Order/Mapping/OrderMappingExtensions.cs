using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Infrastructure.Database.Entities.Order;

internal static class OrderMappingExtensions
{
    public static OrderEntity ToOrderEntity(this Domain.Entities.Order order)
    {
        return OrderEntity.Create(order.ExternalId, order.Items.Select(item => item.ToOrderItemEntity()).ToList());
    }
    
    public static Domain.Entities.Order ToOrder(this OrderEntity order)
    {
        return Domain.Entities.Order.Load(
            order.Id,
            order.ExternalId,
            order.OrderStatus,
            order.OrderItems.Select(item => item.ToOrderItem()).ToList());
    }

    private static OrderItem ToOrderItem(this OrderItemEntity orderItem)
    {
        return OrderItem.Load(
            orderItem.Id,
            orderItem.OrderId,
            orderItem.ProductExternalId,
            orderItem.ProductSku,
            orderItem.ProductName,
            orderItem.Quantity,
            orderItem.Price,
            orderItem.Category);
    }
    
    private static OrderItemEntity ToOrderItemEntity(this OrderItem orderItem)
    {
        return OrderItemEntity.Create(
            orderItem.ProductExternalId,
            orderItem.ProductSku,
            orderItem.ProductName,
            orderItem.Quantity,
            orderItem.Price,
            orderItem.Category);
    }
}