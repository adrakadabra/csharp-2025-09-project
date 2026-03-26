using OrdersService.Api.Domain.Entities;

namespace OrdersService.Api.Infrastructure.Interfaces;

public interface IOrdersRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}