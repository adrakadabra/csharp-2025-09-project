using Microsoft.EntityFrameworkCore;
using OrdersService.Api.Domain.Entities;
using OrdersService.Api.Infrastructure.Datas;
using OrdersService.Api.Infrastructure.Interfaces;

namespace OrdersService.Api.Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrdersDbContext _dbContext;

    public OrdersRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);
    }

    public Task<List<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(x => x.Items)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}