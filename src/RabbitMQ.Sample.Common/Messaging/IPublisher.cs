namespace RabbitMQ.Sample.Common.Messaging
{
    public interface IPublisher : IDisposable
    {
        void ConfigureChannel(MessagingConfiguration messagingConfiguration);
        void Publish<T>(T message) where T : class;
    }
}
