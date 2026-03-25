using OrderPickingService.Domain.Enums;

namespace OrderPickingService.Domain.Events;

public sealed record PickingCompletedEvent(
    long OrderId,
    long PickingId,
    Guid ExternalOrderId,
    string UserId,
    DateTime StartedAt,
    DateTime? FinishedAt,
    string PickingStatus,
    string? Notes,
    List<PickingResultItem> Items);