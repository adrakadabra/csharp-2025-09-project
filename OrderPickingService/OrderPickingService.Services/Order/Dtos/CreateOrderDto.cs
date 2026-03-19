namespace OrderPickingService.Services.Order.Dtos;

public record CreateOrderDto(
    Guid ExternalId,
    List<CreateOrderItemDto> Items);