using OrderPickingService.Services.Order.Dtos;

namespace OrderPickingService.Services.Order.Abstractions;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default);
}