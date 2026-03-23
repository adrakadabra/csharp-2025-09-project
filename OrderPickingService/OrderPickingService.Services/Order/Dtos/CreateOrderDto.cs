namespace OrderPickingService.Services.Order.Dtos;

public record CreateOrderDto(
    Guid OrderNumber,
    string UserId,
    List<CreateOrderItemDto> Items);