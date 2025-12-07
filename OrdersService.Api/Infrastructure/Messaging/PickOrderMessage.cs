namespace OrdersService.Api.Infrastructure.Messaging
{
    public class PickOrderMessage
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public List<PickOrderItem> Items { get; set; } = new List<PickOrderItem>();
    }

    public class PickOrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderCompletedMessage
    {
        public int OrderId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}