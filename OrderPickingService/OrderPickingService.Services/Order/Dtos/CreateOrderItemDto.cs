namespace OrderPickingService.Services.Order.Dtos;

public record CreateOrderItemDto(
    Guid ProductExternalId,
    string ProductSku,
    string ProductName,
    long Quantity,
    decimal Price,
    string Category
);