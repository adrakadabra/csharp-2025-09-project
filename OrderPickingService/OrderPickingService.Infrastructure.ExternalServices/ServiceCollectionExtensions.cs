using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.ExternalServices.Storage;
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
}