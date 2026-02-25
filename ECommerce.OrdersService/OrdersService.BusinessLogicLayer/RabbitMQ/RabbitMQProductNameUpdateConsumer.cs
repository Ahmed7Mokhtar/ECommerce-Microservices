using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrdersService.BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQProductNameUpdateConsumer : IDisposable, IRabbitMQProductNameUpdateConsumer
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQProductNameUpdateConsumer> _logger;
        private readonly ICachingService _cacheService;

        public RabbitMQProductNameUpdateConsumer(IConfiguration config, ILogger<RabbitMQProductNameUpdateConsumer> logger, ICachingService cacheService)
        {
            _config = config;
            _cacheService = cacheService;

            Console.WriteLine($"RabbitMQ_HostName: {_config["RabbitMQ_HostName"]}");
            Console.WriteLine($"RabbitMQ_UserName: {_config["RabbitMQ_UserName"]}");
            Console.WriteLine($"RabbitMQ_Password: {_config["RabbitMQ_Password"]}");
            Console.WriteLine($"RabbitMQ_Port: {_config["RabbitMQ_Port"]}");

            string hostName = _config["RabbitMQ_HostName"]!;
            string userName = _config["RabbitMQ_UserName"]!;
            string password = _config["RabbitMQ_Password"]!;
            string port = _config["RabbitMQ_Port"]!;

            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = Convert.ToInt32(port)
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _logger = logger;
        }

        public void Consume()
        {
            var routingKey = "product.update.name"; // Not needed for Fanout
            var topicRoutingKey = "product.update.*"; // any prop update
            var headers = new Dictionary<string, object>()
            {
                { "x-match", "all" },   // all or any which means all headers or any should match and it's predefined
                { "event", "product.update" },
                { "rowCount", 1 }
            };

            string queueName = "orders.product.update.name.queue";

            // Create exchange
            string exchangeName = _config["RabbitMQ_Products_Exchange"]!;
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true); //  For direct exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true); // Fanout Exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true); // Topic Exchange
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Headers, durable: true); // Headers Exchange

            // Create Message Queue
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Binding Queue to Exchange
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey); //  For direct exchange
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, string.Empty); // Fanout Exchange
            //_channel.QueueBind(queue: queueName, exchange: exchangeName, topicRoutingKey); // Topic Exchange
            _channel.QueueBind(queue: queueName, exchange: exchangeName, string.Empty, arguments: headers); // Headers Exchange

            EventingBasicConsumer consumer = new(_channel);

            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    var productMessage = JsonSerializer.Deserialize<ProductDTO>(message);
                    _logger.LogInformation("Product name updated: {@Id}, new name: {@name}", productMessage?.ProductId, productMessage?.ProductName);

                    await _cacheService.SetCacheAsync($"product-{productMessage?.ProductId}", productMessage, new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                    );
                }
            };

            _channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
