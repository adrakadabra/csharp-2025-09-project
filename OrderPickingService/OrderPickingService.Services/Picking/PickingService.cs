using OrderPickingService.Domain.Services.Abstractions;
using OrderPickingService.Services.Order;
using OrderPickingService.Services.Picking.Abstractions;
using OrderPickingService.Services.Picking.Dtos;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Services.Picking;

internal sealed class PickingService(
    IOrderRepository orderRepository, 
    IPickerRepository pickerRepository,
    IPickingSessionRepository pickingSessionRepository,
    IPickingProcessor pickingProcessor,
    IUnitOfWork unitOfWork) : IPickingService
{
    public async Task<CreatedPickingSessionDto> ClaimOrder(
        ClaimOrderDto claimOrderDto,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(claimOrderDto.OrderId, cancellationToken);
        
        if(order == null)
        {
            throw new KeyNotFoundException($"Order with id = {claimOrderDto.OrderId} not found");
        }
        
        var picker = await pickerRepository.GetByIdAsync(claimOrderDto.PickerId, cancellationToken);
        
        if(picker == null)
        {
            throw new KeyNotFoundException($"Picker with id = {claimOrderDto.PickerId} not found");
        }
        
        var pickingSession = pickingProcessor.StartPicking(order, picker);
        
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            await orderRepository.UpdateAsync(order, cancellationToken);
            pickingSession = await pickingSessionRepository.CreateAsync(pickingSession, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return
            new CreatedPickingSessionDto(
                pickingSession.Id,
                order.ToOrderDto().Items); 
    }
}