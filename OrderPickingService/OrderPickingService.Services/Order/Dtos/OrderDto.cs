using OrderPickingService.Services.Order.Dtos;
using OrderPickingService.Domain.Enums;
namespace OrderPickingService.Services.Order.Dtos;

public sealed record OrderDto(
    long Id,
    Guid ExternalId,
    OrderStatus OrderStatus,
    List<OrderItemDto> Items
)
{
    public static OrderDto Create(
        Guid externalId, 
        OrderStatus orderStatus,
        List<OrderItemDto> items)
    {
        return new OrderDto(0, externalId, orderStatus, items);
    }
    
    public static OrderDto Load(
        long id, 
        Guid externalId, 
        OrderStatus orderStatus,
        List<OrderItemDto> items)
    {
        return new OrderDto(id, externalId, orderStatus, items);
    }
};


