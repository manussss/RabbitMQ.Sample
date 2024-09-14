using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQ.Sample.Common.Messaging;
using System.Text;

namespace RabbitMQ.Sample.TopicExchange.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISubscriber _subscriber;
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public Worker(
            ILogger<Worker> logger,
            ISubscriber subscriber,
            IConfiguration configuration)
        {
            _logger = logger;
            _subscriber = subscriber;
            _configuration = configuration;
            _channel = _subscriber.Channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var queue = _configuration.GetSection("RabbitMq:QueueName").Value;

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += ProcessMessageAsync;

                _channel.BasicQos(0, prefetchCount: 5, true);
                _channel.BasicConsume(queue, false, consumer);

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs args)
        {
            try
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());

                _logger.LogInformation("Received message: {message}", message);

                _channel.BasicAck(args.DeliveryTag, true);

                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred.");
            }
        }
    }
}
