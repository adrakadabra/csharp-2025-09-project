namespace OrderPickingService.Domain.Enums;

public enum OrderStatus
{
    Available = 1,
    Reserved = 101,
    Picking = 201,
    Picked = 301
}