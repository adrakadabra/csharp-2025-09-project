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
    
    public PickedItem PickItem(Order order, PickingSession session, string sku, string? note)
    {
        if (session.PickingStatus != PickingStatus.InProgress)
            throw new InvalidOperationException("Session is not in progress");
        
        var orderItem = order.Items.FirstOrDefault(i => i.ProductSku == sku);
        
        if (orderItem == null)
            throw new InvalidOperationException($"Item with sku {sku} not found in order");
        
        var alreadyPickedCount = session.PickedItems.Count(i => i.OrderItemId == orderItem.Id);
        
        if (alreadyPickedCount >= orderItem.Quantity)
            throw new InvalidOperationException("All items already picked");
        
        var pickedItem = PickedItem.Create(session.Id, orderItem.Id, note);
        session.PickItem(pickedItem);
    
        return pickedItem;
    }
}