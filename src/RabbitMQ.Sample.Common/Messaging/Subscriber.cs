using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMQ.Sample.Common.Messaging
{
    public class Subscriber : ISubscriber
    {
        public IModel Channel { get; }
        private readonly IConnection _connection;
        private readonly IAsyncConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;

        public Subscriber(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionFactory = new ConnectionFactory
            {
                UserName = _configuration.GetSection("RabbitMq:UserName").Value,
                Password = _configuration.GetSection("RabbitMq:Password").Value,
                HostName = _configuration.GetSection("RabbitMq:HostName").Value,
                Port = int.Parse(_configuration.GetSection("RabbitMq:Port").Value),
                VirtualHost = _configuration.GetSection("RabbitMq:VirtualHost").Value,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(300),
                DispatchConsumersAsync = true
            };

            _connection = _connectionFactory.CreateConnection();
            Channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
