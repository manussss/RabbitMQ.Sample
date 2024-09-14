using RabbitMQ.Sample.Common.Messaging;
using RabbitMQ.Sample.TopicExchange.Producer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<WorkerTwo>();

builder.Services.AddSingleton<IPublisher, Publisher>();

var host = builder.Build();
host.Run();
