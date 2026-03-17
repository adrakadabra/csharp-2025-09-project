namespace OrdersService.Api.Infrastructure.Clients;

public class ProductInfo
{
    public Guid Id { get; set; }
    public string Article { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }
}