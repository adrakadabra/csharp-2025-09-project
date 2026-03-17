namespace OrdersService.Api.Infrastructure.Clients;

public class CreateAssemblyOrderItemRequest
{
    public string Article { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class CreateAssemblyOrderRequest
{
    public Guid OrderNumber { get; set; }
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CreateAssemblyOrderItemRequest> Items { get; set; } = [];
}

public class AssemblyOrderResponse
{
    public Guid Id { get; set; }
}