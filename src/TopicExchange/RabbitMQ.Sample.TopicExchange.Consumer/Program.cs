using RabbitMQ.Sample.Common.Messaging;
using RabbitMQ.Sample.TopicExchange.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<ISubscriber, Subscriber>();

var host = builder.Build();
host.Run();
