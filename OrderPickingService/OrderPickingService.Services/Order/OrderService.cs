using OrderPickingService.Services.Order.Abstractions;
using OrderPickingService.Services.Order.Dtos;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Services.Order;

internal sealed class OrderService(
    IOrderRepository orderRepository) : IOrderService
{
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.CreateAsync(createOrderDto.ToOrder(), cancellationToken);
        
        return order.ToOrderDto();
    }

    public async Task<OrderDto> GetOrderByExternalId(Guid externalOrderId, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderByExternalId(externalOrderId, cancellationToken);
        
        if(order == null)
        {
            throw new KeyNotFoundException($"Order with id = {externalOrderId} not found");
        }
        
        return order.ToOrderDto();
    }
}