using OrderPickingService.Domain.Entities;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Domain.Services.Abstractions;

namespace OrderPickingService.Domain.Services;

public sealed class PickingProcessor : IPickingProcessor
{
    public PickingSession StartPicking(Order order, Picker picker)
    {
        if (order.OrderStatus != OrderStatus.Available)
        {
            throw new InvalidOperationException($"Order {order.Id} is not available");
        }
        
        var pickingSession = PickingSession.Create(order.Id, picker.Id);
        
        order.ClaimOrder();
        
        return pickingSession;
    }
}