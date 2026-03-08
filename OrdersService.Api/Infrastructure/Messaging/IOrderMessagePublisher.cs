using OrdersService.Api.Infrastructure.Messaging.Messages;

namespace OrdersService.Api.Infrastructure.Messaging;

public interface IOrderMessagePublisher
{
    Task SendPickOrderAsync(
        PickOrderMessage message,
        CancellationToken cancellationToken = default);
}