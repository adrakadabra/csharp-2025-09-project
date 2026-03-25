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

    public void CompletePickingSession(PickingSession session, Order order, string? notes)
    {
        if (session.PickingStatus != PickingStatus.InProgress)
            throw new InvalidOperationException("Session is not in progress");
        
        if(session.PickedItems.Count == 0)
            throw new InvalidOperationException("Cannot complete picking session: no items have been picked");
        
        if(order.OrderStatus != OrderStatus.Picking)
            throw new InvalidOperationException("Order is not picking");
        
        session.Complete(notes);
        order.Complete();
    }
}