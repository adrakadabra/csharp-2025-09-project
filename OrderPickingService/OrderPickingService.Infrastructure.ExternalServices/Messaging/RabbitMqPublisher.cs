using MassTransit;
using OrderPickingService.Services.Messages;

namespace OrderPickingService.Infrastructure.ExternalServices.Messaging;

internal sealed class RabbitMqPublisher(
    IPublishEndpoint publishEndpoint) : IMessagePublisher
{
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
    {
        ArgumentNullException.ThrowIfNull(message);
        await publishEndpoint.Publish(message, cancellationToken);
    }
}