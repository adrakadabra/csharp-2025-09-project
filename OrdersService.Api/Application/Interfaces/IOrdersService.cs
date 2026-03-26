using OrdersService.Api.Application.DTOs;
using OrdersService.Api.Domain.Enums;

namespace OrdersService.Api.Application.Interfaces;

public interface IOrdersService
{
    Task<OrderDto> CreateOrderAsync(
        string userId,
        string userJwt,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default);

    Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<OrderDto>> GetUserOrdersAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<bool> SetStatusAsync(
        int orderId,
        OrderStatus status,
        DateTime? completedAt = null,
        CancellationToken cancellationToken = default);

    Task<bool> SetStatusAsync(
        Guid orderNumber,
        OrderStatus newStatus,
        DateTime? completedAt = null,
        CancellationToken cancellationToken = default);

    Task<bool> CancelOrderAsync(
        int orderId,
        string userId,
        string userJwt,
        CancellationToken cancellationToken = default);
}