namespace OrdersService.Api.Infrastructure.Messaging.Messages;

public class PickOrderMessage
{
    public int OrderId { get; set; }
    public string UserId { get; set; }
    public List<PickOrderItemMessage> Items { get; set; } = [];
}

public class PickOrderItemMessage
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}