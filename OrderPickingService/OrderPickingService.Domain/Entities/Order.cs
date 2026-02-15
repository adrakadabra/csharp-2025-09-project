using OrderPickingService.Domain.Enums;

namespace OrderPickingService.Domain.Entities;

public sealed class Order
{
    public long Id  { get; }
    public long ExternalId { get; private set;  } 
    public OrderStatus OrderStatus { get; private set;  }
    public List<OrderItem> Items { get; private set;  }

    public static Order Create(long externalId, List<OrderItem> items)
    {
        return new Order(0, externalId, OrderStatus.Available, items);
    }
    
    public static Order Load(
        long id, 
        long externalId, 
        OrderStatus orderStatus,
        List<OrderItem> items)
    {
        return new Order(id, externalId, orderStatus, items);
    }

    private Order(long id, long externalId, OrderStatus orderStatus, List<OrderItem> items)
    {
        Id = id;
        ExternalId = externalId;
        OrderStatus = orderStatus;
        Items = items;
    }
}