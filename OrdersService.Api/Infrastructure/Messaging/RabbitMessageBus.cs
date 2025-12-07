using IRmqConnectionFactory = RabbitMQ.Client.IConnectionFactory;
using RmqConnectionFactory = RabbitMQ.Client.ConnectionFactory;

namespace OrdersService.Api.Infrastructure.Messaging
{
    namespace OrdersService.Api.Infrastructure.Messaging
    {
        public class RabbitMessageBus : IMessageBus
        {
            private readonly IRmqConnectionFactory _factory;
            private readonly string _exchangeName;

            public RabbitMessageBus(IConfiguration configuration)
            {
                var host = configuration["Rabbit:Host"] ?? "localhost";
                var user = configuration["Rabbit:User"] ?? "guest";
                var password = configuration["Rabbit:Password"] ?? "guest";
                _exchangeName = configuration["Rabbit:Exchange"] ?? "orders-exchange";

                _factory = new RmqConnectionFactory
                {
                    HostName = host,
                    UserName = user,
                    Password = password,
                    //DispatchConsumersAsync = true
                };
            }

            public Task PublishPickOrderAsync(PickOrderMessage message)
            {
                //TODO не разобрался как с кроликом соединяться, жду учеба дальше, мб понятнее станет

                //using var connection = _factory.CreateConnection();
                //using var channel = connection.CreateModel();

                //channel.ExchangeDeclare(
                //    exchange: _exchangeName,
                //    type: ExchangeType.Topic,
                //    durable: true,
                //    autoDelete: false);

                //var body = JsonSerializer.SerializeToUtf8Bytes(message);

                //var props = channel.CreateBasicProperties();
                //props.Persistent = true;

                //channel.BasicPublish(
                //    exchange: _exchangeName,
                //    routingKey: "orders.pick",
                //    basicProperties: props,
                //    body: body);

                return Task.CompletedTask;
            }
        }
    }
}