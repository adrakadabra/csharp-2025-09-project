using Common.Messages.PickingCompleted;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderPickingService.Infrastructure.Outbox.Publishing;
using OrderPickingService.Infrastructure.Outbox.Publishing.Mapping;

namespace OrderPickingService.Infrastructure.Outbox;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services
            .AddHostedService<OutboxMessageBackgroundService>()
            .AddPublishMessage<PickingCompletedMessage>()
            .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly))
            .AddScoped<IOutboxMessageMapper, OutboxMessageMapper>()
            .AddScoped<IConcreteOutboxMessageMapper, PickingCompletedOutboxMessageMapper>()
            ;
        
        return services;
    }

    private static IServiceCollection AddPublishMessage<TMessage>(this IServiceCollection services)
    {
        return services
            .AddTransient<
                IRequestHandler<PublishEventCommand<TMessage>, Result>,
                PublishEventCommandHandler<TMessage>>();
    }
}