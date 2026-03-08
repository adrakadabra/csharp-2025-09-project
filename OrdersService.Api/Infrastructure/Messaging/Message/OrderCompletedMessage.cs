namespace OrdersService.Api.Infrastructure.Messaging.Messages;

public class OrderCompletedMessage
{
    public int OrderId { get; set; }
    public DateTime CompletedAt { get; set; }
}