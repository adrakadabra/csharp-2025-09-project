namespace OrdersService.Api.Infrastructure.Clients;

public class ReserveItemRequest
{
    public string Article { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class ReserveProductsRequest
{
    public Guid OrderNumber { get; set; }
    public List<ReserveItemRequest> Items { get; set; } = [];
}