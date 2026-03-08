using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Domain.Entities;
using OrdersService.Api.Domain.Enums;

namespace OrdersService.Api.Application.Interfaces;

public interface IOrdersService
{
    public Task<List<Order>> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken = default);

    Task<OrderDto?> GetByIdAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<List<OrderDto>> GetUserOrdersAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<bool> SetStatusAsync(
        int orderId,
        OrderStatus newStatus,
        DateTime? completedAt = null,
        CancellationToken cancellationToken = default);

    Task<OrderDto> CreateOrderAsync(
        string userId,
        string userJwt,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default);
}