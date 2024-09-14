using RabbitMQ.Sample.Common.Messaging;
using RabbitMQ.Sample.DirectExchange;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IPublisher, Publisher>();

var host = builder.Build();
host.Run();
