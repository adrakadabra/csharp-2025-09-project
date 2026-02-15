using RabbitMQ.Client;

namespace OrdersService.Api.Infrastructure.Messaging
{
    public class OrderCompletedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConnectionFactory _factory;
        private readonly string _exchangeName;
        private readonly string _queueName;

        public OrderCompletedConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;

            var host = configuration["Rabbit:Host"] ?? "localhost";
            var user = configuration["Rabbit:User"] ?? "guest";
            var password = configuration["Rabbit:Password"] ?? "guest";
            _exchangeName = configuration["Rabbit:Exchange"] ?? "orders-exchange";
            _queueName = configuration["Rabbit:CompletedQueue"] ?? "orders-completed-queue";

            _factory = new ConnectionFactory
            {
                HostName = host,
                UserName = user,
                Password = password,
                //DispatchConsumersAsync = true
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO не разобрался как с кроликом соединяться, жду учеба дальше, мб понятнее станет

            //var connection = _factory.CreateConnection();
            //var channel = connection.CreateModel();

            //channel.ExchangeDeclare(
            //    exchange: _exchangeName,
            //    //type: ExchangeType.Topic,
            //    durable: true,
            //    autoDelete: false);

            //channel.QueueDeclare(
            //    queue: _queueName,
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: false);

            //channel.QueueBind(
            //    queue: _queueName,
            //    exchange: _exchangeName,
            //    routingKey: "orders.completed");

            //var consumer = new AsyncEventingBasicConsumer(channel);
            //consumer.ReceivedAsync += async (sender, args) =>
            //{
            //    var bodyBytes = args.Body.ToArray();
            //    var json = Encoding.UTF8.GetString(bodyBytes);

            //    OrderCompletedMessage message;
            //    try
            //    {
            //        message = JsonSerializer.Deserialize<OrderCompletedMessage>(json);
            //    }
            //    catch
            //    {
            //        channel.BasicAck(args.DeliveryTag, false);
            //        return;
            //    }

            //    using var scope = _scopeFactory.CreateScope();
            //    var ordersService = scope.ServiceProvider.GetRequiredService<IOrdersService>();

            //    await ordersService.SetStatusAsync(message.OrderId, OrderStatus.Completed);

            //    channel.BasicAck(args.DeliveryTag, false);
            //};

            //channel.BasicConsume(
            //    queue: _queueName,
            //    autoAck: false,
            //    consumer: consumer);

            return Task.CompletedTask;
        }
    }
}