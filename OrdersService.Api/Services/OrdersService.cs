using OrdersService.Api.Domain;
using OrdersService.Api.DTO;
using OrdersService.Api.Infrastructure.Messaging;
using OrdersService.Api.Infrastructure.Persistence;
using OrdersService.Api.Infrastructure.Warehouse;

namespace OrdersService.Api.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _repository;
        private readonly IWarehouseClient _warehouseClient;
        private readonly IMessageBus _messageBus;
        public OrdersService(IOrdersRepository repository, IWarehouseClient warehouseClient, IMessageBus messageBus)
        {
            _repository = repository;
            _warehouseClient = warehouseClient;
            _messageBus = messageBus;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, string userJwt, CreateOrderRequest request)
        {
            if (request == null || request.Items.Count == 0)
                throw new InvalidOperationException("Заказ должен содержать хотя бы один товар");

            if (request.Items.Any(i => i.Quantity <= 0))
                throw new InvalidOperationException("Количество по каждому товару должно быть положительным.");

            var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();

            var products = await _warehouseClient.GetProductsByIdsAsync(productIds, userJwt);

            if (products.Count != productIds.Count)
                throw new InvalidOperationException("Некоторые товары не найдены в сервисе склада.");

            var now = DateTime.UtcNow;

            if (products.Any(p => p.ExpirationDate != null && p.ExpirationDate <= now))
                throw new InvalidOperationException("В заказе присутствуют просроченные товары.");

            var order = new Order
            {
                UserId = userId,
                OrderStatus = OrderStatus.Created,
                CreateAt = now,
                UpdateAt = now,
            };

            foreach (var item in request.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }

            await _repository.AddAsync(order);
            await _repository.SaveChangesAsync();

            var message = new PickOrderMessage
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Items = order.Items.Select(i => new PickOrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                }).ToList()
            };

            await _messageBus.PublishPickOrderAsync(message);
            return MapToDto(order);
        }

        public async Task<OrderDto> GetByIdAsync(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null)
                return null;

            return MapToDto(order);
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _repository.GetByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }

        public async Task SetStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null)
                return;

            order.OrderStatus = newStatus;
            order.UpdateAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.OrderStatus,
                CreateAt = order.CreateAt,
                UpdateAt = order.UpdateAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}