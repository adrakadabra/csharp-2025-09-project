using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.Database.Repositories;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetSection("DataBase")
            .Get<DatabaseOptions>();
        
        services
            .AddDbContext<DatabaseContext>(builder
            => builder
                .UseNpgsql(options.ConnectionString, options =>
                    options
                        .MapEnum<OrderStatus>()
                        .MapEnum<PickingStatus>())
                .UseSnakeCaseNamingConvention());
        
        services
            .AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>(
                tags: ["order_picking_service"]);

        services
            .AddScoped<IPickerRepository, PickerRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            ;
        
        return services;
    }
}