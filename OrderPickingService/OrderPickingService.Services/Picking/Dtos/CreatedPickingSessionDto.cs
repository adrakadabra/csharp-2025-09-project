using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Services.Picking.Dtos;

public record CreatedPickingSessionDto(
    long IdSession,
    List<OrderItemDto> ItemsToPick);