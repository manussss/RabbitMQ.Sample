using RabbitMQ.Client;
using RabbitMQ.Sample.Common.Messaging;

namespace RabbitMQ.Sample.FanoutExchange.Producer
{
    public class Worker(
        ILogger<Worker> logger,
        IPublisher publisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            publisher.ConfigureChannel(new MessagingConfiguration
            {
                RoutingKey = "",
                Queue = "sample.fanoutqueue",
                Exchange = "sample.fanoutexchange",
                ExchangeType = ExchangeType.Fanout
            });

            publisher.ConfigureChannel(new MessagingConfiguration
            {
                RoutingKey = "",
                Queue = "sample.fanoutqueue.two",
                Exchange = "sample.fanoutexchange",
                ExchangeType = ExchangeType.Fanout
            });

            var count = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = $"Message sent: {count++}";

                publisher.Publish(message);

                logger.LogInformation("[FanoutExchange] Sent {message}", message);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
