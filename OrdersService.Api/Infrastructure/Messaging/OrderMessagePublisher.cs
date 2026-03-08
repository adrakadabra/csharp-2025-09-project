using MassTransit;
using OrdersService.Api.Infrastructure.Messaging.Messages;

namespace OrdersService.Api.Infrastructure.Messaging;

public class OrderMessagePublisher : IOrderMessagePublisher
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IConfiguration _configuration;

    public OrderMessagePublisher(ISendEndpointProvider sendEndpointProvider, IConfiguration configuration)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _configuration = configuration;
    }

    public async Task SendPickOrderAsync(
        PickOrderMessage message,
        CancellationToken cancellationToken = default)
    {
        var queueName = _configuration["Rabbit:PickQueue"] ?? "orders-pick-queue";

        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
        await endpoint.Send(message, cancellationToken);
    }
}