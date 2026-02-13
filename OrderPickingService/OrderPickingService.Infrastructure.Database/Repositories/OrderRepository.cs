using OrderPickingService.Domain.Entities;
using OrderPickingService.Infrastructure.Database.Entities.Order;

namespace OrderPickingService.Infrastructure.Database.Repositories;
using Services.Repositories.Abstractions; 

internal sealed class OrderRepository(DatabaseContext databaseContext) : IOrderRepository
{
    public async Task<Order> CreateAsync(Order create, CancellationToken cancellationToken = default)
    {
        var newOrder = create.ToOrderEntity();
        await databaseContext.Orders.AddAsync(newOrder, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return newOrder.ToOrder();
    }
}