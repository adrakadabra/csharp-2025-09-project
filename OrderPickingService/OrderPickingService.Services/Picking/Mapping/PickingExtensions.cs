using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Services.Picking;

internal static class PickingExtensions
{
    
    public static PickingSessionDto ToPickingSessionDto(this Domain.Entities.PickingSession pickingSession)
    {
        return new PickingSessionDto(
            pickingSession.Id,
            pickingSession.OrderId,
            pickingSession.PickerId,
            pickingSession.StartedAt,
            pickingSession.FinishedAt,
            pickingSession.PickingStatus,
            pickingSession.Notes,
            pickingSession.PickedItems.Select(item => item.ToPickedItemDto()).ToList());
    }
    
    public static PickedItemDto ToPickedItemDto(this Domain.Entities.PickedItem pickedItem)
    {
        return new PickedItemDto(
            pickedItem.Id,
            pickedItem.PickingSessionId,
            pickedItem.OrderItemId,
            pickedItem.PickedAt,
            pickedItem.Note
        );
    }
}