using Common.Messages.PickingCompleted;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Domain.Entities;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Messaging.Consumers;
using OrdersService.Api.Infrastructure.Messaging.Messages;
using OrdersService.Api.Tests.Unit.Helpers;
using Xunit;

namespace OrdersService.Api.Tests.Unit.Messaging;

public class OrderCompletedConsumerTests
{
    private readonly Mock<IOrdersService> _ordersServiceMock = new();
    private readonly Mock<ILogger<PickingCompletedMessage>> _loggerMock = new();

    private OrderCompletedConsumer CreateConsumer()
    {
        return new OrderCompletedConsumer(_ordersServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_Should_Set_Order_Status_To_Completed_When_Order_Exists()
    {
        // Arrange
        var message = new PickingCompletedMessage
        (
            OrderId: 15,
            PickingId: 10,
            ExternalOrderId: Guid.NewGuid(),
            UserId: "test-user",
            StartedAt: DateTime.UtcNow,
            FinishedAt: new DateTime(2026, 3, 8, 21, 0, 0, DateTimeKind.Utc),
            PickingStatus: "Completed",
            Notes: "test-notes",
            Items: new List<PickingResultItem>()
        );

        var contextMock = new Mock<ConsumeContext<PickingCompletedMessage>>();
        contextMock.SetupGet(x => x.Message).Returns(message);
        contextMock.SetupGet(x => x.CancellationToken).Returns(CancellationToken.None);

        _ordersServiceMock
            .Setup(x => x.SetStatusAsync(
                message.ExternalOrderId,
                OrderStatus.Completed,
                message.FinishedAt,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var consumer = CreateConsumer();

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        _ordersServiceMock.Verify(x => x.SetStatusAsync(
            message.ExternalOrderId,
            OrderStatus.Completed,
            message.FinishedAt,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_Should_Log_Warning_When_Order_Not_Found()
    {
        // Arrange
        var message = new PickingCompletedMessage
        (
            OrderId: 99,
            PickingId: 10,
            ExternalOrderId: Guid.NewGuid(),
            UserId: "test-user",
            StartedAt: DateTime.UtcNow,
            FinishedAt: DateTime.UtcNow,
            PickingStatus: "Completed",
            Notes: "test-notes",
            Items: new List<PickingResultItem>()
        );

        var contextMock = new Mock<ConsumeContext<PickingCompletedMessage>>();
        contextMock.SetupGet(x => x.Message).Returns(message);
        contextMock.SetupGet(x => x.CancellationToken).Returns(CancellationToken.None);

        _ordersServiceMock
            .Setup(x => x.SetStatusAsync(
                message.ExternalOrderId,
                OrderStatus.Completed,
                message.FinishedAt,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var consumer = CreateConsumer();

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        _ordersServiceMock.Verify(x => x.SetStatusAsync(
            message.ExternalOrderId,
            OrderStatus.Completed,
            message.FinishedAt,
            It.IsAny<CancellationToken>()), Times.Once);

        _loggerMock.VerifyLog(
            LogLevel.Warning,
            msg => msg.Contains("Заказ 99 не найден"),
            Times.Once());
    }
}