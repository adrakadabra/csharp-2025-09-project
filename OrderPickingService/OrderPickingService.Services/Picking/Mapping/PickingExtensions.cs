using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Services.Picking;

internal static class PickingExtensions
{
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