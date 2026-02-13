using OrderPickingService.Services.Order.Abstractions;
using OrderPickingService.Services.Order.Dtos;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Services.Order;

internal sealed class OrderService(IOrderRepository pickerRepository) : IOrderService
{
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
    {
        var order = await pickerRepository.CreateAsync(createOrderDto.ToOrder(), cancellationToken);

        return order.ToOrderDto();
    }
}