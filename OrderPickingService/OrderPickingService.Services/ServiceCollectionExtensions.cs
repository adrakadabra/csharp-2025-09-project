using Microsoft.Extensions.DependencyInjection;
using OrderPickingService.Services.Order;
using OrderPickingService.Services.Order.Abstractions;
using OrderPickingService.Services.Picker;
using OrderPickingService.Services.Picker.Abstractions;

namespace OrderPickingService.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .AddScoped<IPickerService, PickerService>()
            .AddScoped<IOrderService, OrderService>()
            ;
        
        return services;
    }
}