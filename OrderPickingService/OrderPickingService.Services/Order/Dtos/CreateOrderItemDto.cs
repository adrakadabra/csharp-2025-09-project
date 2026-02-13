namespace OrderPickingService.Services.Order.Dtos;

public record CreateOrderItemDto(
    long ProductExternalId,
    string ProductSku,
    string ProductName,
    long Quantity,
    decimal Price,
    string Category
);