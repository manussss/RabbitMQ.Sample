using RabbitMQ.Client;

namespace RabbitMQ.Sample.Common.Messaging
{
    public interface IPublisher : IDisposable
    {
        IModel Channel { get; }
        void ConfigureChannel(MessagingConfiguration messagingConfiguration);
        void Publish<T>(T message, IBasicProperties? properties = null) where T : class;
    }
}
