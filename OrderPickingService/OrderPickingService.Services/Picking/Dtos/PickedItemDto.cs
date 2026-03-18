namespace OrderPickingService.Services.Picking.Dtos;

public record PickedItemDto(
    long Id,
    long PickingSessionId,
    long OrderItemId,
    DateTime PickedAt,
    string? Note);
