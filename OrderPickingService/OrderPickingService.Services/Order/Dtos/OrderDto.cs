using OrderPickingService.Services.Order.Dtos;
using OrderPickingService.Domain.Enums;
namespace OrderPickingService.Services.Order.Dtos;

public sealed record OrderDto(
    long Id,
    Guid ExternalId,
    string UserId,
    OrderStatus OrderStatus,
    List<OrderItemDto> Items
)
{
    public static OrderDto Create(
        Guid externalId, 
        string userId,
        OrderStatus orderStatus,
        List<OrderItemDto> items)
    {
        return new OrderDto(0, externalId, userId, orderStatus, items);
    }
    
    public static OrderDto Load(
        long id, 
        Guid externalId, 
        string userId,
        OrderStatus orderStatus,
        List<OrderItemDto> items)
    {
        return new OrderDto(id, externalId, userId, orderStatus, items);
    }
};


