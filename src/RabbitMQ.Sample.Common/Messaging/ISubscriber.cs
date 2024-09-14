using RabbitMQ.Client;

namespace RabbitMQ.Sample.Common.Messaging
{
    public interface ISubscriber : IDisposable
    {
        public IModel Channel { get; }
    }
}
