namespace OrderPickingService.Infrastructure.Database.Dtos;

internal sealed class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductExternalId { get; set; }
    public string ProductSku { get; set; }
    public string ProductName { get; set; }
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public long CategoryId { get; set; } 
}