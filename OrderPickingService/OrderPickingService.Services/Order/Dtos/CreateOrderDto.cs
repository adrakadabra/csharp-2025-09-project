namespace OrderPickingService.Services.Order.Dtos;

public record CreateOrderDto(
    long ExternalId,
    List<CreateOrderItemDto> Items);