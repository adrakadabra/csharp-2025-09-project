using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .UseNpgsql(options.ConnectionString)
                .UseSnakeCaseNamingConvention());
        
        services
            .AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>(
                tags: new[] { "order_picking_service" });

        services
            .AddScoped<IPickerRepository, PickerRepository>()
            ;
        
        return services;
    }
}