namespace OrderPickingService.Services.Order.Dtos;

public record OrderItemDto(
    long Id,
    long OrderId,
    long ProductExternalId,
    string ProductSku,
    string ProductName,
    long Quantity,
    decimal Price,
    string Category)
{
    public static OrderItemDto Create(long productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        return new OrderItemDto(0, 0, productExternalId, productSku, productName, quantity, price, category);
    }
    
    public static OrderItemDto Load(long id, long orderId, long productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        return new OrderItemDto(id, orderId, productExternalId, productSku, productName, quantity, price, category);
    }
};