namespace OrderPickingService.Infrastructure.Database.Dtos;

internal enum PickingStatus
{
    InProgress = 1,
    Completed = 101,
    Canceled = 102
}