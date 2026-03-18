namespace OrderPickingService.Services.Order.Dtos;

public record OrderItemDto(
    long Id,
    long OrderId,
    Guid ProductExternalId,
    string ProductSku,
    string ProductName,
    long Quantity,
    decimal Price,
    string Category)
{
    public static OrderItemDto Create(Guid productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        return new OrderItemDto(0, 0, productExternalId, productSku, productName, quantity, price, category);
    }
    
    public static OrderItemDto Load(long id, long orderId, Guid productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        return new OrderItemDto(id, orderId, productExternalId, productSku, productName, quantity, price, category);
    }
};