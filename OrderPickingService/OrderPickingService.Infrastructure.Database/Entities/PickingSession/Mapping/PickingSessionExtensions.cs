namespace OrderPickingService.Infrastructure.Database.Entities.PickingSession;

internal static class PickingSessionExtensions
{
    public static Domain.Entities.PickingSession ToPickingSession(this PickingSessionEntity pickingSessionEntity)
    {
        return Domain.Entities.PickingSession.Load(
            pickingSessionEntity.Id,
            pickingSessionEntity.OrderId,
            pickingSessionEntity.PickerId,
            pickingSessionEntity.StartedAt,
            pickingSessionEntity.FinishedAt,
            pickingSessionEntity.PickingStatus,
            pickingSessionEntity.Notes,
            pickingSessionEntity.PickedItems.Select(item => item.ToPickedItem()).ToList());
    }
    
    public static Domain.Entities.PickedItem ToPickedItem(this PickedItemEntity pickedItemEntity)
    {
        return Domain.Entities.PickedItem.Load(
            pickedItemEntity.Id, 
            pickedItemEntity.PickingSessionId, 
            pickedItemEntity.OrderItemId, 
            pickedItemEntity.PickedAt, 
            pickedItemEntity.Note
            );
    }

    public static PickingSessionEntity ToPickingSessionEntity(this Domain.Entities.PickingSession pickingSession)
    {
        return PickingSessionEntity.Load(
            pickingSession.Id,
            pickingSession.OrderId,
            pickingSession.PickerId,
            pickingSession.StartedAt,
            pickingSession.FinishedAt,
            pickingSession.PickingStatus,
            pickingSession.Notes,
            pickingSession.PickedItems.Select(item => item.ToPickedItemEntity()).ToList()
            );
    }
    
    public static PickedItemEntity ToPickedItemEntity(this Domain.Entities.PickedItem pickingSessionEntity)
    {
        return PickedItemEntity.Load(
            pickingSessionEntity.Id, 
            pickingSessionEntity.PickingSessionId, 
            pickingSessionEntity.OrderItemId, 
            pickingSessionEntity.PickedAt, 
            pickingSessionEntity.Note
        );
    }
}