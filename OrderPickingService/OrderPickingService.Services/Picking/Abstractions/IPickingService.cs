using OrderPickingService.Services.Order.Dtos;
using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Services.Picking.Abstractions;

public interface IPickingService
{
    Task<CreatedPickingSessionDto> ClaimOrder(ClaimOrderDto claimOrderDto, CancellationToken cancellationToken);
}