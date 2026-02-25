using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductsService.BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQPublisher(IConfiguration config)
        {
            _config = config;

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
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public void Publish<T>(string routingKey, T message)
        {
            string messageJson = JsonSerializer.Serialize(message, _jsonOptions);
            byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

            // Create exchange
            string exchangeName = _config["RabbitMQ_Products_Exchange"]!;
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);   // Direct Exchange
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true); // Fanout Exchange
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true); // Topic Exchange

            // publich message
            //_channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: messageBodyInBytes); //  For direct exchange
            //_channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, basicProperties: null, body: messageBodyInBytes); // Fanout Exchange
            _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: messageBodyInBytes); // Topic Exchange
        }

        public void Publish<T>(Dictionary<string, object> headers, T message)
        {
            string messageJson = JsonSerializer.Serialize(message, _jsonOptions);
            byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

            // Create exchange
            string exchangeName = _config["RabbitMQ_Products_Exchange"]!;
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Headers, durable: true); // Headers Exchange

            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.Headers = headers;

            // publich message
            _channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, basicProperties: basicProperties, body: messageBodyInBytes); // Headers Exchange
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
