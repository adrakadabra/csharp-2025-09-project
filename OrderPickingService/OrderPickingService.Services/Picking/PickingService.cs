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
    IUnitOfWork unitOfWork,
    IStorageServiceClient storageServiceClient) : IPickingService
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

    public async Task<PickItemResultDto> PickItemAsync(PickItemDto dto, CancellationToken cancellationToken)
    {
        var pickingSession = await pickingSessionRepository.GetByIdAsync(dto.PickingSessionId, cancellationToken);
        
        if(pickingSession == null)
        {
            throw new KeyNotFoundException($"Picking session with id = {dto.PickingSessionId} not found");
        }
        
        var order = await orderRepository.GetByIdAsync(pickingSession.OrderId, cancellationToken);

        if(order == null)
        {
            throw new KeyNotFoundException($"Order with id = {pickingSession.OrderId} not found");
        }

        try
        {
            await storageServiceClient.AssemblyAsync(order.ExternalId, dto.Sku, cancellationToken);
            // 3. Если успех — создать PickedItem
            // 4. Сохранить
            // 5. Вернуть результат
            
            return new PickItemResultDto(false, $"Пока не реализован функционал", null);
        }
        catch (HttpRequestException exception)
        {
            return new PickItemResultDto(false, $"Ошибка склада: {exception?.Message}", null);
        }
        catch (Exception exception)
        {
            return new PickItemResultDto(false, $"Ошибка склада: {exception?.Message}", null);
        }
    }
}