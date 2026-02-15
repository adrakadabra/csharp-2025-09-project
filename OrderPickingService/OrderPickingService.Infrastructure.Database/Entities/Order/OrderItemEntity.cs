namespace OrderPickingService.Infrastructure.Database.Entities.Order;

internal sealed class OrderItemEntity : BaseEntity
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;
    public long ProductExternalId { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;

    public OrderItemEntity() {}

    public static OrderItemEntity Create(
        long orderItemProductExternalId, 
        string orderItemProductSku, 
        string orderItemProductName, 
        long orderItemQuantity, 
        decimal orderItemPrice, 
        string orderItemCategory)
    {
        return new OrderItemEntity()
        {
            Id = 0,
            OrderId = 0,
            ProductExternalId = orderItemProductExternalId,
            ProductSku = orderItemProductSku,
            ProductName = orderItemProductName,
            Quantity = orderItemQuantity,
            Price = orderItemPrice,
            Category = orderItemCategory
        };
    }
}