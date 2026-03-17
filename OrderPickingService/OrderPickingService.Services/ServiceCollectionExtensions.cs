using Microsoft.Extensions.DependencyInjection;
using OrderPickingService.Domain.Services;
using OrderPickingService.Domain.Services.Abstractions;
using OrderPickingService.Services.Order;
using OrderPickingService.Services.Order.Abstractions;
using OrderPickingService.Services.Picker;
using OrderPickingService.Services.Picker.Abstractions;
using OrderPickingService.Services.Picking;
using OrderPickingService.Services.Picking.Abstractions;

namespace OrderPickingService.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .AddScoped<IPickerService, PickerService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IPickingService, PickingService>()
            .AddScoped<IPickingProcessor, PickingProcessor>()
            ;
        
        return services;
    }
}