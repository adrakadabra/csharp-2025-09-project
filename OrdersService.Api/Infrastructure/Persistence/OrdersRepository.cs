using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Domain;

namespace OrdersService.Api.Infrastructure.Persistence
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDbContext _db;

        public OrdersRepository(OrdersDbContext db)
        {
            _db = db;
        }

        public Task<Order> GetByIdAsync(int id)
        {
            return _db.Orders.Include(x => x.Items).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
        }

        public Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return _db.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}