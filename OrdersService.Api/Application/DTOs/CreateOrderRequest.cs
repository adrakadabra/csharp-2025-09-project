namespace OrdersService.Api.Application.DTOs;

public class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderRequest
{
    public List<CreateOrderItemRequest> Items { get; set; } = [];
}