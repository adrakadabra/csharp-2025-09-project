using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Infrastructure.Messaging;
using OrdersService.Api.Infrastructure.Messaging.OrdersService.Api.Infrastructure.Messaging;
using OrdersService.Api.Infrastructure.Persistence;
using OrdersService.Api.Infrastructure.Warehouse;
using OrdersService.Api.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("OrdersDb")
                               ?? "Host=localhost;Port=5432;Database=orders_db;Username=orders_user;Password=orders_password";

        builder.Services.AddDbContext<OrdersDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
        builder.Services.AddScoped<IOrdersService, OrdersService.Api.Services.OrdersService>();

        builder.Services.AddHttpClient<IWarehouseClient, WarehouseClient>(client =>
        {
            var warehouseBaseUrl = builder.Configuration["Warehouse:BaseUrl"] ?? "http://localhost:5002";
            client.BaseAddress = new Uri(warehouseBaseUrl);
        });

        builder.Services.AddSingleton<IMessageBus, RabbitMessageBus>();
        builder.Services.AddHostedService<OrderCompletedConsumer>();

        var jwtSection = builder.Configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"] ?? "demo-issuer";
        var audience = jwtSection["Audience"] ?? "demo-audience";
        var secret = jwtSection["Secret"] ?? "SuperSecretJwtKey1234567890";

        //TODO перемудрил, перестало рабоатть
        //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        //builder.Services
        //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = issuer,
        //            ValidAudience = audience,
        //            IssuerSigningKey = key
        //        };
        //    });

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}