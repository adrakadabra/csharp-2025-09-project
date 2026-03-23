using FluentAssertions;
using FluentValidation;
using Moq;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Services;
using OrdersService.Api.Application.Validators;
using OrdersService.Api.Domain.Entities;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Clients;
using OrdersService.Api.Infrastructure.Interfaces;
using OrdersService.Api.Infrastructure.Messaging;
using OrdersService.Api.Infrastructure.Messaging.Messages;
using Xunit;

namespace OrdersService.Api.Tests.Unit.Services;

public class OrdersServiceTests
{
    private readonly Mock<IOrdersRepository> _repositoryMock = new();
    private readonly Mock<IWarehouseClient> _warehouseClientMock = new();
    private readonly Mock<IOrderMessagePublisher> _publisherMock = new();
    private readonly Mock<IOrderPickingClient> _orderPickingClientMock = new();
    private readonly IValidator<CreateOrderRequest> _validator = new CreateOrderRequestValidator();

    private OrdersServicee CreateService()
    {
        return new OrdersServicee(
            _repositoryMock.Object,
            _warehouseClientMock.Object,
            _publisherMock.Object,
            _orderPickingClientMock.Object,
            _validator);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Create_Order_When_Request_Is_Valid()
    {
        // Arrange
        var productId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var request = new CreateOrderRequest
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

        Order? addedOrder = null;

        _warehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new ProductInfo
                {
                    Id = productId,
                    Name = "Машинное масло",
                    Article = "AUTO-0001",
                    Quantity = 3,
                    AvailableQuantity = 3
                }
            ]);

        _warehouseClientMock
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
                        Article = "AUTO-0001",
                        Quantity = 1,
                        ReservationItemStatus = "Reserved"
                    }
                ]
            });

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) =>
            {
                addedOrder = order;
                order.Id = 123;
            })
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _orderPickingClientMock
            .Setup(x => x.CreateOrderAsync(
                It.IsAny<CreateAssemblyOrderRequest>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssemblyOrderResponse
            {
                Id = 0
            });

        _publisherMock
            .Setup(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var result = await service.CreateOrderAsync(
            "user-123",
            "jwt-token",
            request,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(123);
        result.UserId.Should().Be("user-123");
        result.Status.Should().Be(OrderStatus.Created);
        result.OrderNumber.Should().NotBeEmpty();
        result.Items.Should().HaveCount(1);
        result.Items[0].ProductId.Should().Be(productId);
        result.Items[0].Quantity.Should().Be(1);

        addedOrder.Should().NotBeNull();
        addedOrder!.Id.Should().Be(123);
        addedOrder.OrderNumber.Should().NotBeEmpty();
        addedOrder.UserId.Should().Be("user-123");
        addedOrder.Items.Should().HaveCount(1);
        addedOrder.Items[0].ProductId.Should().Be(productId);
        addedOrder.Items[0].Quantity.Should().Be(1);

        _warehouseClientMock.Verify(x => x.GetProductsByIdsAsync(
            It.Is<IEnumerable<Guid>>(ids => ids.Single() == productId),
            "jwt-token",
            It.IsAny<CancellationToken>()), Times.Once);

        _warehouseClientMock.Verify(x => x.ReserveProductsAsync(
            It.Is<ReserveProductsRequest>(r =>
                r.OrderNumber != Guid.Empty &&
                r.Items.Count == 1 &&
                r.Items[0].Article == "AUTO-0001" &&
                r.Items[0].Quantity == 1),
            "jwt-token",
            It.IsAny<CancellationToken>()), Times.Once);

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);

        _orderPickingClientMock.Verify(x => x.CreateOrderAsync(
            It.Is<CreateAssemblyOrderRequest>(r =>
                r.OrderNumber == addedOrder!.OrderNumber &&
                r.UserId == "user-123" &&
                r.Items.Count == 1 &&
                r.Items[0].ProductSku == "AUTO-0001" &&
                r.Items[0].Quantity == 1),
            "jwt-token",
            It.IsAny<CancellationToken>()), Times.Once);

        _publisherMock.Verify(x => x.SendPickOrderAsync(
            It.Is<PickOrderMessage>(m =>
                m.OrderId == 123 &&
                m.UserId == "user-123" &&
                m.Items.Count == 1 &&
                m.Items[0].ProductId == productId &&
                m.Items[0].Quantity == 1),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Throw_When_Items_Are_Empty()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            Items = []
        };

        var service = CreateService();

        // Act
        Func<Task> act = async () => await service.CreateOrderAsync(
            "user-123",
            "jwt-token",
            request,
            CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Message.Should().Contain("Заказ должен содержать хотя бы один товар.");

        _warehouseClientMock.Verify(x => x.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _warehouseClientMock.Verify(x => x.ReserveProductsAsync(It.IsAny<ReserveProductsRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _orderPickingClientMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateAssemblyOrderRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Throw_When_Quantity_Is_Less_Than_Or_Equal_To_Zero()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Quantity = 0
                }
            ]
        };

        var service = CreateService();

        // Act
        Func<Task> act = async () => await service.CreateOrderAsync(
            "user-123",
            "jwt-token",
            request,
            CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Message.Should().Contain("Количество по каждому товару должно быть больше нуля.");

        _warehouseClientMock.Verify(x => x.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _warehouseClientMock.Verify(x => x.ReserveProductsAsync(It.IsAny<ReserveProductsRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _orderPickingClientMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateAssemblyOrderRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Throw_When_Product_Not_Found_In_Warehouse()
    {
        // Arrange
        var productId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var request = new CreateOrderRequest
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

        _warehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = CreateService();

        // Act
        Func<Task> act = async () => await service.CreateOrderAsync(
            "user-123",
            "jwt-token",
            request,
            CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Message.Should().Contain("Некоторые товары не найдены в сервисе склада.");

        _warehouseClientMock.Verify(x => x.ReserveProductsAsync(It.IsAny<ReserveProductsRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _orderPickingClientMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateAssemblyOrderRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Throw_When_Warehouse_Quantity_Is_Not_Enough()
    {
        // Arrange
        var productId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = productId,
                    Quantity = 5
                }
            ]
        };

        _warehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new ProductInfo
                {
                    Id = productId,
                    Name = "Product 1",
                    Article = "TEST-0001",
                    Quantity = 10,
                    AvailableQuantity = 3
                }
            ]);

        var service = CreateService();

        // Act
        Func<Task> act = async () => await service.CreateOrderAsync(
            "user-123",
            "jwt-token",
            request,
            CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Message.Should().Contain($"Недостаточно остатка на складе по товару {productId}");

        _warehouseClientMock.Verify(x => x.ReserveProductsAsync(It.IsAny<ReserveProductsRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _orderPickingClientMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateAssemblyOrderRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(x => x.SendPickOrderAsync(It.IsAny<PickOrderMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Sum_Duplicate_Product_Quantities_When_Checking_Warehouse()
    {
        // Arrange
        var productId = Guid.Parse("55555555-5555-5555-5555-555555555555");

        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = productId,
                    Quantity = 2
                },
                new CreateOrderItemRequest
                {
                    ProductId = productId,
                    Quantity = 2
                }
            ]
        };

        _warehouseClientMock
            .Setup(x => x.GetProductsByIdsAsync(
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new ProductInfo
                {
                    Id = productId,
                    Name = "Product 1",
                    Article = "TEST-0002",
                    Quantity = 10,
                    AvailableQuantity = 3
                }
            ]);

        var service = CreateService();

        // Act
        Func<Task> act = async () => await service.CreateOrderAsync(
            "user-123",
            "jwt-token",
            request,
            CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Message.Should().Contain($"Недостаточно остатка на складе по товару {productId}");

        _warehouseClientMock.Verify(x => x.GetProductsByIdsAsync(
            It.Is<IEnumerable<Guid>>(ids => ids.Count() == 1 && ids.Single() == productId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _warehouseClientMock.Verify(x => x.ReserveProductsAsync(It.IsAny<ReserveProductsRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _orderPickingClientMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateAssemblyOrderRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_OrderDto_When_Order_Exists()
    {
        // Arrange
        var productId = Guid.Parse("66666666-6666-6666-6666-666666666666");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order
            {
                Id = 10,
                OrderNumber = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = "user-123",
                Status = OrderStatus.Created,
                CreatedAt = new DateTime(2026, 3, 8, 20, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 3, 8, 20, 1, 0, DateTimeKind.Utc),
                Items =
                [
                    new OrderItem
                    {
                        Id = 1,
                        OrderId = 10,
                        ProductId = productId,
                        Quantity = 2
                    }
                ]
            });

        var service = CreateService();

        // Act
        var result = await service.GetByIdAsync(10, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
        result.OrderNumber.Should().Be(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
        result.UserId.Should().Be("user-123");
        result.Status.Should().Be(OrderStatus.Created);
        result.Items.Should().HaveCount(1);
        result.Items[0].ProductId.Should().Be(productId);
        result.Items[0].Quantity.Should().Be(2);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Order_Does_Not_Exist()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var service = CreateService();

        // Act
        var result = await service.GetByIdAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserOrdersAsync_Should_Return_User_Orders_As_Dto_List()
    {
        // Arrange
        var productId1 = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var productId2 = Guid.Parse("88888888-8888-8888-8888-888888888888");

        _repositoryMock
            .Setup(x => x.GetByUserIdAsync("user-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new Order
                {
                    Id = 1,
                    OrderNumber = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    UserId = "user-123",
                    Status = OrderStatus.Created,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-10),
                    Items =
                    [
                        new OrderItem
                        {
                            Id = 1,
                            OrderId = 1,
                            ProductId = productId1,
                            Quantity = 1
                        }
                    ]
                },
                new Order
                {
                    Id = 2,
                    OrderNumber = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    UserId = "user-123",
                    Status = OrderStatus.Completed,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-1),
                    CompletedAt = DateTime.UtcNow.AddMinutes(-1),
                    Items =
                    [
                        new OrderItem
                        {
                            Id = 2,
                            OrderId = 2,
                            ProductId = productId2,
                            Quantity = 3
                        }
                    ]
                }
            ]);

        var service = CreateService();

        // Act
        var result = await service.GetUserOrdersAsync("user-123", CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(x => x.UserId == "user-123");
        result[0].OrderNumber.Should().Be(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
        result[1].OrderNumber.Should().Be(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
        result[0].Items.Should().HaveCount(1);
        result[1].Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task SetStatusAsync_Should_Return_False_When_Order_Not_Found()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetByIdAsync(404, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var service = CreateService();

        // Act
        var result = await service.SetStatusAsync(
            404,
            OrderStatus.Completed,
            DateTime.UtcNow,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SetStatusAsync_Should_Set_Completed_Status_And_CompletedAt_When_Order_Exists()
    {
        // Arrange
        var initialUpdatedAt = new DateTime(2026, 3, 8, 20, 0, 0, DateTimeKind.Utc);
        var completedAt = new DateTime(2026, 3, 8, 21, 0, 0, DateTimeKind.Utc);

        var order = new Order
        {
            Id = 2,
            OrderNumber = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            UserId = "user-123",
            Status = OrderStatus.Created,
            CreatedAt = new DateTime(2026, 3, 8, 19, 0, 0, DateTimeKind.Utc),
            UpdatedAt = initialUpdatedAt,
            CompletedAt = null
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _repositoryMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var result = await service.SetStatusAsync(
            2,
            OrderStatus.Completed,
            completedAt,
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
        order.CompletedAt.Should().Be(completedAt);
        order.UpdatedAt.Should().BeAfter(initialUpdatedAt);

        _repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SetStatusAsync_Should_Set_CompletedAt_To_UtcNow_When_CompletedAt_Is_Null()
    {
        // Arrange
        var order = new Order
        {
            Id = 3,
            OrderNumber = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            UserId = "user-123",
            Status = OrderStatus.InProgress,
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _repositoryMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var before = DateTime.UtcNow;
        var result = await service.SetStatusAsync(3, OrderStatus.Completed, null, CancellationToken.None);
        var after = DateTime.UtcNow;

        // Assert
        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
        order.CompletedAt.Should().NotBeNull();
        order.CompletedAt.Should().BeOnOrAfter(before);
        order.CompletedAt.Should().BeOnOrBefore(after);
    }
}