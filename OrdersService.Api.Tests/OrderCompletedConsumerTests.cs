using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Messaging.Consumers;
using OrdersService.Api.Infrastructure.Messaging.Messages;
using OrdersService.Api.Tests.Unit.Helpers;
using Xunit;

namespace OrdersService.Api.Tests.Unit.Messaging;

public class OrderCompletedConsumerTests
{
    private readonly Mock<IOrdersService> _ordersServiceMock = new();
    private readonly Mock<ILogger<OrderCompletedConsumer>> _loggerMock = new();

    private OrderCompletedConsumer CreateConsumer()
    {
        return new OrderCompletedConsumer(_ordersServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_Should_Set_Order_Status_To_Completed_When_Order_Exists()
    {
        // Arrange
        var message = new OrderCompletedMessage
        {
            OrderId = 15,
            CompletedAt = new DateTime(2026, 3, 8, 21, 0, 0, DateTimeKind.Utc)
        };

        var contextMock = new Mock<ConsumeContext<OrderCompletedMessage>>();
        contextMock.SetupGet(x => x.Message).Returns(message);
        contextMock.SetupGet(x => x.CancellationToken).Returns(CancellationToken.None);

        _ordersServiceMock
            .Setup(x => x.SetStatusAsync(
                15,
                OrderStatus.Completed,
                message.CompletedAt,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var consumer = CreateConsumer();

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        _ordersServiceMock.Verify(x => x.SetStatusAsync(
            15,
            OrderStatus.Completed,
            message.CompletedAt,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_Should_Log_Warning_When_Order_Not_Found()
    {
        // Arrange
        var message = new OrderCompletedMessage
        {
            OrderId = 99,
            CompletedAt = DateTime.UtcNow
        };

        var contextMock = new Mock<ConsumeContext<OrderCompletedMessage>>();
        contextMock.SetupGet(x => x.Message).Returns(message);
        contextMock.SetupGet(x => x.CancellationToken).Returns(CancellationToken.None);

        _ordersServiceMock
            .Setup(x => x.SetStatusAsync(
                99,
                OrderStatus.Completed,
                message.CompletedAt,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var consumer = CreateConsumer();

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        _ordersServiceMock.Verify(x => x.SetStatusAsync(
            99,
            OrderStatus.Completed,
            message.CompletedAt,
            It.IsAny<CancellationToken>()), Times.Once);

        _loggerMock.VerifyLog(
            LogLevel.Warning,
            msg => msg.Contains("Заказ 99 не найден"),
            Times.Once());
    }
}