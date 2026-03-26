using System.Text.Json;
using Common.Messages.PickingCompleted;
using OrderPickingService.Domain.Entities;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Domain.Events;
using OrderPickingService.Domain.Services.Abstractions;
using OrderPickingService.Services.Messages;
using OrderPickingService.Services.Order;
using OrderPickingService.Services.Picking.Abstractions;
using OrderPickingService.Services.Picking.Dtos;
using OrderPickingService.Services.Repositories.Abstractions;
using OrderPickingService.Services.Repositories.Abstractions.Dtos;
using PickingResultItem = OrderPickingService.Domain.Events.PickingResultItem;

namespace OrderPickingService.Services.Picking;

internal sealed class PickingService(
    IOrderRepository orderRepository, 
    IPickerRepository pickerRepository,
    IPickingSessionRepository pickingSessionRepository,
    IPickingProcessor pickingProcessor,
    IUnitOfWork unitOfWork,
    IStorageServiceClient storageServiceClient,
    IOutboxRepository outboxRepository) : IPickingService
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

        if (pickingSession.PickingStatus != PickingStatus.InProgress)
            throw new InvalidOperationException("Session not in progress");
        
        try
        {
            await storageServiceClient.AssemblyAsync(order.ExternalId, dto.Sku, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine(exception);
            return new PickItemResultDto(false, $"Ошибка склада: {exception?.Message}", null);
        }
        
        pickingProcessor.PickItem(order, pickingSession, dto.Sku, dto.Note);
            
        await pickingSessionRepository.UpdateAsync(pickingSession, cancellationToken);
        
        var updatedSession = await pickingSessionRepository.GetByIdAsync(pickingSession.Id, cancellationToken);
        var savedItem = updatedSession?.PickedItems.Last();
        
        return new PickItemResultDto(true, $"Товар добавлен", savedItem?.ToPickedItemDto());
    }

    public async Task<PickingSessionDto> GetPickingSessionByIdAsync(long id, CancellationToken cancellationToken)
    {
        var session = await pickingSessionRepository.GetByIdAsync(id, cancellationToken);
        
        if (session == null)
            throw new KeyNotFoundException($"Session {id} not found");
    
        return new PickingSessionDto(
            session.Id,
            session.OrderId,
            session.PickerId,
            session.StartedAt,
            session.FinishedAt,
            session.PickingStatus,
            session.Notes,
            session.PickedItems.Select(i => i.ToPickedItemDto()).ToList());
    }

    public async Task<PickingSessionDto> CompletePickingSessionAsync(
        CompletePickingSessionDto dto,
        CancellationToken cancellationToken)
    {
        var pickingSession = await pickingSessionRepository.GetByIdAsync(dto.PickingSessionId, cancellationToken);

        if(pickingSession == null)
        {
            throw new KeyNotFoundException($"Picking session with id = {dto.PickingSessionId} not found");
        }
        
        var order = await orderRepository.GetByIdAsync(pickingSession.OrderId, cancellationToken);

        if(order == null)
        {
            throw new KeyNotFoundException($"Order with id = {pickingSession.OrderId} not found. Message no sended");
        }
        
        pickingProcessor.CompletePickingSession(pickingSession, order, dto.Note);
        
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            await pickingSessionRepository.UpdateAsync(pickingSession, cancellationToken);
            await orderRepository.UpdateAsync(order, cancellationToken);
            var pickingCompletedMessage = CreatePickingCompletedEvent(order, pickingSession).ToPickingCompletedMessage();
            var pickingCompletedMessageJson = JsonSerializer.Serialize<PickingCompletedMessage>(pickingCompletedMessage);
            await outboxRepository.AddAsync(new CreateOutboxMessageDto(OutboxEventTypes.PickingCompleted, pickingCompletedMessageJson), cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        
        return pickingSession.ToPickingSessionDto();
    }

    private PickingCompletedEvent CreatePickingCompletedEvent(Domain.Entities.Order order, PickingSession pickingSession)
    {
        return new PickingCompletedEvent(
            OrderId: order.Id,
            PickingId: pickingSession.Id,
            ExternalOrderId: order.ExternalId,
            UserId: order.UserId,
            StartedAt: pickingSession.StartedAt,
            FinishedAt: pickingSession.FinishedAt,
            PickingStatus: pickingSession.PickingStatus.ToString(),
            Notes: pickingSession.Notes,
            Items: pickingSession.PickedItems
                .Join(order.Items,
                    pickedItem => pickedItem.OrderItemId,
                    item => item.Id,
                    (pickedItem, orderItem) => new PickingResultItem(
                        OrderItemId: orderItem.Id,
                        ProductExternalId: orderItem.ProductExternalId,
                        ProductSku: orderItem.ProductSku,
                        ProductName: orderItem.ProductName,
                        ExpectedQuantity: orderItem.Quantity,
                        ActualQuantity: 1,
                        Notes: pickedItem.Note
                    ))
                .GroupBy(result => new {result.OrderItemId, result.ProductExternalId, result.ProductSku, result.ProductName, result.ExpectedQuantity})
                .Select(groupedResult => new PickingResultItem(
                    OrderItemId: groupedResult.Key.OrderItemId,
                    ProductExternalId: groupedResult.Key.ProductExternalId,
                    ProductSku: groupedResult.Key.ProductSku,
                    ProductName: groupedResult.Key.ProductName,
                    ExpectedQuantity: groupedResult.Key.ExpectedQuantity,
                    ActualQuantity: groupedResult.Sum(result => result.ActualQuantity),
                    Notes: string.Join("; ", groupedResult.Select(gr => gr.Notes).Where(gr => !string.IsNullOrEmpty(gr)))
                ))
                .ToList()
        );
    }
}
