using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Infrastructure.Clients;
using OrdersService.Api.Infrastructure.Datas;
using OrdersService.Api.Infrastructure.Messaging.Messages;
using System.Net;
using System.Net.Http.Json;

namespace OrdersService.Api.Tests.Integration;

public class OrdersApiIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public OrdersApiIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Api_Orders_Should_Create_Order_And_Return_201()
    {
        var productId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        _factory.WarehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new ProductInfo
                {
                    Id = productId,
                    Name = "Test Product",
                    Article = "TEST-0001",
                    Quantity = 5,
                    AvailableQuantity = 5
                }
            ]);

        _factory.WarehouseClientMock
            .Setup(x => x.ReserveProductsAsync(
                It.IsAny<ReserveProductsRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReservedOrderInfo
            {
                OrderNumber = Guid.NewGuid(),
                Items =
                [
                    new ReservedItemInfo
                    {
                        Article = "TEST-0001",
                        Quantity = 2,
                        ReservationItemStatus = "Reserved"
                    }
                ]
            });

        _factory.OrderPickingClientMock
            .Setup(x => x.CreateOrderAsync(
                It.IsAny<CreateAssemblyOrderRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssemblyOrderResponse
            {
                Id = 0
            });

        _factory.PublisherMock
            .Setup(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = productId,
                    Quantity = 2
                }
            ]
        };

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var dto = await response.Content.ReadFromJsonAsync<OrderDto>();
        dto.Should().NotBeNull();
        dto!.Id.Should().BeGreaterThan(0);
        dto.OrderNumber.Should().NotBeEmpty();
        dto.UserId.Should().Be("test-user-id");
        dto.Items.Should().HaveCount(1);
        dto.Items[0].ProductId.Should().Be(productId);
        dto.Items[0].Quantity.Should().Be(2);
    }

    [Fact]
    public async Task Get_Api_Orders_Should_Return_Orders_Of_Current_User()
    {
        var productId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        _factory.WarehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new ProductInfo
                {
                    Id = productId,
                    Name = "Test Product",
                    Article = "TEST-0002",
                    Quantity = 10,
                    AvailableQuantity = 10
                }
            ]);

        _factory.WarehouseClientMock
            .Setup(x => x.ReserveProductsAsync(
                It.IsAny<ReserveProductsRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReservedOrderInfo
            {
                OrderNumber = Guid.NewGuid(),
                Items =
                [
                    new ReservedItemInfo
                    {
                        Article = "TEST-0002",
                        Quantity = 1,
                        ReservationItemStatus = "Reserved"
                    }
                ]
            });

        _factory.OrderPickingClientMock
            .Setup(x => x.CreateOrderAsync(
                It.IsAny<CreateAssemblyOrderRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssemblyOrderResponse
            {
                Id = 0
            });

        _factory.PublisherMock
            .Setup(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var createRequest = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = productId,
                    Quantity = 1
                }
            ]
        };

        var createResponse = await _client.PostAsJsonAsync("/api/orders", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getResponse = await _client.GetAsync("/api/orders");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var orders = await getResponse.Content.ReadFromJsonAsync<List<OrderDto>>();
        orders.Should().NotBeNull();
        orders.Should().NotBeEmpty();
        orders!.Should().OnlyContain(x => x.UserId == "test-user-id");
    }

    [Fact]
    public async Task Get_Api_Orders_Id_Should_Return_404_When_Order_Does_Not_Exist()
    {
        var response = await _client.GetAsync("/api/orders/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_Api_Orders_Should_Return_400_When_Product_Not_Found()
    {
        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Quantity = 1
                }
            ]
        };

        _factory.WarehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Completed_Message_Flow_Can_Be_Simulated_By_Updating_Stored_Order()
    {
        var productId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        _factory.WarehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new ProductInfo
                {
                    Id = productId,
                    Name = "Test Product",
                    Article = "TEST-0004",
                    Quantity = 10,
                    AvailableQuantity = 10
                }
            ]);

        _factory.WarehouseClientMock
            .Setup(x => x.ReserveProductsAsync(
                It.IsAny<ReserveProductsRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReservedOrderInfo
            {
                OrderNumber = Guid.NewGuid(),
                Items =
                [
                    new ReservedItemInfo
                    {
                        Article = "TEST-0004",
                        Quantity = 1,
                        ReservationItemStatus = "Reserved"
                    }
                ]
            });

        _factory.OrderPickingClientMock
            .Setup(x => x.CreateOrderAsync(
                It.IsAny<CreateAssemblyOrderRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssemblyOrderResponse
            {
                Id = 0
            });

        _factory.PublisherMock
            .Setup(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var createRequest = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = productId,
                    Quantity = 1
                }
            ]
        };

        var createResponse = await _client.PostAsJsonAsync("/api/orders", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResponse.Content.ReadFromJsonAsync<OrderDto>();
        created.Should().NotBeNull();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

        var order = await db.Orders.FindAsync(created!.Id);
        order.Should().NotBeNull();

        order!.Status = OrdersService.Api.Domain.Enums.OrderStatus.Completed;
        order.CompletedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        var getResponse = await _client.GetAsync($"/api/orders/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
        updated.Should().NotBeNull();
        updated!.Status.Should().Be(OrdersService.Api.Domain.Enums.OrderStatus.Completed);
        updated.CompletedAt.Should().NotBeNull();
    }
}