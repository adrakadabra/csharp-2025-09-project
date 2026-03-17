using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Validators;
using OrdersService.Api.Infrastructure.Clients;
using OrdersService.Api.Infrastructure.Datas;
using OrdersService.Api.Infrastructure.Interfaces;
using OrdersService.Api.Infrastructure.Messaging;

namespace OrdersService.Api.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _databaseRoot = new();

    public Mock<IWarehouseClient> WarehouseClientMock { get; } = new();
    public Mock<IOrderMessagePublisher> PublisherMock { get; } = new();
    public Mock<IOrderPickingClient> OrderPickingClientMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<OrdersDbContext>));
            services.RemoveAll(typeof(OrdersDbContext));
            services.RemoveAll(typeof(IWarehouseClient));
            services.RemoveAll(typeof(IOrderMessagePublisher));
            services.RemoveAll(typeof(IOrderPickingClient));
            services.RemoveAll(typeof(IValidator<CreateOrderRequest>));

            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseInMemoryDatabase("OrdersIntegrationTestsDb", _databaseRoot);
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
            services.AddScoped(_ => WarehouseClientMock.Object);
            services.AddScoped(_ => PublisherMock.Object);
            services.AddScoped(_ => OrderPickingClientMock.Object);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.SchemeName,
                _ => { });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}