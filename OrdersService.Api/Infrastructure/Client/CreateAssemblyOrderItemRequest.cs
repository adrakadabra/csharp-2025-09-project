namespace OrdersService.Api.Infrastructure.Clients;

public class CreateAssemblyOrderItemRequest
{
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid ProductExternalId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; }  = null!;
}

public class CreateAssemblyOrderRequest
{
    public Guid OrderNumber { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CreateAssemblyOrderItemRequest> Items { get; set; } = [];
}

public class AssemblyOrderResponse
{
    public long Id { get; set; }
}