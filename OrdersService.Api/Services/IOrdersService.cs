using OrdersService.Api.Domain;
using OrdersService.Api.DTO;

namespace OrdersService.Api.Services
{
    public interface IOrdersService
    {
        Task<OrderDto> CreateOrderAsync(int userId, string userJwt, CreateOrderRequest request);
        Task<OrderDto> GetByIdAsync(int orderId);
        Task<List<OrderDto>> GetUserOrdersAsync(int userId);
        Task SetStatusAsync(int orderId, OrderStatus newStatus);
    }
}