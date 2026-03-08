using FluentValidation;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Application.Mappers;
using OrdersService.Api.Domain.Entities;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Interfaces;
using OrdersService.Api.Infrastructure.Messaging;
using OrdersService.Api.Infrastructure.Messaging.Messages;

namespace OrdersService.Api.Application.Services;

public class OrdersServicee : IOrdersService
{
    private readonly IOrdersRepository _repository;
    private readonly IWarehouseClient _warehouseClient;
    private readonly IOrderMessagePublisher _orderMessagePublisher;
    private readonly IValidator<CreateOrderRequest> _validator;

    public OrdersServicee(
        IOrdersRepository repository,
        IWarehouseClient warehouseClient,
        IOrderMessagePublisher orderMessagePublisher,
        IValidator<CreateOrderRequest> validator)
    {
        _repository = repository;
        _warehouseClient = warehouseClient;
        _orderMessagePublisher = orderMessagePublisher;
        _validator = validator;
    }

    public async Task<OrderDto> CreateOrderAsync(
        string userId,
        string userJwt,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var productIds = request.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await _warehouseClient.GetProductsByIdsAsync(productIds, userJwt, cancellationToken);

        if (products.Count != productIds.Count)
        {
            throw new InvalidOperationException("Некоторые товары не найдены в сервисе склада.");
        }

        var requestedQuantities = request.Items
            .GroupBy(x => x.ProductId)
            .ToDictionary(x => x.Key, x => x.Sum(i => i.Quantity));

        var warehouseQuantities = products
            .ToDictionary(x => x.Id, x => x.Quantity);

        foreach (var requested in requestedQuantities)
        {
            if (!warehouseQuantities.TryGetValue(requested.Key, out var availableQuantity))
            {
                throw new InvalidOperationException($"Товар {requested.Key} не найден на складе.");
            }

            if (availableQuantity < requested.Value)
            {
                throw new InvalidOperationException($"Недостаточно остатка на складе по товару {requested.Key}.");
            }
        }

        var now = DateTime.UtcNow;

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Created,
            CreatedAt = now,
            UpdatedAt = now
        };

        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        await _repository.AddAsync(order, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var message = new PickOrderMessage
        {
            OrderId = order.Id,
            UserId = order.UserId,
            Items = order.Items.Select(i => new PickOrderItemMessage
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        await _orderMessagePublisher.SendPickOrderAsync(message, cancellationToken);

        return order.ToDto();
    }

    public async Task<OrderDto?> GetByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(orderId, cancellationToken);
        return order?.ToDto();
    }

    public Task<List<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<OrderDto>> GetUserOrdersAsync(string userId, CancellationToken cancellationToken = default)
    {
        var orders = await _repository.GetByUserIdAsync(userId, cancellationToken);
        return orders.Select(x => x.ToDto()).ToList();
    }

    public async Task<bool> SetStatusAsync(
        int orderId,
        OrderStatus newStatus,
        DateTime? completedAt = null,
        CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(orderId, cancellationToken);

        if (order is null)
            return false;

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        if (newStatus == OrderStatus.Completed)
        {
            order.CompletedAt = completedAt ?? DateTime.UtcNow;
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}