using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderByExternalId(Guid externalOrderId, CancellationToken cancellationToken = default);
    Task<Order> CreateAsync(Order create, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}