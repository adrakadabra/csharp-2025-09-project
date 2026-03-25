using OrderPickingService.Domain.Enums;

namespace OrderPickingService.Domain.Entities;

public sealed class Order
{
    public long Id  { get; }
    public Guid ExternalId { get; private set;  } 
    public OrderStatus OrderStatus { get; private set;  }

    public string UserId { get; private set; } = null!;
    public List<OrderItem> Items { get; private set;  }

    public void ClaimOrder()
    {
        OrderStatus = OrderStatus.Picking;
    }
    
    public static Order Create(Guid externalId, string userId, List<OrderItem> items)
    {
        return new Order(0, externalId, userId, OrderStatus.Available, items);
    }
    
    public static Order Load(
        long id, 
        Guid externalId, 
        string userId,
        OrderStatus orderStatus,
        List<OrderItem> items)
    {
        return new Order(id, externalId, userId, orderStatus, items);
    }

    private Order(long id, Guid externalId, string userId, OrderStatus orderStatus, List<OrderItem> items)
    {
        Id = id;
        ExternalId = externalId;
        OrderStatus = orderStatus;
        Items = items;
        UserId = userId;
    }

    public void Complete()
    {
        OrderStatus = OrderStatus.Picked;
    }
}