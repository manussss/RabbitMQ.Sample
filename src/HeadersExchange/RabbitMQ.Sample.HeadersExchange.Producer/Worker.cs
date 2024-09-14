using RabbitMQ.Client;
using RabbitMQ.Sample.Common.Messaging;

namespace RabbitMQ.Sample.HeadersExchange.Producer
{
    public class Worker(
        ILogger<Worker> logger,
        IPublisher publisher) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueArguments = new Dictionary<string, object>
            {
                { "x-match", "any" }, // pode ser "all" para exigir que todas as chaves correspondam
                { "format", "json" },
                { "type", "info" }
            };

            publisher.ConfigureChannel(new MessagingConfiguration
            {
                RoutingKey = "",
                Queue = "sample.headersqueue",
                Exchange = "sample.headersexchange",
                ExchangeType = ExchangeType.Headers,
                QueueArguments = queueArguments
            });

            var count = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = $"Message sent: {count++}";

                var properties = publisher.Channel.CreateBasicProperties();
                properties.Headers = queueArguments;

                /*
                A headers exchange is designed for routing on multiple attributes that are more
                easily expressed as message headers than a routing key.
                Headers exchanges ignore the routing key attribute.
                It is possible to bind a queue to a headers exchange using more than one header for matching. In this case, 
                the broker needs one more piece of information from the application developer, namely, should it consider messages 
                with any of the headers matching, or all of them? This is what the "x-match" binding argument is for. When the 
                "x-match" argument is set to "any", just one matching header value is sufficient. Alternatively, setting "x-match"
                to "all" mandates that all the values must match.
                 */
                publisher.Publish(message, properties);

                logger.LogInformation("[HeadersExchange] Sent {message}", message);

                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
