namespace RabbitMQ.Sample.Common.Messaging
{
    public class MessagingConfiguration
    {
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string ExchangeType { get; set; }
        public IDictionary<string, object> QueueArguments { get; set; }
        public IDictionary<string, object> ExchangeArguments { get; set; }
        public IDictionary<string, object> BindArguments { get; set; }
    }
}
