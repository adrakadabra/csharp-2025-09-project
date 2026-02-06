namespace OrderPickingService.Infrastructure.Database.Entities;

internal enum PickingStatus
{
    InProgress = 1,
    Completed = 101,
    Canceled = 102
}