using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Enums;
using OrderPickingService.Infrastructure.Database.Entities.Order;
using OrderPickingService.Infrastructure.Database.Entities.Picker;
using OrderPickingService.Infrastructure.Database.Entities.PickingSession;

namespace OrderPickingService.Infrastructure.Database.Seed;

internal sealed class DataSeeder : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        SeedPickersFromJson(modelBuilder);
        SeedOrdersFromJson(modelBuilder);
    }
    
    private void SeedPickersFromJson(ModelBuilder modelBuilder)
    {
        var pickers = JsonSeedDataLoader.LoadPickersFromJson();
        modelBuilder.Entity<PickerEntity>().HasData(pickers);
    }

    private void SeedOrdersFromJson(ModelBuilder modelBuilder)
    {
        var orders = JsonSeedDataLoader.LoadOrdersFromJson();
        var orderItems = new List<OrderItemEntity>();
    
        foreach (var order in orders)
        {
            orderItems.AddRange(order.OrderItems);
            
            foreach (var item in order.OrderItems)
            {
                item.OrderId = order.Id;
            }
            
            order.OrderItems = new List<OrderItemEntity>();
        }
    
        modelBuilder.Entity<OrderEntity>().HasData(orders);
        modelBuilder.Entity<OrderItemEntity>().HasData(orderItems);
    }
}