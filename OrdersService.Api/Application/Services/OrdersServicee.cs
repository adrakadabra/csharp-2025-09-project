using FluentValidation;
using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Application.Interfaces;
using OrdersService.Api.Application.Mappers;
using OrdersService.Api.Domain.Entities;
using OrdersService.Api.Domain.Enums;
using OrdersService.Api.Infrastructure.Clients;
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
    private readonly IOrderPickingClient _orderPickingClient;
    private IOrdersRepository object1;
    private IWarehouseClient object2;
    private IOrderMessagePublisher object3;
    private IValidator<CreateOrderRequest> validator;

    //private IOrdersRepository object1;
    //private IWarehouseClient object2;
    //private IOrderMessagePublisher object3;
    //private IValidator<CreateOrderRequest> validator;

    public OrdersServicee(
        IOrdersRepository repository,
        IWarehouseClient warehouseClient,
        IOrderMessagePublisher orderMessagePublisher,
        IOrderPickingClient orderPickingClient,
        IValidator<CreateOrderRequest> validator)
    {
        _repository = repository;
        _warehouseClient = warehouseClient;
        _orderMessagePublisher = orderMessagePublisher;
        _orderPickingClient = orderPickingClient;
        _validator = validator;
    }

    public OrdersServicee(IOrdersRepository object1, IWarehouseClient object2, IOrderMessagePublisher object3, IValidator<CreateOrderRequest> validator)
    {
        this.object1 = object1;
        this.object2 = object2;
        this.object3 = object3;
        this.validator = validator;
    }

    //public OrdersServicee(IOrdersRepository object1, IWarehouseClient object2, IOrderMessagePublisher object3, IValidator<CreateOrderRequest> validator)
    //{
    //    this.object1 = object1;
    //    this.object2 = object2;
    //    this.object3 = object3;
    //    this.validator = validator;
    //}

    public async Task<OrderDto> CreateOrderAsync(
    string userId,
    string userJwt,
    CreateOrderRequest request,
    CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new InvalidOperationException(validationResult.Errors.First().ErrorMessage);

        var groupedItems = request.Items
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        var productIds = groupedItems.Select(x => x.ProductId).ToList();

        var products = await _warehouseClient.GetProductsByIdsAsync(
            productIds,
            userJwt,
            cancellationToken);

        if (products.Count != productIds.Count)
            throw new InvalidOperationException("Некоторые товары не найдены в сервисе склада.");

        foreach (var requestedItem in groupedItems)
        {
            var warehouseProduct = products.First(x => x.Id == requestedItem.ProductId);

            if (warehouseProduct.AvailableQuantity < requestedItem.Quantity)
                throw new InvalidOperationException($"Недостаточно остатка на складе по товару {requestedItem.ProductId}. " +
                                                    $"Доступное количество {warehouseProduct.AvailableQuantity}. " +
                                                    $"Запрашиваемое количество {requestedItem.Quantity}");
        }

        var orderNumber = Guid.NewGuid();

        var reserveRequest = new ReserveProductsRequest
        {
            OrderNumber = orderNumber,
            Items = groupedItems.Select(x =>
            {
                var product = products.First(p => p.Id == x.ProductId);

                return new ReserveItemRequest
                {
                    Article = product.Article,
                    Quantity = x.Quantity
                };
            }).ToList()
        };

        await _warehouseClient.ReserveProductsAsync(
            reserveRequest,
            userJwt,
            cancellationToken);

        var now = DateTime.UtcNow;

        var order = new Order
        {
            OrderNumber = orderNumber,
            UserId = userId,
            Status = OrderStatus.Created,
            CreatedAt = now,
            UpdatedAt = now,
            Items = groupedItems.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList()
        };

        await _repository.AddAsync(order, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        try
        {
            var pickingRequest = new CreateAssemblyOrderRequest
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Items = groupedItems.Select(x =>
                {
                    var product = products.First(p => p.Id == x.ProductId);

                    return new CreateAssemblyOrderItemRequest
                    {
                        ProductExternalId = product.Id,
                        ProductSku = product.Article,
                        ProductName = product.Name,
                        Quantity = x.Quantity,
                        Price = product.Price,
                        Category = product.CategoryName
                    };
                }).ToList()
            };

            await _orderPickingClient.CreateOrderAsync(
                pickingRequest,
                userJwt,
                cancellationToken);
        }
        catch
        {
            await _warehouseClient.CancelReservationAsync(order.OrderNumber, userJwt, cancellationToken);

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync(cancellationToken);

            throw;
        }

        // var message = new PickOrderMessage
        // {
        //     OrderId = order.Id,
        //     UserId = order.UserId,
        //     Items = order.Items.Select(i => new PickOrderItemMessage
        //     {
        //         ProductId = i.ProductId,
        //         Quantity = i.Quantity
        //     }).ToList()
        // };
        //
        // await _orderMessagePublisher.SendPickOrderAsync(message, cancellationToken);

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

    public async Task<bool> CancelOrderAsync(
    int orderId,
    string userId,
    string userJwt,
    CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(orderId, cancellationToken);
        if (order is null)
            return false;

        if (order.UserId != userId)
            return false;

        if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Заказ уже завершён или отменён.");

        await _warehouseClient.CancelReservationAsync(order.OrderNumber, userJwt, cancellationToken);

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}