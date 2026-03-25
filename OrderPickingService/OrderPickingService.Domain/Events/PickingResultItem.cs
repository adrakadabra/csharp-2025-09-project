namespace OrderPickingService.Domain.Events;

public sealed record PickingResultItem(
    long OrderItemId,
    Guid ProductExternalId,
    string ProductSku,
    string ProductName,
    long ExpectedQuantity,
    long ActualQuantity,
    string? Notes);