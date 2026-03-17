namespace OrderPickingService.Services.Picking.Dtos;

public record ClaimOrderDto(        
    long PickerId, 
    long OrderId);