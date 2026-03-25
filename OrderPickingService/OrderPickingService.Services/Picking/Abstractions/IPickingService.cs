using OrderPickingService.Services.Order.Dtos;
using OrderPickingService.Services.Picking.Dtos;

namespace OrderPickingService.Services.Picking.Abstractions;

public interface IPickingService
{
    Task<CreatedPickingSessionDto> ClaimOrder(ClaimOrderDto claimOrderDto, CancellationToken cancellationToken);
    Task<PickItemResultDto> PickItemAsync(PickItemDto dto, CancellationToken cancellationToken);
    Task<PickingSessionDto> GetPickingSessionByIdAsync(long id, CancellationToken cancellationToken);
    Task<PickingSessionDto> CompletePickingSessionAsync(CompletePickingSessionDto completePickingSessionDto, CancellationToken cancellationToken);
}