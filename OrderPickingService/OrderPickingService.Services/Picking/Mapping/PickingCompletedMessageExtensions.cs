using Common.Messages.PickingCompleted;
using OrderPickingService.Domain.Events;

namespace OrderPickingService.Services.Picking;

internal static class PickingCompletedMessageExtensions
{
    public static PickingCompletedMessage ToPickingCompletedMessage(this PickingCompletedEvent pickingCompletedEvent)
    {
        return new PickingCompletedMessage(
            OrderId: pickingCompletedEvent.OrderId,
            PickingId: pickingCompletedEvent.PickingId,
            ExternalOrderId: pickingCompletedEvent.ExternalOrderId,
            UserId: pickingCompletedEvent.UserId,
            StartedAt: pickingCompletedEvent.StartedAt,
            FinishedAt: pickingCompletedEvent.FinishedAt,
            PickingStatus: pickingCompletedEvent.PickingStatus,
            Notes: pickingCompletedEvent.Notes,
            Items: pickingCompletedEvent.Items.ToPickingResultItemList());
    }
    
    private static List<Common.Messages.PickingCompleted.PickingResultItem> ToPickingResultItemList(
        this List<OrderPickingService.Domain.Events.PickingResultItem> source)
    {
        return source.Select(item => new Common.Messages.PickingCompleted.PickingResultItem(
            OrderItemId: item.OrderItemId,
            ProductExternalId: item.ProductExternalId,
            ProductSku: item.ProductSku,
            ProductName: item.ProductName,
            ExpectedQuantity: item.ExpectedQuantity,
            ActualQuantity: item.ActualQuantity,
            Notes: item.Notes
        )).ToList();
    }
}