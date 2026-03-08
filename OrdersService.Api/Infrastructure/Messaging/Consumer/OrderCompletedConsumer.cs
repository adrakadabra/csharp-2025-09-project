using MassTransit;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Messaging.Messages;

namespace OrdersService.Api.Infrastructure.Messaging.Consumers;

public class OrderCompletedConsumer : IConsumer<OrderCompletedMessage>
{
    private readonly IOrdersService _ordersService;
    private readonly ILogger<OrderCompletedConsumer> _logger;

    public OrderCompletedConsumer(
        IOrdersService ordersService,
        ILogger<OrderCompletedConsumer> logger)
    {
        _ordersService = ordersService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCompletedMessage> context)
    {
        var message = context.Message;

        var updated = await _ordersService.SetStatusAsync(
            message.OrderId,
            OrderStatus.Completed,
            message.CompletedAt,
            context.CancellationToken);

        if (!updated)
        {
            _logger.LogWarning("Заказ {OrderId} не найден для завершения.", message.OrderId);
        }
    }
}