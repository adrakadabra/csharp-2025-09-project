using OrderPickingService.Domain.Entities;
using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Services.Order;

internal static class OrderMappingExtensions
{
    public static Domain.Entities.Order ToOrder(this CreateOrderDto dto)
    {
        return Domain.Entities.Order.Create(
            dto.ExternalId,
            dto.Items.Select(i => i.ToOrderItem()).ToList()
            );
    }

    public static OrderDto ToOrderDto(this Domain.Entities.Order order)
    {
        return OrderDto.Load(
            order.Id, 
            order.ExternalId, 
            order.OrderStatus, 
            order.Items.Select(i => i.ToOrderItemDto()).ToList());
    }
    
    private static OrderItem ToOrderItem(this CreateOrderItemDto dto)
    {
        return OrderItem.Create(
            dto.ProductExternalId,
            dto.ProductSku,
            dto.ProductName,
            dto.Quantity,
            dto.Price,
            dto.Category);
    }

    private static OrderItemDto ToOrderItemDto(this OrderItem item)
    {
        return OrderItemDto.Load(
            item.Id, 
            item.OrderId, 
            item.ProductExternalId, 
            item.ProductSku, 
            item.ProductName,
            item.Quantity, 
            item.Price, 
            item.Category);
    }
}