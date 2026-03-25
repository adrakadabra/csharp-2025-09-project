using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using OrderPickingService.Domain.Events;
using OrderPickingService.Infrastructure.ExternalServices.Messaging;
using OrderPickingService.Infrastructure.ExternalServices.Storage;
using OrderPickingService.Services.Messages;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.ExternalServices;

public static  class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["STORAGE_SERVICE_URL"] ?? "http://storage-service:8080";
    
        services.AddHttpClient<IStorageServiceClient, StorageServiceClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
    
        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busRegistration =>
        {
            busRegistration.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RMQ_HOST"] ?? "rabbitmq";
                var port = configuration.GetValue<ushort>("RMQ_PORT", 5672);
                var queueName = configuration["RMQ_PICKING_COMPLETED_QUEUE"] ?? "picking-completed-queue";

                cfg
                    .Host(host, port, "/", h =>
                    {
                        h.Username(configuration["RMQ_USER"] ?? "guest");
                        h.Password(configuration["RMQ_PASSWORD"] ?? "guest");
                    });
                
                cfg.Message<PickingCompletedEvent>(e =>
                {
                    e.SetEntityName(queueName);
                });
                
                cfg.ReceiveEndpoint(queueName, e => { });
            });
        });

        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
        
        return services;
    }
}