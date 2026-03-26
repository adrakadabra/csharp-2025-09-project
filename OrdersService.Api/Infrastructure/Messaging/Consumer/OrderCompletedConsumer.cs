using MassTransit;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Messaging.Messages;
using Common.Messages.PickingCompleted;

namespace OrdersService.Api.Infrastructure.Messaging.Consumers;

public class OrderCompletedConsumer : IConsumer<PickingCompletedMessage>
{
    private readonly IOrdersService _ordersService;
    private readonly ILogger<PickingCompletedMessage> _logger;

    public OrderCompletedConsumer(
        IOrdersService ordersService,
        ILogger<PickingCompletedMessage> logger)
    {
        _ordersService = ordersService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PickingCompletedMessage> context)
    {
        var message = context.Message;

        OrderStatus orderStatus = 
            string.Equals(message.PickingStatus, "Completed", StringComparison.OrdinalIgnoreCase) 
                ? OrderStatus.Completed
                : OrderStatus.Cancelled;
        
        var updated = await _ordersService.SetStatusAsync(
            message.ExternalOrderId,
            orderStatus,
            message.FinishedAt,
            context.CancellationToken);

        if (!updated)
        {
            _logger.LogWarning("Заказ {OrderId} не найден для завершения.", message.OrderId);
        }
    }
}