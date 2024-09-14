using RabbitMQ.Client;
using RabbitMQ.Sample.Common.Messaging;

namespace RabbitMQ.Sample.DirectExchange
{
    public class Worker(
        ILogger<Worker> logger,
        IPublisher publisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            publisher.ConfigureChannel(new MessagingConfiguration
            {
                RoutingKey = "direct.routing.key",
                Queue = "sample.directqueue",
                Exchange = "sample.directexchange",
                ExchangeType = ExchangeType.Direct
            });

            var count = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = $"Message sent: {count++}";

                publisher.Publish(message);
                
                logger.LogInformation("[DirectExchange] Sent {message}", message);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
