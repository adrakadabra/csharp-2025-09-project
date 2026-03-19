using OrderPickingService.Domain.Enums;

namespace OrderPickingService.Services.Picking.Dtos;

public record PickingSessionDto(    
    long Id,
    long OrderId,
    long PickerId,
    DateTime StartedAt,
    DateTime? FinishedAt,
    PickingStatus Status,
    string? Notes,
    List<PickedItemDto> PickedItems);