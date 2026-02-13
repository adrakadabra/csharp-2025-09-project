namespace OrderPickingService.Domain.Entities;

public sealed class OrderItem
{
    public long Id { get; }
    public long OrderId  { get; private set;  } 
    public long ProductExternalId  { get; private set;  } 
    public string ProductSku  { get; private set;  } 
    public string ProductName  { get; private set;  } 
    public long Quantity  { get; private set;  } 
    public decimal Price  { get; private set;  } 
    public string Category  { get; private set;  }

    public static OrderItem Create(long productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        return new OrderItem(0, 0, productExternalId, productSku, productName, quantity, price, category);
    }
    
    public static OrderItem Load(long id, long orderId, long productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        return new OrderItem(id, orderId, productExternalId, productSku, productName, quantity, price, category);
    }
    
    private OrderItem(long id, long orderId, long productExternalId, string productSku, string productName, 
        long quantity, decimal price, string category)
    {
        Id = id;
        OrderId = orderId;
        ProductExternalId = productExternalId;
        ProductSku = productSku;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        Category = category;
    }
}