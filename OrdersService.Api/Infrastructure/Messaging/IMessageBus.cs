namespace OrdersService.Api.Infrastructure.Messaging
{
    public interface IMessageBus
    {
        Task PublishPickOrderAsync(PickOrderMessage message);
    }
}