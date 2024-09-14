using RabbitMQ.Stream.Client.Reliable;
using RabbitMQ.Stream.Client;
using System.Text;

namespace RabbitMQ.Sample.Stream.Producer
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

                //Ideal para logs
                //em streams as mensagens permanecem no stream e podem ser lidas múltiplas vezes ou a partir de diferentes pontos no tempo
                var producer = await RabbitMQ.Stream.Client.Reliable.Producer.Create(new ProducerConfig(streamSystem, "hello-stream"));

                await producer.Send(new Message(Encoding.UTF8.GetBytes("stream message")));

                _logger.LogInformation("Stream sent");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
