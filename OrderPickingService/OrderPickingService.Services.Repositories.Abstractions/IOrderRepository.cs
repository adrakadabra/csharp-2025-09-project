using OrderPickingService.Domain.Entities;

namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IOrderRepository
{
    // Task<Order?> GetByIdAsync(int id);
    // Task<List<Order>> GetOrdersAsync(int id);
    Task<Order> CreateAsync(Order create, CancellationToken cancellationToken = default);
}