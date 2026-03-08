using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Application.Services;
using OrdersService.Api.Application.Validators;
using OrdersService.Api.Infrastructure.Clients;
using OrdersService.Api.Infrastructure.Datas;
using OrdersService.Api.Infrastructure.Interfaces;
using OrdersService.Api.Infrastructure.Messaging;
using OrdersService.Api.Infrastructure.Messaging.Consumers;
using OrdersService.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsEnvironment("Testing"))
{
    var connectionString =
        builder.Configuration.GetConnectionString("OrdersDb") ??
        "Host=localhost;Port=5432;Database=orders_db;Username=orders_user;Password=orders_password";

    builder.Services.AddDbContext<OrdersDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });
}

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrdersService, OrdersServicee>();
builder.Services.AddScoped<IOrderMessagePublisher, OrderMessagePublisher>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
builder.Services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();

builder.Services.AddHttpClient<IWarehouseClient, WarehouseClient>(client =>
{
    var warehouseBaseUrl = builder.Configuration["Warehouse:BaseUrl"] ?? "http://localhost:5002";
    client.BaseAddress = new Uri(warehouseBaseUrl);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCompletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = builder.Configuration["Rabbit:Host"] ?? "localhost";
        var rabbitVirtualHost = builder.Configuration["Rabbit:VirtualHost"] ?? "/";
        var rabbitUser = builder.Configuration["Rabbit:User"] ?? "guest";
        var rabbitPassword = builder.Configuration["Rabbit:Password"] ?? "guest";
        var completedQueue = builder.Configuration["Rabbit:CompletedQueue"] ?? "orders-completed-queue";

        cfg.Host(rabbitHost, rabbitVirtualHost, h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPassword);
        });

        cfg.ReceiveEndpoint(completedQueue, e =>
        {
            e.ConfigureConsumer<OrderCompletedConsumer>(context);
        });
    });
});

var issuer = builder.Configuration["Jwt:Issuer"] ?? "http://localhost:8080/realms/csharp-2025-09-project";
var audience = builder.Configuration["Jwt:Audience"] ?? "account";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.MetadataAddress = $"{issuer}/.well-known/openid-configuration";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "OrdersService.Api", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Вставь токен в формате: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment() && !app.Environment.IsStaging())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("Orders service working"))
    .AllowAnonymous();

app.Run();

public partial class Program
{
}