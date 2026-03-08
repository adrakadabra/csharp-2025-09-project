using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Controllers;
using OrdersService.Api.Domain.Enums;
using System.Security.Claims;
using Xunit;

namespace OrdersService.Api.Tests.Unit.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<IOrdersService> _ordersServiceMock = new();

    private OrdersController CreateController(params Claim[] claims)
    {
        var controller = new OrdersController(_ordersServiceMock.Object);

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };

        return controller;
    }

    [Fact]
    public async Task Create_Should_Return_Unauthorized_When_UserId_Claim_Is_Missing()
    {
        // Arrange
        var controller = CreateController();

        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1
                }
            ]
        };

        // Act
        var result = await controller.Create(request, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Create_Should_Return_CreatedAtAction_When_Request_Is_Valid()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var controller = CreateController(new Claim("sub", "user-123"));
        controller.HttpContext.Request.Headers.Authorization = "Bearer jwt-token";

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

        var createdOrder = new OrderDto
        {
            Id = 10,
            UserId = "user-123",
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items =
            [
                new OrderItemDto
                {
                    ProductId = productId,
                    Quantity = 1
                }
            ]
        };

        _ordersServiceMock
            .Setup(x => x.CreateOrderAsync("user-123", "jwt-token", request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdOrder);

        // Act
        var result = await controller.Create(request, CancellationToken.None);

        // Assert
        var createdAt = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAt.ActionName.Should().Be(nameof(controller.GetById));
        createdAt.RouteValues!["id"].Should().Be(10);
        createdAt.Value.Should().BeEquivalentTo(createdOrder);
    }

    [Fact]
    public async Task Create_Should_Return_BadRequest_When_Service_Throws_InvalidOperationException()
    {
        // Arrange
        var controller = CreateController(new Claim("sub", "user-123"));
        controller.HttpContext.Request.Headers.Authorization = "Bearer jwt-token";

        var request = new CreateOrderRequest
        {
            Items =
            [
                new CreateOrderItemRequest
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1
                }
            ]
        };

        _ordersServiceMock
            .Setup(x => x.CreateOrderAsync("user-123", "jwt-token", request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Ошибка бизнеса"));

        // Act
        var result = await controller.Create(request, CancellationToken.None);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_Should_Return_Unauthorized_When_UserId_Claim_Is_Missing()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.GetById(1, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Order_Does_Not_Exist()
    {
        // Arrange
        var controller = CreateController(new Claim("sub", "user-123"));

        _ordersServiceMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderDto?)null);

        // Act
        var result = await controller.GetById(1, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetById_Should_Return_Forbid_When_Order_Belongs_To_Another_User()
    {
        // Arrange
        var controller = CreateController(new Claim("sub", "user-123"));

        _ordersServiceMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrderDto
            {
                Id = 1,
                UserId = "another-user",
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

        // Act
        var result = await controller.GetById(1, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task GetById_Should_Return_Ok_When_Order_Belongs_To_Current_User()
    {
        // Arrange
        var dto = new OrderDto
        {
            Id = 1,
            UserId = "user-123",
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var controller = CreateController(new Claim("sub", "user-123"));

        _ordersServiceMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        // Act
        var result = await controller.GetById(1, CancellationToken.None);

        // Assert
        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetMyOrders_Should_Return_Unauthorized_When_UserId_Claim_Is_Missing()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.GetMyOrders(CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task GetMyOrders_Should_Return_Ok_With_Orders()
    {
        // Arrange
        var orders = new List<OrderDto>
        {
            new()
            {
                Id = 1,
                UserId = "user-123",
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var controller = CreateController(new Claim("sub", "user-123"));

        _ordersServiceMock
            .Setup(x => x.GetUserOrdersAsync("user-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);

        // Act
        var result = await controller.GetMyOrders(CancellationToken.None);

        // Assert
        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(orders);
    }
}