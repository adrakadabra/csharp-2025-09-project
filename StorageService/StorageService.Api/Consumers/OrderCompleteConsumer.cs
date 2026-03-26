using Common.Messages.PickingCompleted;
using MassTransit;
using StorageService.Api.Application.Interfaces;

namespace StorageService.Api.Consumers
{
    public class OrderCompleteConsumer : IConsumer<PickingCompletedMessage>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<OrderCompleteConsumer> _logger;

        public OrderCompleteConsumer(IReservationService reservationService, ILogger<OrderCompleteConsumer> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PickingCompletedMessage> context)
        {
            var message = context.Message;
            try
            {
                _logger.LogInformation("Try complete order with id : {orderId}", message.ExternalOrderId);
                await _reservationService.CompleteReservationAsync(message.ExternalOrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Don't complete order with id: {orderId}", message.ExternalOrderId);
            }
        }
    }
}
