namespace OrdersService.Api.Domain
{
    public enum OrderStatus
    {
        Created = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3,
    }
}