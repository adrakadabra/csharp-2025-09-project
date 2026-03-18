namespace OrderPickingService.Services.Picking.Dtos;

public record PickItemDto(
    long PickingSessionId,
    string Sku,
    string? Note = null);