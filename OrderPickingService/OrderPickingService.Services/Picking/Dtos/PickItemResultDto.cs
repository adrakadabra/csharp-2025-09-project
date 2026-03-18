namespace OrderPickingService.Services.Picking.Dtos;

public record PickItemResultDto(
    bool Success,
    string Message,
    PickedItemDto? Item = null);