using OrdersService.Api.Domain;

namespace OrdersService.Api.Infrastructure.Persistence
{
    public interface IOrdersRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetByUserIdAsync(int userId);
        Task AddAsync (Order order);
        Task SaveChangesAsync();
    }
}