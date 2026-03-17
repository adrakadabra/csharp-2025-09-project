namespace OrdersService.Api.Infrastructure.Clients;

public class ReservedItemInfo
{
    public string Article { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ReservationItemStatus { get; set; } = string.Empty;
}

public class ReservedOrderInfo
{
    public Guid OrderNumber { get; set; }
    public List<ReservedItemInfo> Items { get; set; } = [];
}