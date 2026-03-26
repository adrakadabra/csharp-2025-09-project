using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Entities;
using OrderPickingService.Infrastructure.Database.Entities.Order;

namespace OrderPickingService.Infrastructure.Database.Repositories;
using Services.Repositories.Abstractions; 

internal sealed class OrderRepository(DatabaseContext databaseContext) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var foundOrder = await databaseContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
        
        return foundOrder?.ToOrder();
    }

    public async Task<Order?> GetOrderByExternalId(Guid externalOrderId, CancellationToken cancellationToken = default)
    {
        var foundOrder = await databaseContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(order => order.ExternalId == externalOrderId, cancellationToken);
        
        return foundOrder?.ToOrder();
    }

    public async Task<Order> CreateAsync(Order create, CancellationToken cancellationToken = default)
    {
        var newOrder = create.ToOrderEntity();
        await databaseContext.Orders.AddAsync(newOrder, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return newOrder.ToOrder();
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        var entity = await databaseContext.Orders.FindAsync([order.Id], cancellationToken: cancellationToken);
        
        if(entity == null)
            throw new KeyNotFoundException($"Order with id {order.Id} not found in database");
        
        entity.ExternalId = order.ExternalId;
        entity.OrderStatus = order.OrderStatus;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "system";
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}