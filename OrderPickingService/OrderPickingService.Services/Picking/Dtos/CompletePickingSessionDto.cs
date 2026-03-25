namespace OrderPickingService.Services.Picking.Dtos;

public record CompletePickingSessionDto(
    long PickingSessionId,
    string? Note = null);