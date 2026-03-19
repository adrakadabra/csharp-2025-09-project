using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Domain.Services.Abstractions;

public interface IPickingProcessor
{
    PickingSession StartPicking(Order order, Picker picker);
    PickedItem PickItem(Order order, PickingSession session, string sku, string? note);
}