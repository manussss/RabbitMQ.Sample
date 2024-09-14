using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Sample.Common.Messaging
{
    public class Publisher : IPublisher
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IAsyncConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private string? _exchange;
        private string? _routingKey;

        public IModel Channel => _channel;

        public Publisher(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionFactory = new ConnectionFactory
            {
                UserName = _configuration.GetSection("RabbitMq:UserName").Value,
                Password = _configuration.GetSection("RabbitMq:Password").Value,
                HostName = _configuration.GetSection("RabbitMq:HostName").Value,
                Port = int.Parse(_configuration.GetSection("RabbitMq:Port").Value!),
                VirtualHost = _configuration.GetSection("RabbitMq:VirtualHost").Value,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(300),
                DispatchConsumersAsync = true
            };

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }


        public void Publish<T>(T message, IBasicProperties? properties = null) where T : class
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            var messageBytes = Encoding.UTF8.GetBytes(serializedMessage);

            _channel.BasicPublish(_exchange, _routingKey, properties, messageBytes);
        }

        public void ConfigureChannel(MessagingConfiguration messagingConfiguration)
        {
            _exchange = messagingConfiguration.Exchange;
            _routingKey = messagingConfiguration.RoutingKey;

            _channel.QueueDeclare(messagingConfiguration.Queue, true, false, false, messagingConfiguration.QueueArguments);
            _channel.ExchangeDeclare(_exchange, messagingConfiguration.ExchangeType, true, false, messagingConfiguration.ExchangeArguments);
            _channel.QueueBind(messagingConfiguration.Queue, _exchange, _routingKey, messagingConfiguration.QueueArguments);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
