using RabbitMQ.Client;
using RabbitMQ.Sample.Common.Messaging;

namespace RabbitMQ.Sample.TopicExchange.Producer
{
    public class Worker(
        ILogger<Worker> logger,
        IPublisher publisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            publisher.ConfigureChannel(new MessagingConfiguration
            {
                RoutingKey = "topic.routing.*",
                Queue = "sample.topicqueue",
                Exchange = "sample.topicexchange",
                ExchangeType = ExchangeType.Topic
            });

            var count = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = $"Message sent: {count++}";

                //Topic exchanges route messages to one or many queues based on matching
                //between a message routing key and the pattern that was used to bind a queue to an exchange.
                //When special characters * (star) and # (hash) aren't used in bindings,
                //the topic exchange will behave just like a direct one.
                //* (star) can substitute for exactly one word.
                //# (hash) can substitute for zero or more words.
                publisher.Publish(message);

                logger.LogInformation("[TopicExchange] Sent {message}", message);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
