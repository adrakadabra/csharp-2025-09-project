namespace OrderPickingService.Infrastructure.Database.Entities;

internal enum OrderStatus
{
    Available = 1,
    Reserved = 101,
    Picking = 201,
    Picked = 301
}