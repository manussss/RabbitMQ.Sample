using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;
using System.Text;

namespace RabbitMQ.Sample.Stream.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var streamSystem = await StreamSystem.Create(new StreamSystemConfig()); //user e pwd default

                await streamSystem.CreateStream(new StreamSpec("hello-stream")
                {
                    MaxLengthBytes = 5_000_000_000
                });

                var consumer = await RabbitMQ.Stream.Client.Reliable.Consumer.Create(new ConsumerConfig(streamSystem, "hello-stream")
                {
                    OffsetSpec = new OffsetTypeFirst(),
                    MessageHandler = async (stream, _, _, message) =>
                    {
                        _logger.LogInformation("Stream received: {message}", Encoding.UTF8.GetString(message.Data.Contents));
                        await Task.CompletedTask;
                    }
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
